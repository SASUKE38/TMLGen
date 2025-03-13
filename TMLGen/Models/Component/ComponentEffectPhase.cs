using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentEffectPhase : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Phase";
        [XmlIgnore]
        public static readonly int phaseFallback = 0;
    }
}
