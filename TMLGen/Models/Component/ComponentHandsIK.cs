using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentHandsIK : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Hands IK";
    }
}
