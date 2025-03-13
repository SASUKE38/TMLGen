using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentShowActor : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Show Actor";
    }
}
