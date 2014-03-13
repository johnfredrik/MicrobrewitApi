using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitModel
{
    public class Hop
    {
    
        public int Id { get; set; }        
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public Nullable<int> OriginId { get; set; } 
        public virtual Origin Origin { get; set; }

        public virtual ICollection<RecipeHop> RecipeHops { get; set; }
    }
}