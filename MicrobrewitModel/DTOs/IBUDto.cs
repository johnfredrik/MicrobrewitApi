﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "ibu")]
    public class IBUDto
    {
        [JsonProperty(PropertyName = "ibuId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "standard")]
        public int Standard { get; set; }
        [JsonProperty(PropertyName = "tinseth")]
        public double Tinseth { get; set; }
        [JsonProperty(PropertyName = "rager")]
        public double Rager { get; set; }
    }
}