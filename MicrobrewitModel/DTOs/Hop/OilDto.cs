using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class OilDto
    {
        [JsonProperty(PropertyName = "total oil")]
        public TotalOilDto TotalOilDto { get; set; }
        [JsonProperty(PropertyName = "b-pinene")]
        public BPineneDto BPineneDto { get; set; }
        [JsonProperty(PropertyName = "linaloll")]
        public LinalLoolDto LinalLoolDto { get; set; }
        [JsonProperty(PropertyName = "myrcene")]
        public MyrceneDto MyrceneDto { get; set; }
        [JsonProperty(PropertyName = "caryophyllene")]
        public CaryophylleneDto CaryophylleneDto { get; set; }
        [JsonProperty(PropertyName = "farnesene")]
        public FarneseneDto FarneseneDto { get; set; }
        [JsonProperty(PropertyName = "humulene")]
        public HumuleneDto HumuleneDto { get; set; }
        [JsonProperty(PropertyName = "geraniol")]
        public GeraniolDto GeraniolDto { get; set; }
        [JsonProperty(PropertyName = "other")]
        public OtherOilDto OtherOilDto { get; set; }


    }
}
