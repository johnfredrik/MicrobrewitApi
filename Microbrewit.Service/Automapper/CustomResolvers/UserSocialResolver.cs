using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Api.Service.Util;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class UserSocialResolver : ValueResolver<User,Dictionary<string,string>>
    {
        protected override Dictionary<string, string> ResolveCore(User source)
        {
            var socials = new Dictionary<string, string>();
            if (source.Socials == null) return socials;
            foreach (var social in source.Socials.GroupBy(s => s.Site).Select(s => s.First()))
            {
                socials.Add(social.Site,social.Url);
            }
            return socials;
        }
    }
}
