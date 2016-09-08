using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClubProject.Models
{
    public class ClubContext:DbContext
    {
        private readonly IConfigurationRoot _config;

        public ClubContext(IConfigurationRoot config, DbContextOptions options):base(options)
        {
            _config = config;
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:ClubConnectionString"]);
        }
    }
}
