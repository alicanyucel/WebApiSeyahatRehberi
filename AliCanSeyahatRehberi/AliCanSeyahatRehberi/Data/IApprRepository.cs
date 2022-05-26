using AliCanSeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliCanSeyahatRehberi.Data
{
   public interface IApprRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool SaveAll();
        List<City> GetCities();
        List<Photo> GetPhotosByCity(int id);
        City GetCityById(int cityid);
        Photo GetPhoto(int id);
        



    }
}
