using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackFloat : KeyTrackBase
    {
        [XmlAttribute]
        public float FallbackValue;

        public KeyTrackFloat()
        {
            Type = "KeyTrackFloat";
            FallbackValue = 0f;
        }
    }
}
