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
    [XmlRoot("YEAST")]
    public class Yeast
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
        [XmlElement("FORM")]
        public string Form { get; set; }
        [DataMember]
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        [DataMember]
        [XmlElement("AMOUNT_IS_WEIGHT")]
        public string Amount_Is_Weight { get; set; }
        [DataMember]
        [XmlElement("LABORATORY")]
        public string Laboratory { get; set; }
        [DataMember]
        [XmlElement("PRODUCT_ID")]
        public string Product_Id { get; set; }
        [DataMember]
        [XmlElement("MIN_TEMPERATURE")]
        public string Min_Temperature { get; set; }
        [DataMember]
        [XmlElement("MAX_TEMPERATURE")]
        public string Max_Temperature { get; set; }
        [DataMember]
        [XmlElement("FLOCCULATION")]
        public string Flocculation { get; set; }
        [DataMember]
        [XmlElement("ATTENUATION")]
        public string Attenuation { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("BEST_FOR")]
        public string Best_For { get; set; }
        [DataMember]
        [XmlElement("MAZ_REUSE")]
        public string Max_Reuse { get; set; }
        [DataMember]
        [XmlElement("TIMES_CULTURED")]
        public string Times_Cultured { get; set; }
        [DataMember]
        [XmlElement("ADD_TO_SECONDARY")]
        public string Add_To_Secondary { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_MIN_TEMP")]
        public string Display_Min_Temp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_MAX_TEMP")]
        public string Display_Max_Temp { get; set; }
        [DataMember]
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        [DataMember]
        [XmlElement("CULTURE_DATE")]
        public string Culture_Date { get; set; }
    }
}
