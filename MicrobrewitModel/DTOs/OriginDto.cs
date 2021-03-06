﻿using Nest;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "origin")]
    public class OriginDto
    {
        [JsonProperty(PropertyName = "originId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "origin"; } }
    }
}
