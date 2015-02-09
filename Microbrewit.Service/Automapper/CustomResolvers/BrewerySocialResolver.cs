using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BrewerySocialResolver : ValueResolver<Brewery,Dictionary<string,string>>
    {
        protected override Dictionary<string, string> ResolveCore(Brewery source)
        {
            var socials = new Dictionary<string, string>();
            if (source.Socials == null) return socials;
            foreach (var social in source.Socials)
            {
                socials.Add(social.Site,social.Url);
            }
            return socials;
        }
    }
}
