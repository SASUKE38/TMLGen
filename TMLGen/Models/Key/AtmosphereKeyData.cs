using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class AtmosphereKeyData
    {
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public float FadeTime;

        public AtmosphereKeyData()
        {
            Id = Guid.Empty;
            FadeTime = 0f;
        }
    }
}
