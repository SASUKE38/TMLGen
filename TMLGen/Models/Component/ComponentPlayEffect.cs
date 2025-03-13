using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentPlayEffect : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Play Effect";
    }
}
