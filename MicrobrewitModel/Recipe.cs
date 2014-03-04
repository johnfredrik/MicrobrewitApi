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
       
        public int RecipeId { get; set; }
        public string Name { get; set; }      
    }
}