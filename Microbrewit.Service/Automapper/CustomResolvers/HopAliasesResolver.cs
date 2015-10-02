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
    public class HopAliasesResolver : ValueResolver<Hop, IList<string>>
    {
        protected override IList<string> ResolveCore(Hop hop)
        {
            if(hop.Aliases != null && hop.Aliases.Any())
                return hop.Aliases.Split(';');
            else 
                return new List<string>();
        }
    }
}