using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliCanSeyahatRehberi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace AliCanSeyahatRehberi.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }

        public DbSet<Value> Values { get; set; }//veri tabanına values adında tablo oluşturur
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
