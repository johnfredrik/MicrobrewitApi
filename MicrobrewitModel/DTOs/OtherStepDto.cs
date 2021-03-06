﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "otherStep")]
    public class OtherStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "otherId")]
        public int OtherId { get; set; }
        [JsonProperty(PropertyName = "stepNumber")]
        public int Number { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}