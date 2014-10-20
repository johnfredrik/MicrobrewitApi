using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;

namespace Microbrewit.Model.DTOs
{
    public class BeerStyleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DTO SuperBeerStyle { get; set; }
        public IList<DTO> SubBeerStyles { get; set; }
        public string DataType { get { return "beerstyle"; } }
       
    }
}