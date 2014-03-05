using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MicrobrewitModel.ModelBuilder;

namespace MicrobrewitModel
{
    public class MicrobrewitApiContext : DbContext
    {
        public DbSet<Hop> Hops { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Fermentable> Fermentables { get; set; }
        public DbSet<FermentableType> FermentableTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Yeast> Yeasts { get; set; }
        public DbSet<Other> Others { get; set; }
 
        public MicrobrewitApiContext()
            : base("name=MicrobrewitApiContext")
        {
           Configuration.LazyLoadingEnabled = false;
           Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HopsConfiguration());
            modelBuilder.Configurations.Add(new FermentableConfiguration());
            modelBuilder.Configurations.Add(new OriginConfiguration());
            modelBuilder.Configurations.Add(new FermentableTypeConfiguration());
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new YeastConfiguration());
            modelBuilder.Configurations.Add(new OtherConfiguration());
        }

        
    
    }
}
