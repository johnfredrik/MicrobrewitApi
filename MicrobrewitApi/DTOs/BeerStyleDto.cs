using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;

namespace Microbrewit.Api.DTOs
{
    public class BeerStyleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SuperBeerStyle { get; set; }
        public IList<string> SubBeerStyles { get; set; }
        public BeerStyleLinks Links { get; set; }
    }
}