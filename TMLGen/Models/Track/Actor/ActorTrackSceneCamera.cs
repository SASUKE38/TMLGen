using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Actor
{
    public class ActorTrackSceneCamera : ActorTrackBase
    {

        [XmlAttribute]
        public Guid SceneCameraAttachTo;
        [XmlAttribute]
        public Guid SceneCameraId;
        [XmlAttribute]
        public Guid SceneCameraLookAt;

        public ActorTrackSceneCamera()
        {
            this.SceneCameraAttachTo = Guid.Empty;
            this.SceneCameraId = Guid.Empty;
            this.SceneCameraLookAt = Guid.Empty;
            this.Type = "ActorTrackSceneCamera";
        }
    }
}
