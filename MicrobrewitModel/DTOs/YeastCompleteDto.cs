using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class YeastCompleteDto
    {
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
                    Href = "http://api.microbrew.it/suplliers/:id",
                    Type = "supplier",
                }

            };
        }
    }
}