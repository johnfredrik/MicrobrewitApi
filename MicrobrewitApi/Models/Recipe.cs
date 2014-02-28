using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models
{
    public class Recipe
    {
       
        public int RecipeId { get; set; }
        public string Name { get; set; }
        [ForeignKey("FermentableId")]
        public virtual List<Fermentable> Fermentables { get; set; }
        [ForeignKey("HopId")]
        public virtual List<Hop> Hops { get; set; }

    }
}