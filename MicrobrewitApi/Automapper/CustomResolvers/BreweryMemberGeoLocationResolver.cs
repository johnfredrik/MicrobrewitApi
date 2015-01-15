using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class BreweryMemberGeoLocationResolver : ValueResolver<BreweryMember, GeoLocationDto>
    {
        
        protected override GeoLocationDto ResolveCore(BreweryMember breweryMember)
        {
            
            return new GeoLocationDto
            {
                Latitude = breweryMember.Brewery.Latitude,
                Longitude = breweryMember.Brewery.Longitude
            };
        }
    }
}