using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentShapeshift : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Shapeshift";
    }
}
