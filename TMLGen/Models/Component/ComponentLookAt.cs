using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentLookAt : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Look At Event";
    }
}
