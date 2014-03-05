using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitModel
{
    public class Fermentable
    {
        
        public int FermentableId { get; set; }
        public string href { get; set; }
        public string Name { get; set; }
        public int Colour { get; set; }
        public int PPG { get; set; }
    }
}