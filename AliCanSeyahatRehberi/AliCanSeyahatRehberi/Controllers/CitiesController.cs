using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliCanSeyahatRehberi.Data;
using AliCanSeyahatRehberi.Dtos;
using AliCanSeyahatRehberi.Models;
using AutoMapper;
namespace AliCanSeyahatRehberi.Controllers
{
    [Produces("application/json")]
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private IApprRepository _apprRepository;
        private IMapper _mapper;

        public CitiesController(IApprRepository apprRepository,IMapper mapper)
        {
            _apprRepository = apprRepository;
            _mapper = mapper;
        }
        public IActionResult GetCities()
        {
            var cities = _apprRepository.GetCities();
            var citiesReturn =_mapper.Map<List<CityForListDto>>(cities);
            return Ok(citiesReturn);
        }
        [HttpPost("add")]
       
        public IActionResult Add([FromBody] City city)
        {
            _apprRepository.Add(city);
            _apprRepository.SaveAll();
            return Ok(city);

        }
        [HttpGet]
        [Route("detail")]
        public IActionResult GetCityById (int id)
        {
            var city = _apprRepository.GetCityById(id);
            var cityReturn = _mapper.Map<CityForDetailDto>(city);
            return Ok(cityReturn);

        }
        [HttpGet]
        [Route("photos")]
        public IActionResult GetPhotosByCity(int cityid)
        {
            var photos = _apprRepository.GetPhotosByCity(cityid);
            return Ok(photos);
        }
    }
}
