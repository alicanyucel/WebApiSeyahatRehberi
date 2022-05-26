using AliCanSeyahatRehberi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sun.security.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliCanSeyahatRehberi.Data
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<User> Login(string UserName, string Password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if(user==null)
            {
                return null;
            }
            if(!VerifyPasswordHash(Password,user.PasswordHash,user.PasswordSalt))
            {
                return null;
            }
            return user;
        }
        private bool VerifyPasswordHash(string password, byte[] UserPasswordHash, byte[] UserPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(UserPasswordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(var i=0;i<computeHash.Length;i++)
                {
                    if(computeHash[i]!=UserPasswordHash[i])
                    {
                        return false;
                    }
                    
                }
                return true;
            }
        }
        public async Task<bool> UserExists([FromBody] string userName)
        {
            var user = _context.Users.Where(t => t.UserName == userName).FirstOrDefault();
            if (user != null)
                return true;
            else
                return false;
            //if(await _context.Users.AnyAsync(x=>x.UserName==userName))
            //{
            //    return true;
            //}
            //return false;
        }
        private DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Register(User user, string Password)
        {
            byte[] passwordHash, passwordsalt;
            CreatePassHash(Password,out passwordHash,out passwordsalt);
            user.PasswordHash =passwordHash;
            user.PasswordSalt = passwordsalt;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void CreatePassHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}
