using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microbrewit.Model.ModelBuilder;

namespace Microbrewit.Model
{
    public class MicrobrewitContext : DbContext
    {
        public DbSet<Hop> Hops { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Fermentable> Fermentables { get; set; }
        public DbSet<FermentableType> FermentableTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Yeast> Yeasts { get; set; }
        public DbSet<Other> Others { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeHop> RecipeHops { get; set; }
        public DbSet<Flavour> Flavours { get; set; }
        public DbSet<HopFlavour> HopFlavours { get; set; }

 
        public MicrobrewitContext()
            : base("name=MicrobrewitContext")
        {
           Configuration.LazyLoadingEnabled = false;
           //Configuration.ProxyCreationEnabled = false;
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
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserCredentialsConfiguration());
            modelBuilder.Configurations.Add(new RecipeConfiguration());
            modelBuilder.Configurations.Add(new RecipeHopConfiguration());
            modelBuilder.Configurations.Add(new FlavourConfiguration());
            modelBuilder.Configurations.Add(new HopFlavourConfiguration());
        }
   
    }
}
