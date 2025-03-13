using System;
using System.Xml.Serialization;
using TMLGen.Models.Track.Component;

namespace TMLGen.Models.Key
{
    public class SoundEventKeyData
    {
        [XmlAttribute("SoundType")]
        public string Type;
        [XmlAttribute]
        public Guid Event;
        [XmlAttribute]
        public bool KeepAlive;
        [XmlAttribute]
        public float LoopLifetime;
        [XmlAttribute("SoundObjectIndex")]
        public string ObjectIndex;
        [XmlAttribute]
        public string FoleyType;
        [XmlAttribute]
        public string FoleyIntensity;
        [XmlAttribute]
        public string VocalType;

        public SoundEventKeyData()
        {
            Type = Enum.GetName(typeof(SoundType), SoundType.SOUNDTYPE_Default);
            Event = Guid.Empty;
            KeepAlive = false;
            LoopLifetime = 10f;
            ObjectIndex = Enum.GetName(typeof(SoundObjectIndex), SoundObjectIndex.Root);
            FoleyType = Enum.GetName(typeof(FoleyTypeEnum), FoleyTypeEnum.FOLEYTYPE_Medium);
            FoleyIntensity = Enum.GetName(typeof(FoleyIntensityEnum), FoleyIntensityEnum.FOLEYINTENSITY_Soft);
            VocalType = Enum.GetName(typeof(VocalTypeEnum), VocalTypeEnum.VOCALTYPE_None);
        }
    }
}
