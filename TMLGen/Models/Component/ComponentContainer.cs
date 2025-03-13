using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentContainer
    {
        [XmlAttribute]
        public bool IncludeInTimeline;
        [XmlAttribute]
        public Guid ComponentTrackReference;
        [XmlAttribute]
        public Guid TrackId;

        [XmlElement]
        public List<ComponentBase> Component;

        public ComponentContainer()
        {
            Component = new List<ComponentBase>();
            IncludeInTimeline = true;
        }
    }
}
