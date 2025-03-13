using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Actor
{
    public class PeanutTrack : ActorTrackBase
    {
        [XmlAttribute]
        public Guid ActorOverride;
        [XmlAttribute]
        public int PeanutSlotIndex;

        public PeanutTrack()
        {
            this.Type = "PeanutTrack";
            this.ActorOverride = Guid.Empty;
            this.PeanutSlotIndex = 0;
            this.AssignedActorId = new Guid("19dc6cab-aa38-4f09-b9cf-546e1c0ecb8a");
            this.Name = "Peanuts";
        }
    }
}
