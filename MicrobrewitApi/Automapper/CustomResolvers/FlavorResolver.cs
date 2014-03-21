using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class FlavorResolver : ValueResolver<Hop,IList<string>>
    {
        protected override IList<string> ResolveCore(Hop hop)
        {
            var flavors = new List<string>();
            if (hop.HopFlavours != null)
            {
                foreach (var flavor in hop.HopFlavours)
                {
                    flavors.Add(flavor.Flavour.Name);
                }
            }
                return flavors;
        }
       
    }
}