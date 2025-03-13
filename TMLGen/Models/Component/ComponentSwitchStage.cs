using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentSwitchStage : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Switch Stage";
    }
}
