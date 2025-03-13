using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentSplatter : ComponentBase
    {
        [XmlIgnore]
        public static readonly string bloodPath = "Splatter.Blood";
        [XmlIgnore]
        public static readonly string dirtPath = "Splatter.Dirt";
        [XmlIgnore]
        public static readonly string bruisePath = "Splatter.Bruise";
        [XmlIgnore]
        public static readonly string sweatPath = "Splatter.Sweat";
    }
}
