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
    public class HopFlavoursResolver : ValueResolver<Hop, IList<string>>
    {
        protected override IList<string> ResolveCore(Hop hop)
        {
            return (from hopFlavour in hop.Flavours where hopFlavour.Flavour != null select hopFlavour.Flavour.Name).ToList();
        }
    }
}