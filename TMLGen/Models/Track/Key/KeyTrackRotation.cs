using System.Xml.Serialization;
using TMLGen.Models.Core;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackRotation : KeyTrackBase
    {
        //[XmlAttribute]
        //public Vector3 FallbackValue;

        public KeyTrackRotation()
        {
            Type = "KeyTrackRotation";
        }
    }
}
