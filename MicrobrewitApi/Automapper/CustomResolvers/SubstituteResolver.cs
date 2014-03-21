using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
        public class SubstituteResolver : ValueResolver<Hop, IList<string>>
        {
            protected override IList<string> ResolveCore(Hop hop)
            {
                List<string> sub = new List<string>();
                if (hop.Substituts != null)
                {
                    foreach (var item in hop.Substituts)
                    {
                        sub.Add(item.Name);
                    }
                }
                return sub;
            }

        }
}