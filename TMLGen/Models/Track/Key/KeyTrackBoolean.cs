using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackBoolean : KeyTrackBase
    {
        [XmlAttribute]
        public bool FallbackValue;

        public KeyTrackBoolean()
        {
            Type = "KeyTrackBoolean";
            FallbackValue = false;
        }
    }
}
