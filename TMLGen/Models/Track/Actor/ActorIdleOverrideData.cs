using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Actor
{
    public class ActorIdleOverrideData
    {
        [XmlAttribute]
        public Guid ActorIdleOverride;

        public ActorIdleOverrideData()
        {
            ActorIdleOverride = Guid.Empty;
        }
    }
}
