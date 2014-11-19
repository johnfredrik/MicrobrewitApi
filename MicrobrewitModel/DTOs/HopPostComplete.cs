using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class HopPostComplete
    {
        [JsonProperty(PropertyName = "hops")]
        public IList<HopDto> Hops { get; set; }
    }
}
