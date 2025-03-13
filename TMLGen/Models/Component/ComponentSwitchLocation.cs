using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentSwitchLocation : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Switch Location";
    }
}
