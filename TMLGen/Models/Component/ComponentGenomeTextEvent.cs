using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentGenomeTextEvent : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Genome Text Event";
    }
}
