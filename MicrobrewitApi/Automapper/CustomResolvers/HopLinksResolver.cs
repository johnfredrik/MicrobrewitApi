using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Api.DTOs;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class HopLinksResolver : ValueResolver<Hop,HopLinks>
    {
        protected override HopLinks ResolveCore(Hop hop)
            {
 	            var hopLinks = new HopLinks(){ FlavorIds = new List<int>(), SubstituesIds = new List<int>()};
                if(hop.OriginId != null)
                {
                    hopLinks.OriginId = hop.OriginId; 
                }
                if (hop.HopFlavours != null)
                {
                    foreach (var flavor in hop.HopFlavours)
                    {
                        hopLinks.FlavorIds.Add(flavor.FlavourId);
                    }
                }
                if (hop.Substituts != null)
                {
                    foreach (var sub in hop.Substituts)
                    {
                        hopLinks.SubstituesIds.Add(sub.Id);
                    }
                }
                return hopLinks;

            }
    }
}