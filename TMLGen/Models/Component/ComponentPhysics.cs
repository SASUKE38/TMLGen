using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentPhysics : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Foot IK";
    }
}
