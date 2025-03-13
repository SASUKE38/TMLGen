using System.Xml.Serialization;
using TMLGen.Models.Core;

namespace TMLGen.Models.Key
{
    public class CameraLookAtKeyData : ImmutableTargetKeyDataBase
    {
        [XmlAttribute]
        public string TargetBone;
        [XmlAttribute]
        public string Framing;
        [XmlAttribute]
        public string FramingOffset;
        [XmlAttribute]
        public string FreeBounds;
        [XmlAttribute]
        public string SoftBounds;
        [XmlAttribute]
        public float DampingStrength;
        [XmlAttribute]
        public float FreeZoneDelay;
        [XmlAttribute]
        public float SoftZoneDelay;
        [XmlAttribute]
        public float SoftZoneRampTime;

        public CameraLookAtKeyData()
        {
            TargetBone = string.Empty;
            Framing = new Vector2(0.33333334f).ToString();
            FramingOffset = new Vector2(0).ToString();
            FreeBounds = new Vector2(0.3f, 0.1f).ToString();
            SoftBounds = new Vector2(0.1f, 0.8f).ToString();
            DampingStrength = 1f;
            FreeZoneDelay = 0.3f;
            SoftZoneDelay = 0.3f;
            SoftZoneRampTime = 5;
        }
    }
}
