using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackEnum : KeyTrackBase
    {
        [XmlAttribute]
        public string FallbackValue;

        public KeyTrackEnum()
        {
            Type = "KeyTrackEnum";
        }
    }
}
