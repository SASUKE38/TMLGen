using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Sequences;
using TMLGen.Models.Track.Component;

namespace TMLGen.Models.Component
{
    [XmlInclude(typeof(ComponentActorPropertiesReflection))]
    [XmlInclude(typeof(ComponentAnimation))]
    [XmlInclude(typeof(ComponentAtmosphereAndLighting))]
    [XmlInclude(typeof(ComponentAttitudeEvent))]
    [XmlInclude(typeof(ComponentCameraDoF))]
    [XmlInclude(typeof(ComponentCameraExposure))]
    [XmlInclude(typeof(ComponentCameraFoV))]
    [XmlInclude(typeof(ComponentCameraLookAt))]
    [XmlInclude(typeof(ComponentCameraShot))]
    [XmlInclude(typeof(ComponentEffectPhase))]
    [XmlInclude(typeof(ComponentEmotionEvent))]
    [XmlInclude(typeof(ComponentGenomeTextEvent))]
    [XmlInclude(typeof(ComponentHandsIK))]
    [XmlInclude(typeof(ComponentLookAt))]
    [XmlInclude(typeof(ComponentMaterial))]
    [XmlInclude(typeof(ComponentPhysics))]
    [XmlInclude(typeof(ComponentPlayEffect))]
    [XmlInclude(typeof(ComponentPlayRate))]
    [XmlInclude(typeof(ComponentShapeshift))]
    [XmlInclude(typeof(ComponentShowActor))]
    [XmlInclude(typeof(ComponentShowArmor))]
    [XmlInclude(typeof(ComponentShowPeanuts))]
    [XmlInclude(typeof(ComponentShowWeapon))]
    [XmlInclude(typeof(ComponentSoundEvent))]
    [XmlInclude(typeof(ComponentSplatter))]
    [XmlInclude(typeof(ComponentSprings))]
    [XmlInclude(typeof(ComponentSwitchLocation))]
    [XmlInclude(typeof(ComponentSwitchStage))]
    [XmlInclude(typeof(ComponentTransform))]
    [XmlInclude(typeof(ComponentVoice))]
    [XmlInclude(typeof(Camera))]
    public class ComponentBase
    {
        [XmlAttribute]
        public Guid ComponentId;
        [XmlAttribute]
        public float TimeStart;
        [XmlAttribute]
        public float TimeDuration;
        [XmlAttribute]
        public bool IsInfinite;

        [XmlElement]
        public List<Channel> Channel;

        public ComponentBase()
        {
            Channel = new List<Channel>();
            TimeStart = 0f;
            TimeDuration = 1f;
            IsInfinite = true;
        }
    }
}
