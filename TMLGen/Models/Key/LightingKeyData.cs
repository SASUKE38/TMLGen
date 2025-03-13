using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class LightingKeyData
    {
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public float FadeTime;

        public LightingKeyData()
        {
            Id = Guid.Empty;
            FadeTime = 0f;
        }
    }
}
