using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class YeastCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public LinksYeast Links { get; set; }
        [JsonProperty(PropertyName = "yeasts")]
        public IList<YeastDto> Yeasts { get; set; }

        public YeastCompleteDto()
        {
            Links = new LinksYeast()
            {
                YeastsSupplier = new Links()
                {
                    Href = apiPath + "/suplliers/:id",
                    Type = "supplier",
                }

            };
        }
    }
}