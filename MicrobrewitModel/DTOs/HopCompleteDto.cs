using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class HopCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
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
                    Href = apiPath + "flavors/:id",
                    Type = "flavor"

                },
                HopOrigins = new Links()
                {
                    Href = apiPath + "/origins/:id",
                    Type = "origin"
                },
                HopSubstitutions = new Links()
                {
                    Href = apiPath + "/hop/:id",
                    Type = "hop"
                }

            };
        }
    }
}