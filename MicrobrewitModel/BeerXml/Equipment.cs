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
    [XmlRoot("EQUIPMENT")]
    public class Equipment
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("BOIL_SIZE")]
        public string Boil_Size { get; set; }
        [DataMember]
        [XmlElement("BATCH_SIZE")]
        public string Batch_Size { get; set; }
        [DataMember]
        [XmlElement("TUN_VOLUME")]
        public string Tun_Volume { get; set; }
        [DataMember]
        [XmlElement("TUN_WEIGHT")]
        public string Tun_Weight { get; set; }
        [DataMember]
        [XmlElement("TUN_SPECIFIC_HEAT")]
        public string Tun_Specific_Heat { get; set; }
        [DataMember]
        [XmlElement("TOP_UP_WATER")]
        public string Top_Up_Water { get; set; }
        [DataMember]
        [XmlElement("TRUB_CHILLER_LOSS")]
        public string Trub_Chiller_Loss { get; set; }
        [DataMember]
        [XmlElement("EVAP_RATE")]
        public string Evap_Rate { get; set; }
        [DataMember]
        [XmlElement("BOIL_TIME")]
        public string Boil_Time { get; set; }
        [DataMember]
        [XmlElement("CALC_BOIL_VOLUME")]
        public string Calc_Boil_Volume { get; set; }
        [DataMember]
        [XmlElement("LAUTER_DEADSPACE")]
        public string Lauter_Deadspace { get; set; }
        [DataMember]
        [XmlElement("TOP_UP_KETTLE")]
        public string Top_Up_Kettle { get; set; }
        [DataMember]
        [XmlElement("HOP_UTILIZATION")]
        public string Hop_Utilization { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_BOIL_SIZE")]
        public string Display_Boil_Size { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_BATCH_SIZE")]
        public string Display_Batch_Size { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TUN_VOLUME")]
        public string Display_Tun_Volume { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TOP_UP_WEIGHT")]
        public string Display_Tun_Weight { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TOP_UP_WATER")]
        public string Display_TopUp_Water { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TRUB_CHILLER_LOSS")]
        public string Display_Trub_Chiller_Loss { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_LAUTER_DEADSPACE")]
        public string Display_Lauter_Deadspace { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_TOP_UP_KETTLE")]
        public string Display_Top_Up_Kettle { get; set; }
    }
}
