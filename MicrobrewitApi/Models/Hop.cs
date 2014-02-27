using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models
{
    public class Hop
    {     
        public int HopId { get; set; }   
        [Required]
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public int OriginId { get; set; }
        [ForeignKey("OriginId")]
        public Origin Origin { get; set; }


        public virtual List<Recipe> Recipes { get; set; }
    }
}