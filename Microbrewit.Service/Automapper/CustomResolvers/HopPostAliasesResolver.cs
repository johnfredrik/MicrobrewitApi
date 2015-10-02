using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class HopPostAliasesResolver : ValueResolver<HopDto, string>
    {
        protected override string ResolveCore(HopDto hop)
        {
            return string.Join(";", hop.Aliases);
        }
    }
}