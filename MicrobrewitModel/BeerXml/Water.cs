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
    [XmlRoot("WATER")]
    public class Water
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        [DataMember]
        [XmlElement("CALCIUM")]
        public string Calcium { get; set; }
        [DataMember]
        [XmlElement("BICRBONATE")]
        public string Bicarbonate { get; set; }
        [DataMember]
        [XmlElement("SULFATE")]
        public string Sulfate { get; set; }
        [DataMember]
        [XmlElement("CHLORIDE")]
        public string Chloride { get; set; }
        [DataMember]
        [XmlElement("SODIUM")]
        public string Sodium { get; set; }
        [DataMember]
        [XmlElement("MAGNESIUM")]
        public string Magnesium { get; set; }
        [DataMember]
        [XmlElement("PH")]
        public string Ph { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
    }
}
