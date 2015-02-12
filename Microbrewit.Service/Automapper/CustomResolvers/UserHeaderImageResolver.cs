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
    public class UserHeaderImageResolver : ValueResolver<UserDto, string>
    {
        protected override string ResolveCore(UserDto source)
        {
            if (source.HeaderImage == null) return string.Empty;
            var image = source.HeaderImage.Split('/').LastOrDefault();
            return image;
        }
    }
}
