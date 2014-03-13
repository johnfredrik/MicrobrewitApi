using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitModel
{
    public class Recipe
    {
       
        public int Id { get; set; }
        public string Name { get; set; }

        //public ICollection<Fermentable> Fermentables { get; set; }
        public virtual ICollection<RecipeHop> RecipeHops { get; set; }
        //public ICollection<Other> Others { get; set; }
        //public ICollection<Yeast> Yeasts { get; set; }




    }
}