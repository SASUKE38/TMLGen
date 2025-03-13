using System.Xml.Serialization;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackEmotionEvent : ComponentTrackBase
    {
        [XmlAttribute]
        public bool UsingMlTrack; // figure out ml stuff

        public ComponentTrackEmotionEvent()
        {
            Name = "Emotion Event";
            Type = "ComponentTrackEmotionEvent";
            UsingMlTrack = false;
        }
    }
}
