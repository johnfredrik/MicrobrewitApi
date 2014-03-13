using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel
{

    public class RecipeHop
    {
        public int Id { get; set; }
        public int HopId { get; set; }
        public int RecipeId { get; set; }
        public int AAValue { get; set; }


        public Recipe Recipe { get; set; }
        public Hop Hop { get; set; }
    }
}
