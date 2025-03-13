using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackFloatRTPC : KeyTrackFloat
    {
        [XmlAttribute]
        public float MinValue;
        [XmlAttribute]
        public float MaxValue;
        [XmlAttribute]
        public float DefaultValue;
        [XmlAttribute]
        public bool HasEmitter;

        public KeyTrackFloatRTPC()
        {
            MinValue = float.MinValue;
            MaxValue = float.MaxValue;
            HasEmitter = true;
            Type = "KeyTrackFloatRTPC";
        }
    }
}
