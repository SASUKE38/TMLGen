using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentCameraFoV : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Field of View";
        [XmlIgnore]
        public static readonly float defaultFov = 27f;
    }
}
