using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class LinksHop
    {
          
        [JsonProperty(PropertyName = "hops.origins")]
        public Links HopOrigins { get; set; }
        [JsonProperty(PropertyName = "hops.substitutions")]
        public Links HopSubstitutions { get; set; }
    }
}