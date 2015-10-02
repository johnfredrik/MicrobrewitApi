using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class HighLow
    {
        [JsonProperty(PropertyName = "low")]
        public double Low { get; set; }
        [JsonProperty(PropertyName = "high")]
        public double High { get; set; }

    }
}
