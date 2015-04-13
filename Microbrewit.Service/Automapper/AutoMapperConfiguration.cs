using AutoMapper;

namespace Microbrewit.Service.Automapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
           Mapper.Initialize(conf => 
           {
               conf.AddProfile(new RecipeProfile());
               conf.AddProfile(new HopProfile());
               conf.AddProfile(new MashStepProfile());
               conf.AddProfile(new BoilStepProfile());
               conf.AddProfile(new FermentationStepProfile());
               conf.AddProfile(new BrewerProfile());
               conf.AddProfile(new FermentableProfile());
               conf.AddProfile(new YeastsProfile());
               conf.AddProfile(new OtherProfile());
               conf.AddProfile(new UserProfile());
               conf.AddProfile(new BeerStyleProfile());
               conf.AddProfile(new BeerProfile());
               conf.AddProfile(new SupplierProfile());
               conf.AddProfile(new BreweryProfile());
               conf.AddProfile(new OriginProfile());
               conf.AddProfile(new GlassProfile());
               conf.AddProfile(new SpargeStepProfile());
               conf.AddProfile(new BeerXmlProfile());
           });
        }

        
    }
}