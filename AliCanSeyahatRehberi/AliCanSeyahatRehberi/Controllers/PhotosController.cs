using AliCanSeyahatRehberi.Data;
using AliCanSeyahatRehberi.Dtos;
using AliCanSeyahatRehberi.Helpers;
using AliCanSeyahatRehberi.Models;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AliCanSeyahatRehberi.Controllers
{
    [Route("api/cities/{cityid}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IApprRepository _appRepository;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryconfig;
        private Cloudinary _cloudinary;
        public PhotosController(IApprRepository apprRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryconfig)
        {
            _appRepository = apprRepository;
            _mapper = mapper;
            _cloudinaryconfig = cloudinaryconfig;
             Account account = new Account(_cloudinaryconfig.Value.CloudName, _cloudinaryconfig.Value.ApiKey,
             _cloudinaryconfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        [HttpPost]
        public IActionResult AddPhotoForCity(int cityid,[FromBody]PhotoForCreationDto photoForCreationDto)
        {
            var city = _appRepository.GetCityById(cityid);
            if(cityid==null)
            {
                return BadRequest("bulunamadi");
            }
            var currentuserid=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
           if(currentuserid!=city.UserId)
            {
                return Unauthorized();
            }
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if(file.Length>0)
            {
               // using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                      //  File = new FileDescription(file.Name, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);

                }
            }
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            var photo = _mapper.Map<Photo>(photoForCreationDto);
            photo.City = city;
            if(!city.Photos.Any(p=>p.IsMain))
            {
                photo.IsMain = true;
            }
            city.Photos.Add(photo);
            if(_appRepository.SaveAll())
            {
                var photosReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto",new {id=photo.Id },photosReturn);


            }
            return BadRequest("not found");    
        }

    }
}
