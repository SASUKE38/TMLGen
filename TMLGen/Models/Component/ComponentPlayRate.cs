using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentPlayRate : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Play Rate";
        [XmlIgnore]
        public static readonly float defaultPlayRate = 1.0f;
    }
}
