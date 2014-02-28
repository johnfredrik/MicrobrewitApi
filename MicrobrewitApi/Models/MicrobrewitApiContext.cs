using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MicrobrewitApi.Models.ModelBuilder;

namespace MicrobrewitApi.Models
{
    public class MicrobrewitApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MicrobrewitApiContext() : base("name=MicrobrewitApiContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FermentableConfiguration());
            modelBuilder.Configurations.Add(new HopsConfiguration());
            modelBuilder.Configurations.Add(new OriginConfiguration());
        }

        public DbSet<Hop> Hops { get; set; }

        public DbSet<Origin> Origins { get; set; }

        public DbSet<Fermentable> Fermentables { get; set; }

        public DbSet<Grain> Grains { get; set; }

        public DbSet<LiquidExtract> LiquidExtracts { get; set; }
    
    }
}
