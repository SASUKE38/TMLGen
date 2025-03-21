using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class TimelinePositionManager
    {
        [XmlAttribute]
        public Guid BoundSceneId;
        [XmlElement("AdditionalBoundSceneId")]
        public List<AdditionalBoundSceneId> AdditionalBoundSceneIds;
        //public List<Guid> RequiredCinematicLevelTemplateIds;

        public TimelinePositionManager()
        {
            this.AdditionalBoundSceneIds = new List<AdditionalBoundSceneId>();
        }

    }
}
