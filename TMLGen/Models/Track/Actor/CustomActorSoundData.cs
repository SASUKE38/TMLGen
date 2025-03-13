using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Actor
{
    [XmlType(TypeName = "CustomActorData")]
    public class CustomActorSoundData
    {
        [XmlAttribute]
        public string Type;
        [XmlAttribute]
        public float AttenuationScale;

        public CustomActorSoundData()
        {
            this.Type = "CustomActorSoundData";
            this.AttenuationScale = 1f;
        }
    }
}
