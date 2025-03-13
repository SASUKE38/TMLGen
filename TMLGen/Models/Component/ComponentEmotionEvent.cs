using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentEmotionEvent : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Emotion Event";
    }
}
