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
    [XmlRoot("MASH")]
    public class Mash
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("GRAIN_TEMP")]
        public string GrainTemp { get; set; }
        [DataMember]
        [XmlElement("TUN_TEMP")]
        public string TunTemp { get; set; }
        [DataMember]
        [XmlElement("SPARGE_TEMP")]
        public string SpargeTemp { get; set; }
        [DataMember]
        [XmlElement("PH")]
        public string Ph { get; set; }
        [DataMember]
        [XmlElement("TUN_WEIGHT")]
        public string TunWeight { get; set; }
        [DataMember]
        [XmlElement("TUN_SPECIFIC_HEAT")]
        public string TunSpecificHeat { get; set; }
        [DataMember]
        [XmlElement("EQUIP_ADJUST")]
        public string EquipAdjust { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_GRAIN_TEMP")]
        public string DisplayGrainTemp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TUN_TEMP")]
        public string DisplayTunTemp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_SPARGE_TEMP")]
        public string DisplaySpargeTemp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TUN_WEIGHT")]
        public string DisplayTunWeight { get; set; }
        [DataMember]
        [XmlArray("MASH_STEPS")]
        [XmlArrayItem("MASH_STEP")]
        public List<MashStep> MashSteps { get; set; }
    }
}
