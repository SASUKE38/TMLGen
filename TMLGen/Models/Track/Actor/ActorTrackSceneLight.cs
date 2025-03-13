using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Actor
{
    public class ActorTrackSceneLight : ActorTrackBase
    {
        [XmlAttribute]
        public Guid SceneLightId;

        public ActorTrackSceneLight()
        {
            this.Type = "ActorTrackSceneLight";
            this.SceneLightId = Guid.Empty;
        }
    }
}
