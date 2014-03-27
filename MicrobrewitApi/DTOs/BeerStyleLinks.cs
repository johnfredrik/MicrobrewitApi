﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class BeerStyleLinks
    {
        [JsonProperty(PropertyName = "subbeerstyleids")]
        public IList<int> SubBeerStyleIds { get; set; }
    }
}