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
        public double OGLow { get; set; }
        public double OGHigh { get; set; }
        public double FGLow { get; set; }
        public double FGHigh { get; set; }
        public double IBULow { get; set; }
        public double IBUHigh { get; set; }
        public double SRMLow { get; set; }
        public double SRMHigh { get; set; }
        public double ABVLow { get; set; }
        public double ABVHigh { get; set; }
        public string Comments { get; set; }
        public IList<DTO> SubBeerStyles { get; set; }
        public string DataType { get { return "beerstyle"; } }
       
    }
}