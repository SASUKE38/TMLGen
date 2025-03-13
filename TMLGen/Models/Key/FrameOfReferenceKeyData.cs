using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class FrameOfReferenceKeyData : ImmutableTargetKeyDataBase
    {
        [XmlAttribute]
        public bool KeepScale = true;
        [XmlAttribute]
        public bool OneFrameOnly = false;
        [XmlAttribute]
        public string TargetBone = string.Empty;
    }
}
