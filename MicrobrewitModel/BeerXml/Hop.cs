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
    [XmlRoot("HOP")]
    public class Hop
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("ORIGIN")]
        public string Origin { get; set; }
        [DataMember]
        [XmlElement("ALPHA")]
        public string Alpha { get; set; }
        [DataMember]
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        [DataMember]
        [XmlElement("USE")]
        public string Use { get; set; }
        [DataMember]
        [XmlElement("TIME")]
        public string Time { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("TYPE")]
        public string Type { get; set; }
        [DataMember]
        [XmlElement("FORM")]
        public string Form { get; set; }
        [DataMember]
        [XmlElement("BETA")]
        public string Beta { get; set; }
        [DataMember]
        [XmlElement("HSI")]
        public string HSI { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
        [DataMember]
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TIME")]
        public string Display_Time { get; set; }
    }
}
