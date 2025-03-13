using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TMLGen.Models.Core;

namespace TMLGen.Models.Track.Actor
{
    public class ActorTrackDefault : ActorTrackBase
    {
        [XmlAttribute]
        public string InitialTransform;
        [XmlAttribute]
        public Guid ResourceId;

        public ActorTrackDefault()
        {
            this.ResourceId = Guid.Empty;
            this.Type = "ActorTrackDefault";
        }

        public void setTransform(Quat rotation, float scale, Vector3 translate)
        {
            Transform tra = new Transform(rotation, scale, translate);
            this.InitialTransform = tra.getMatrix().ToString();
        }
    }
}
