using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class SplatterKeyData
    {
        [XmlAttribute]
        public string SplatterChangeMode;
        [XmlAttribute]
        public string SplatterType;
        [XmlAttribute]
        public float Value;

        public SplatterKeyData()
        {
            SplatterChangeMode = Enum.GetName(typeof(SplatterChangeModeEnum), SplatterChangeModeEnum.Absolute);
            SplatterType = Enum.GetName(typeof(SplatterTypeEnum), SplatterTypeEnum.Blood);
            Value = 0f;
        }
    }
}
