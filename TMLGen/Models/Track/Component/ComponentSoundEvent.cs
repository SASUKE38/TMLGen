using System.Xml.Serialization;
using TMLGen.Models.Component;

namespace TMLGen.Models.Track.Component
{
    public class ComponentSoundEvent : ComponentBase
    {
        [XmlIgnore]
        public static readonly string channelName = "Sound Event";
    }
}
