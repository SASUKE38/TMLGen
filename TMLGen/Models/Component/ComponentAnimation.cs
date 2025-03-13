using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentAnimation : ComponentBase
    {
        [XmlAttribute]
        public Guid AnimationSourceId;
        [XmlAttribute]
        public double PlayRate;
        [XmlAttribute]
        public double PlayStartOffset;
        [XmlAttribute]
        public bool IsContinuous;
        [XmlAttribute]
        public bool Hold;
        [XmlAttribute]
        public bool IsMirrored;
        [XmlAttribute]
        public string OffsetType;
        [XmlAttribute]
        public double FadeIn;
        [XmlAttribute]
        public double FadeOut;
        [XmlAttribute]
        public double FadeInOffset;
        [XmlAttribute]
        public double FadeOutOffset;
        [XmlAttribute]
        public string Transform;
        [XmlAttribute]
        public bool EnableRootMotion;
        [XmlAttribute]
        public Guid BoneGroupId;
        [XmlAttribute]
        public string AnimationType;

        [XmlIgnore]
        public static readonly string hideVfxPath = "Animation.Hide VFX";

        public ComponentAnimation()
        {
            PlayRate = 1d;
            PlayStartOffset = 0d;
            IsContinuous = false;
            Hold = false;
            IsMirrored = false;
            OffsetType = Enum.GetName(typeof(AnimationOffsetType), AnimationOffsetType.None);
            FadeIn = 0.5d;
            FadeOut = 0.5d;
            FadeInOffset = 0d;
            FadeOutOffset = 0d;
            EnableRootMotion = false;
            BoneGroupId = Guid.Empty;
        }
    }
}
