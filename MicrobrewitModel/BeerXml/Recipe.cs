using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Microbrewit.Model.BeerXml
{
    [DataContract(Namespace = "")]
    [XmlRoot("RECIPE")]
    public class Recipe
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement(ElementName = "VERSION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("TYPE")]
        public string Type { get; set; }
        [DataMember]
        [XmlElement("BREWER")]
        public string Brewer { get; set; }
        [DataMember]
        [XmlElement("ASST_BREWER")]
        public string Asst_Brewer { get; set; }
        [DataMember]
        [XmlElement("BATCH_SIZE")]
        public string BatchSize { get; set; }
        [DataMember]
        [XmlElement("BOIL_SIZE")]
        public string BoilSize { get; set; }
        [DataMember]
        [XmlElement("BOIL_TIME")]
        public string Boil_Time { get; set; }
        [DataMember]
        [XmlElement("EFFICIENCY")]
        public string Efficiency { get; set; }
        [DataMember]
        //[XmlElement("HOPS")]
        [XmlArray("HOPS")]
        [XmlArrayItem("HOP")]
        public List<Hop> Hops { get; set; }
        [DataMember]
        [XmlArray("FERMENTABLES")]
        [XmlArrayItem("FERMENTABLE")]
        public List<Fermentable> Fermentables { get; set; }
        [DataMember]
        [XmlArray("MISCS")]
        [XmlArrayItem("MISC")]
        public List<Misc> Miscs { get; set; }
        [DataMember]
        [XmlArray("YEASTS")]
        [XmlArrayItem("YEAST")]
        public List<Yeast> Yeasts { get; set; }
        [DataMember]
        [XmlArray("WATERS")]
        [XmlArrayItem("WATER")]
        public List<Water> Waters { get; set; }
        [DataMember]
        [XmlElement("STYLE")]
        public Style Style { get; set; }
        [DataMember]
        [XmlElement("EQUIPMENT")]
        public Equipment Equipment { get; set; }
        [DataMember]
        [XmlElement("MASH")]
        public Mash Mash { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("TASTE_NOTES")]
        public string Taste_Notes { get; set; }
        [DataMember]
        [XmlElement("TASTE_RATING")]
        public string Taste_Rating { get; set; }
        [DataMember]
        [XmlElement("OG")]
        public string Og { get; set; }
        [DataMember]
        [XmlElement("FG")]
        public string Fg { get; set; }
        [DataMember]
        [XmlElement("CARBONATION")]
        public string Carbonation { get; set; }
        [DataMember]
        [XmlElement("FERMENTATION_STAGES")]
        public string Fermentation_Stages { get; set; }
        [DataMember]
        [XmlElement("PRIMARY_AGE")]
        public string PrimaryAge { get; set; }
        [DataMember]
        [XmlElement("PRIMARY_TEMP")]
        public string Primary_Temp { get; set; }
        [DataMember]
        [XmlElement("SECONDARY_AGE")]
        public string Secondary_Age { get; set; }
        [DataMember]
        [XmlElement("SECONDARY_TEMP")]
        public string Secondary_Temp { get; set; }
        [DataMember]
        [XmlElement("TERTIARY_AGE")]
        public string TertiaryAge { get; set; }
        [DataMember]
        [XmlElement("TERTIARY_TEMP")]
        public string TertiaryTemp { get; set; }
        [DataMember]
        [XmlElement("AGE")]
        public string Age { get; set; }
        [DataMember]
        [XmlElement("AGE_TEMP")]
        public string Age_Temp { get; set; }
        [DataMember]
        [XmlElement("CARBONATION_USED")]
        public string Carbonation_Used { get; set; }
        [DataMember]
        [XmlElement("DATE")]
        public string Date { get; set; }
        [DataMember]
        [XmlElement("EST_OG")]
        public string Est_Og { get; set; }
        [DataMember]
        [XmlElement("EST_FG")]
        public string Est_Fg { get; set; }
        [DataMember]
        [XmlElement("EST_COLOR")]
        public string Est_Color { get; set; }
        [DataMember]
        [XmlElement("IBU")]
        public string Ibu { get; set; }
        [DataMember]
        [XmlElement("IBU_METHOD")]
        public string Ibu_Method { get; set; }
        [DataMember]
        [XmlElement("EST_ABV")]
        public string Est_Abv { get; set; }
        [DataMember]
        [XmlElement("ABV")]
        public string Abv { get; set; }
        [DataMember]
        [XmlElement("ACTUAL_EFFICIENCY")]
        public string Actual_Efficiency { get; set; }
        [DataMember]
        [XmlElement("CALORIES")]
        public string Calories { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_BATCH_SIZE")]
        public string Display_Batch_Size { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_BOIL_SIZE")]
        public string Display_Boil_Size { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_OG")]
        public string Display_Og { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_FG")]
        public string Display_Fg { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_PRIMARY_TEMP")]
        public string Display_Primary_Temp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_SECONDARY_TEMP")]
        public string Display_Secondary_Temp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TERTIARY_TEMP")]
        public string Display_Tertiary_Temp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_AGE_TEMP")]
        public string Display_Age_Temp { get; set; }
    }
}
