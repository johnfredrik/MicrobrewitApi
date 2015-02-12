using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BreweryAvatarResolver : ValueResolver<BreweryDto, string>
    {
        protected override string ResolveCore(BreweryDto source)
        {
            if (source.Avatar == null) return string.Empty;
            var image = source.Avatar.Split('/').LastOrDefault();
            return image;
        }
    }
}
