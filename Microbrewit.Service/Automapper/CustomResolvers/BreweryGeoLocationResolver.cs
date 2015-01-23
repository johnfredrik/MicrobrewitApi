using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
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