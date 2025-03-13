using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentAttitudeEvent : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Attitude Event";
    }
}
