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
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Yeast> Yeasts { get; set; }
        public DbSet<Other> Others { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Flavour> Flavours { get; set; }
        public DbSet<HopFlavour> HopFlavours { get; set; }
        public DbSet<MashStep> MashSteps { get; set; }
        public DbSet<MashStepHop> MashStepHops { get; set; }
        public DbSet<BoilStepHop> BoilStepHops { get; set; }
        public DbSet<FermentationStepHop> FermentationStepHop { get; set; }
        public DbSet<MashStepFermentable> MashStepFermentables { get; set; }
        public DbSet<BoilStepFermentable> BoilStepFermentables { get; set; }
        public DbSet<FermentationStepFermentable> FermentationStepFermentables { get; set; }
        public DbSet<MashStepOther> MashStepOthers { get; set; }
        public DbSet<BoilStepOther> BoilStepOthers { get; set; }
        public DbSet<FermentationStepYeast> FermentationStepYeasts { get; set; }
        public DbSet<BeerStyle> BeerStyles { get; set; }
        public DbSet<HopForm> HopForms { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<ABV> ABVs { get; set; }
        public DbSet<IBU> IBUs { get; set; }
        public DbSet<SRM> SRMs { get; set; }
        public DbSet<Beer> Beers { get; set; }
      //  public DbSet<Substitut> Substituts { get; set; }

 
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
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new YeastConfiguration());
            modelBuilder.Configurations.Add(new OtherConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserCredentialsConfiguration());
            modelBuilder.Configurations.Add(new RecipeConfiguration());
            modelBuilder.Configurations.Add(new FlavourConfiguration());
            modelBuilder.Configurations.Add(new HopFlavourConfiguration());
            modelBuilder.Configurations.Add(new MashStepConfiguration());
            modelBuilder.Configurations.Add(new FermentationStepConfiguration());
            modelBuilder.Configurations.Add(new BoilStepConfiguration());
            modelBuilder.Configurations.Add(new MashStepHopConfiguration());
            modelBuilder.Configurations.Add(new BoilStepHopConfiguration());
            modelBuilder.Configurations.Add(new FermentationStepHopConfiguration());
            modelBuilder.Configurations.Add(new MashStepFermentableConfiguration());
            modelBuilder.Configurations.Add(new BoilStepFermentableConfiguration());
            modelBuilder.Configurations.Add(new FermentationStepFermentableConfiguration());
            modelBuilder.Configurations.Add(new MashStepOtherConfiguration());
            modelBuilder.Configurations.Add(new BoilStepOtherConfiguration());
            modelBuilder.Configurations.Add(new FermentationStepOtherConfiguration());
            modelBuilder.Configurations.Add(new FermentationStepYeastConfiguration());
            modelBuilder.Configurations.Add(new BeerStyleConfiguration());
            modelBuilder.Configurations.Add(new HopFormConfiguration());
            modelBuilder.Configurations.Add(new BreweryConfiguration());
            modelBuilder.Configurations.Add(new BreweryMemberConfiguration());
            modelBuilder.Configurations.Add(new BeerConfiguration());
            modelBuilder.Configurations.Add(new ABVConfiguration());
            modelBuilder.Configurations.Add(new IBUConfiguration());
            modelBuilder.Configurations.Add(new SRMConfiguration());
            modelBuilder.Configurations.Add(new UserBeerConfiguration());

           
           
        }
   
    }
}
