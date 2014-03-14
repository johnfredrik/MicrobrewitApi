using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class RecipeHopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AAValue { get; set; }
        public string Origin { get; set; }
    }
}