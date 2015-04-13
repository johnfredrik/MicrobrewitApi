using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Microbrewit.Model.BeerXml
{
    [DataContract(Namespace = "")]
    [XmlRoot("MASH_STEP")]
    public class MashStep
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERISON")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("TYPE")]
        public string Type { get; set; }
        [DataMember]
        [XmlElement("INFUSE_AMOUNT")]
        public string InfuseAmount { get; set; }
        [DataMember]
        [XmlElement("STEP_TIME")]
        public string StepTime { get; set; }
        [DataMember]
        [XmlElement("STEP_TEMP")]
        public string StepTemp { get; set; }
        [DataMember]
        [XmlElement("RAMP_TIME")]
        public string RampTime { get; set; }
        [DataMember]
        [XmlElement("END_TEMP")]
        public string EndTemp { get; set; }
        [DataMember]
        [XmlElement("DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [XmlElement("WATER_GRAIN_RATIO")]
        public string WaterGrainRatio { get; set; }
        [DataMember]
        [XmlElement("DECOCTION_AMT")]
        public string DecoctionAmt { get; set; }
        [DataMember]
        [XmlElement("INFUSE_TEMP")]
        public string InfuseTemp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_STEP_TEMP")]
        public string DisplayStepTemp { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_INFUSE_AMT")]
        public string DisplayInfuseAmt { get; set; }
    }
}
