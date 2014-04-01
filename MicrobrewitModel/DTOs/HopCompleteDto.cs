using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class HopCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksHop Links { get; set; }
        [JsonProperty(PropertyName = "hops")]
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
                    Type = "hop"
                }

            };
        }
    }
}