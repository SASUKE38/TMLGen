using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentSprings : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelNameBase = "Springs";
    }
}
