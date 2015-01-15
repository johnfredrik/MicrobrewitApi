using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class BreweryGeoLocationResolver : ValueResolver<Brewery,GeoLocationDto>
    {
        protected override GeoLocationDto ResolveCore(Brewery brewery)
        {
            return new GeoLocationDto
            {
                Latitude = brewery.Latitude,
                Longitude = brewery.Longitude
            };
        }
    }
}