using System.Xml.Serialization;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackLookAtEvent : ComponentTrackBase
    {
        [XmlAttribute]
        public bool IsAutomatedLookatEnabled;

        public ComponentTrackLookAtEvent()
        {
            Name = "Look At Event";
            Type = "ComponentTrackLookAtEvent";
            IsAutomatedLookatEnabled = false;
        }
    }
}
