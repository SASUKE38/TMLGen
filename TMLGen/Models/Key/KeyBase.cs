using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    [XmlInclude(typeof(AttitudeKey))]
    [XmlInclude(typeof(AtmosphereKey))]
    [XmlInclude(typeof(BooleanKey))]
    [XmlInclude(typeof(CameraLookAtKey))]
    [XmlInclude(typeof(EffectPhaseKey))]
    [XmlInclude(typeof(EmotionKey))]
    [XmlInclude(typeof(FloatKey))]
    [XmlInclude(typeof(FrameOfReferenceKey))]
    [XmlInclude(typeof(LookAtKey))]
    [XmlInclude(typeof(ShapeshiftKey))]
    [XmlInclude(typeof(SoundEventKey))]
    [XmlInclude(typeof(SplatterKey))]
    [XmlInclude(typeof(StringKey))]
    [XmlInclude(typeof(SwitchLocationKey))]
    [XmlInclude(typeof(SwitchStageKey))]
    [XmlInclude(typeof(TextureResourceKey))]
    public class KeyBase
    {
        [XmlAttribute]
        public float Time;
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute("KeyInterpolation")]
        public string Interpolation;
        [XmlAttribute]
        public bool IgnoreMimicry;

        public KeyBase()
        {
            Time = 0f;
            Id = Guid.NewGuid();
            Interpolation = Enum.GetName(typeof(KeyInterpolation), KeyInterpolation.CubicAuto);
            IgnoreMimicry = false;
        }
    }
}
