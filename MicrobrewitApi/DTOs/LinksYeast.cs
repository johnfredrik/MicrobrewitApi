using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class LinksYeast
    {
        [JsonProperty(PropertyName = "yeasts.supplier")]
        public Links YeastsSupplier { get; set; }
    }
}