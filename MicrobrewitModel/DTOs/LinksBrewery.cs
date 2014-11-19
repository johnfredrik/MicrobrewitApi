using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class LinksBrewery
    {
        [JsonProperty(PropertyName = "beer")]
        public Links Beer { get; set; }
        [JsonProperty(PropertyName = "user")]
        public Links User { get; set; }
    }
}
