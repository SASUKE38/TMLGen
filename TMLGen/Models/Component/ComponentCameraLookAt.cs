using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentCameraLookAt : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Look At";
    }
}
