using AliCanSeyahatRehberi.Data;
using AliCanSeyahatRehberi.Dtos;
using AliCanSeyahatRehberi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AliCanSeyahatRehberi.Controllers
{

    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IAuthRepository _authRepository;
        private IConfiguration _configuration;
        public AuthController(IAuthRepository authRepository, IConfiguration configurations)
        {
            _authRepository = authRepository;
            _configuration = configurations;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody]UserForRegisterDto userForRegisterDto)
        {
           
            var user = await _authRepository.UserExists(userForRegisterDto.UserName);
            if (await _authRepository.UserExists(userForRegisterDto.UserName))
            {
                ModelState.AddModelError("UserName", "UserName already exits");

            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };
            var createduser = await _authRepository.Register(UserToCreate, userForRegisterDto.PassWord);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var user = await _authRepository.Login(userForLoginDto.UserName, userForLoginDto.PassWord);
            if(user==null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key =Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokendecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)

                }),
                // Token ne kadar geçerli olacak aşağıda onu ayarlıyoruz
                Expires = DateTime.Now.AddDays(21),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokendecriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok();
        }
    }
}