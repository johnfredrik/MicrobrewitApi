using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper
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
           });
        }

        
    }
}