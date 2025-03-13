using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentShowPeanuts : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Show Peanuts";
    }
}
