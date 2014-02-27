using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models
{
    public class Fermentable
    {
        public int FermentableId { get; set; }
        public Uri href { get; set; }
        public string Name { get; set; }
        public int Colour { get; set; }
        public int PPG { get; set; }
        public string Type { get; set; }

        public List<Recipe> Recipes { get; set; }
    }
}