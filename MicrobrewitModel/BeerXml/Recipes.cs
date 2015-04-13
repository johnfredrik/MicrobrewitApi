using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Microbrewit.Model.BeerXml
{
    [DataContract(Namespace = "")]
    [XmlRoot("RECIPES")]
    public class RecipesComplete
    {
       [XmlElement("RECIPE")]
       public List<Recipe> Recipes { get; set; }
    }
}
