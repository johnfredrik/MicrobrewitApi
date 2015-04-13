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
    [XmlRoot("STYLE")]
    public class Style
    {
        [DataMember]
        [XmlElement("NAME")]
        public string Name { get; set; }
        [DataMember]
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [DataMember]
        [XmlElement("CATEGORY")]
        public string Category { get; set; }
        [DataMember]
        [XmlElement("CATEGORY_NUMBER")]
        public string CategoryNumber { get; set; }
        [DataMember]
        [XmlElement("STYLE_LETTER")]
        public string StyleLetter { get; set; }
        [DataMember]
        [XmlElement("STYLE_GUIDE")]
        public string StyleGuide { get; set; }
        [DataMember]
        [XmlElement("TYPE")]
        public string Type { get; set; }
        [DataMember]
        [XmlElement("OG_MIN")]
        public string OgMin { get; set; }
        [DataMember]
        [XmlElement("OG_MAX")]
        public string OgMax { get; set; }
        [DataMember]
        [XmlElement("FG_MIN")]
        public string FgMin { get; set; }
        [DataMember]
        [XmlElement("FG_MAX")]
        public string FgMax { get; set; }
        [DataMember]
        [XmlElement("IBU_MIN")]
        public string IbuMin { get; set; }
        [DataMember]
        [XmlElement("IBU_MAX")]
        public string IbuMax { get; set; }
        [DataMember]
        [XmlElement("COLOR_MIN")]
        public string ColorMin { get; set; }
        [DataMember]
        [XmlElement("COLOR_MAX")]
        public string ColorMax { get; set; }
        [DataMember]
        [XmlElement("CARB_MIN")]
        public string CarbMin { get; set; }
        [DataMember]
        [XmlElement("CRB_MAX")]
        public string CarbMax { get; set; }
        [DataMember]
        [XmlElement("ABV_MIN")]
        public string AbvMin { get; set; }
        [DataMember]
        [XmlElement("ABV_MAX")]
        public string AbvMax { get; set; }
        [DataMember]
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        [DataMember]
        [XmlElement("PROFILE")]
        public string Profile { get; set; }
        [DataMember]
        [XmlElement("INGREDIENTS")]
        public string Ingredients { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_OG_MIN")]
        public string DisplayOgMin { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_OG_MAX")]
        public string DisplayOgMax { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_FG_MIN")]
        public string DisplayFgMin { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_FG_MAX")]
        public string DisplayFgMax { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_COLOR_MIN")]
        public string DisplayColorMin { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_COLOR_MAX")]
        public string DisplayColorMax { get; set; }
        [DataMember]
        [XmlElement("OG_RANGE")]
        public string OgRange { get; set; }
        [DataMember]
        [XmlElement("FG_RANGE")]
        public string FgRange { get; set; }
        [DataMember]
        [XmlElement("IBU_RANGE")]
        public string IbuRange { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_CARB_RANGE")]
        public string CarbRange { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_COLOR_RANGE")]
        public string ColorRange { get; set; }
        [DataMember]
        [XmlElement("DISPLAY_ABV_RANGE")]
        public string AbvRange { get; set; }
    }
}
