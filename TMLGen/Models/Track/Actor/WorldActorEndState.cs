using System;
using System.Xml.Serialization;
using TMLGen.Models.Core;

namespace TMLGen.Models.Track.Actor
{
    [XmlType(TypeName = "ActorEndState")]
    public class WorldActorEndState
    {
        [XmlAttribute]
        public Guid NewTemplateId;
        [XmlAttribute]
        public bool Hide;
        [XmlAttribute]
        public bool Show;
        [XmlAttribute]
        public bool UseTransformation;
        [XmlAttribute]
        public string ScenePosition;
        [XmlAttribute]
        public string SceneRotation;

        public WorldActorEndState()
        {
            NewTemplateId = Guid.Empty;
            Hide = false;
            Show = false;
            UseTransformation = false;
            Vector3 vec = new();
            ScenePosition = vec.ToString();
            SceneRotation = vec.ToString();
        }
    }
}
