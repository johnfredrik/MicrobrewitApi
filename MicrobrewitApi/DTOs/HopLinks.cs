using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopLinks
    {
        public int? OriginId { get; set; }
        public IList<int> FlavorIds { get; set; }
        public IList<int> SubstituesIds { get; set; }
    }
}