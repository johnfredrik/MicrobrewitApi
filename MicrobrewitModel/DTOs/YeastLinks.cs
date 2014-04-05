using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class YeastLinks
    {
        [JsonProperty(PropertyName = "supplierid")]
        public int SupplierId { get; set; }
    }
}