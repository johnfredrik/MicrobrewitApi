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
    [XmlRoot("MISC")]
    public class Misc
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
        [XmlElement("USE")]
        public string Use { get; set; }
        [DataMember]
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        [DataMember]
        [XmlElement("TIME")]
        public string Time { get; set; }
        [DataMember]
        [XmlElement("AMOUNT_IS_WEIGHT")]
        public string AmountIsWeight { get; set; }
        [DataMember]
        [XmlElement("USE_FOR")]
        public string UseFor { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_AMOUNT")]
        public string DisplayAmount { get; set; }
        [DataMember]
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TIME")]
        public string DisplayTime { get; set; }
        [DataMember]
        [XmlElement("BATCH_SIZE")]
        public string BatchSize { get; set; }
    }
}
