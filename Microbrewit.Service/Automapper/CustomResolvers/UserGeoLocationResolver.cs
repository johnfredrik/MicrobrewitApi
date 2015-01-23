using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class UserGeoLocationResolver : ValueResolver<User,GeoLocationDto>
    {
        protected override GeoLocationDto ResolveCore(User brewery)
        {
            return new GeoLocationDto
            {
                Latitude = brewery.Latitude,
                Longitude = brewery.Longitude
            };
        }
    }
}