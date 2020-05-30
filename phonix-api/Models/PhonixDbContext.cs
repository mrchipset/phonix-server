using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace phonix_api.Models
{
    public class PhonixDbContext : DbContext
    {
        public PhonixDbContext(DbContextOptions<PhonixDbContext> options) : base(options)
        {

        }
        public DbSet<PictureItem> PictureItems {get; set;}  //Picture Table
    }
}
