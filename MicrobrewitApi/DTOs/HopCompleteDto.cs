using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopCompleteDto
    {
        public LinksHop Links { get; set; }
        public IList<HopDto> Hops { get; set; }

        public HopCompleteDto()
        {
            Links = new LinksHop()
            {
                HopFlavors = new Links()
                {
                    Href = "http://api.microbrew.it/flavors/:id",
                    Type = "flavor"

                },
                HopOrigins = new Links()
                {
                    Href = "http://api.microbrew.it/origins/:id",
                    Type = "origin"
                },
                HopSubstitutions = new Links()
                {
                    Href = "http://api.microbrew.it/hop/:id",
                    Type = "origin"
                }

            };
        }
    }
}