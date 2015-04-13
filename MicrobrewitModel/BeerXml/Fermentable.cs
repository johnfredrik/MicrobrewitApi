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
    [XmlRoot("FERMENTABLE")]
    public class Fermentable
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("TYPE")]
        public string Type { get; set; }
        [DataMember]
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        [DataMember]
        [XmlElement("YIELD")]
        public string Yield { get; set; }
        [DataMember]
        [XmlElement("COLOR")]
        public string Color { get; set; }
        [DataMember]
        [XmlElement("ADD_AFTER_BOIL")]
        public string AddAfterBoil { get; set; }
        [DataMember]
        [XmlElement("ORIGIN")]
        public string Origin { get; set; }
        [DataMember]
        [XmlElement("SUPPLIER")]
        public string Supplier { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        // These should be double
        [DataMember]
        [XmlElement("COARSE_FINE_DIFF")]
        public string Coarse_Fine_Diff { get; set; }
        [DataMember]
        [XmlElement("MOISTURE")]
        public string Moisture { get; set; }
        [DataMember]
        [XmlElement("DIASTATIC_POWER")]
        public string DiastaticPower { get; set; }
        [DataMember]
        [XmlElement("PROTEIN")]
        public string Protein { get; set; }
        // to here
        [DataMember]
        [XmlElement("MAX_IN_BATCH")]
        public string Max_In_Batch { get; set; }
        [DataMember]
        [XmlElement("RECOMMEND_MASH")]
        public string RecommendMash { get; set; }
        [DataMember]
        [XmlElement("IBU_GAL_PER_LB")]
        public string Ibu_Gal_Per_Lb { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
        [DataMember]
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        [DataMember]
        [XmlElement("POTENTIAL")]
        public string Potential { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_COLOR")]
        public string Display_Color { get; set; }
    }
}
