using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackSprings : ComponentTrackBase
    {
        [XmlAttribute]
        public string VisualTargets;

        public ComponentTrackSprings()
        {
            VisualTargets = Enum.GetName(typeof(SpringsVisualFlag), SpringsVisualFlag.AllVisuals);
            Name = "Springs " + VisualTargets;
            Type = "ComponentTrackSprings";
        }
    }
}
