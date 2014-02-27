using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public virtual List<Fermentable> Fermentables { get; set; }
        public virtual List<Hop> Hops { get; set; }

    }
}