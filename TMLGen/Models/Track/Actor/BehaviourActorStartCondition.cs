using System;
using System.Xml.Serialization;
using TMLGen.Models.Core;

namespace TMLGen.Models.Track.Actor
{
    public class BehaviourActorStartCondition
    {
        [XmlAttribute]
        public string BehaviourInteractionTargetType;
        [XmlAttribute]
        public Guid ItemInteractId;
        [XmlAttribute]
        public Guid SpeakerInteractId;
        [XmlIgnore]
        public bool StageWithWorldPosition;
        [XmlAttribute]
        public bool UseTransformation;
        [XmlAttribute]
        public string Position;
        [XmlAttribute]
        public string Rotation;

        public BehaviourActorStartCondition()
        {
            BehaviourInteractionTargetType = Enum.GetName(typeof(BehaviourInteractionTargetTypeEnum), BehaviourInteractionTargetTypeEnum.None);
            ItemInteractId = Guid.Empty;
            SpeakerInteractId = Guid.Empty;
            StageWithWorldPosition = true;
            UseTransformation = true;
            Vector3 vec = new();
            Position = vec.ToString();
            Rotation = vec.ToString();
        }
    }
}
