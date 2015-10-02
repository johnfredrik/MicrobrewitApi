using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class AcidDto
    {
        [JsonProperty(PropertyName = "alpha")]
        public AlphaAcidDto AlphaAcid { get; set; }
        [JsonProperty(PropertyName = "beta")]
        public BetaAcidDto BetaAcid { get; set; }
    }
}
