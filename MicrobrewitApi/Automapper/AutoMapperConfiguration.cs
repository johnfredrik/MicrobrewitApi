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
           });
        }

        
    }
}