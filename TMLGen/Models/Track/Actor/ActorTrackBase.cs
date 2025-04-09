using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Actor
{
    public class ActorTrackBase : FolderTrack
    {
        [XmlAttribute]
        public string ActorType;
        [XmlAttribute]
        public Guid ActorId;
        [XmlAttribute]
        public Guid AssignedActorId; // Not in lsf?
        [XmlAttribute]
        public bool IsTemplate;
        [XmlAttribute]
        public bool IsResource;
        [XmlIgnore]
        public int LightChannel; // Not in lsf?
        [XmlAttribute]
        public bool AlwaysInclude;
        [XmlAttribute]
        public Guid VoiceOverrideId; // Not in lsf?
        [XmlAttribute]
        public bool IsFromLevelTemplate;
        [XmlAttribute]
        public Guid ParentTemplateId;

        public CustomActorSoundData CustomActorData;
        public WorldActorEndState ActorEndState;
        public ActorIdleOverrideData ActorIdleOverrideData;
        public BehaviourActorStartCondition BehaviourActorStartCondition;

        [XmlIgnore]
        public Dictionary<int, List<TrackBase>> actorChildTracks;
        [XmlIgnore]
        public Dictionary<uint, TrackBase> actorChildSpringTracks;

        public ActorTrackBase()
        {
            CustomActorData = new CustomActorSoundData();
            ActorEndState = new WorldActorEndState();
            ActorIdleOverrideData = new ActorIdleOverrideData();
            BehaviourActorStartCondition = new BehaviourActorStartCondition();

            actorChildTracks = [];
            actorChildSpringTracks = [];

            ActorId = Guid.Empty;
            ActorType = string.Empty;
            AssignedActorId = Guid.Empty;
        }
    }
}
