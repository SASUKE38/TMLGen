using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TMLGen.Models.Track.Component;

namespace TMLGen.Models.Track.Actor
{
    [XmlType(Namespace = "")]
    public class ActorTrackSpeaker : ActorTrackBase
    {
        [XmlAttribute]
        public bool IsPeanut;
        [XmlAttribute]
        public int SpeakerId;
        [XmlAttribute]
        public bool IsPlayer;
        [XmlAttribute]
        public bool IsImportantForStaging;
        [XmlIgnore]
        public int PeanutPriority; // not in lsf?
        [XmlAttribute]
        public string SceneActorType;
        [XmlAttribute]
        public int SceneActorIndex;
        [XmlAttribute]
        public Guid SpeakerMappingId;

        public ActorTrackSpeaker()
        {
            Type = "ActorTrackSpeaker";
            IsPeanut = false;
            SpeakerId = -1;
            IsPlayer = false;
            SceneActorType = "None";
            IsImportantForStaging = true;
            SpeakerMappingId = Guid.Empty;
        }
    }
}
