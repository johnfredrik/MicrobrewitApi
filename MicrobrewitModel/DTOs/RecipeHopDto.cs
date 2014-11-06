﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class RecipeHopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "aaValue")]
        public int AAValue { get; set; }
        public string Origin { get; set; }
    }
}