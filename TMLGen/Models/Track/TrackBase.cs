using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Track.Actor;
using TMLGen.Models.Track.Component;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track
{
    [XmlRoot(Namespace = "")]
    [XmlInclude(typeof(ActorTrackSpeaker))]
    [XmlInclude(typeof(ActorTrackSceneCamera))]
    [XmlInclude(typeof(ActorTrackSceneLight))]
    [XmlInclude(typeof(ActorTrackDefault))]
    [XmlInclude(typeof(FolderTrack))]
    [XmlInclude(typeof(PeanutFolderTrack))]
    [XmlInclude(typeof(SceneLightsFolderTrack))]
    [XmlInclude(typeof(PeanutTrack))]
    [XmlInclude(typeof(ComponentTrackActorPropertiesReflection))]
    [XmlInclude(typeof(ComponentTrackAnimation))]
    [XmlInclude(typeof(ComponentTrackAtmosphereAndLighting))]
    [XmlInclude(typeof(ComponentTrackAttitudeEvent))]
    [XmlInclude(typeof(ComponentTrackDoF))]
    [XmlInclude(typeof(ComponentTrackCameraExposure))]
    [XmlInclude(typeof(ComponentTrackCameraFoV))]
    [XmlInclude(typeof(ComponentTrackCameraLookAt))]
    [XmlInclude(typeof(ComponentTrackCameraShot))]
    [XmlInclude(typeof(ComponentTrackEffectPhase))]
    [XmlInclude(typeof(ComponentTrackEmotionEvent))]
    [XmlInclude(typeof(ComponentTrackGenomeTextEvent))]
    [XmlInclude(typeof(ComponentTrackHandsIK))]
    [XmlInclude(typeof(ComponentTrackLookAtEvent))]
    [XmlInclude(typeof(ComponentTrackMaterial))]
    [XmlInclude(typeof(ComponentTrackPhysics))]
    [XmlInclude(typeof(ComponentTrackPlayEffect))]
    [XmlInclude(typeof(ComponentTrackPlayRate))]
    [XmlInclude(typeof(ComponentTrackShowActor))]
    [XmlInclude(typeof(ComponentTrackShowArmor))]
    [XmlInclude(typeof(ComponentTrackShowPeanuts))]
    [XmlInclude(typeof(ComponentTrackShowWeapon))]
    [XmlInclude(typeof(ComponentTrackShapeShift))]
    [XmlInclude(typeof(ComponentTrackSoundEvent))]
    [XmlInclude(typeof(ComponentTrackSplatter))]
    [XmlInclude(typeof(ComponentTrackSprings))]
    [XmlInclude(typeof(ComponentTrackSwitchLocationEvent))]
    [XmlInclude(typeof(ComponentTrackSwitchStageEvent))]
    [XmlInclude(typeof(ComponentTrackTransform))]
    [XmlInclude(typeof(ComponentTrackVoice))]
    [XmlInclude(typeof(KeyTrackAperture))]
    [XmlInclude(typeof(KeyTrackAtmosphere))]
    [XmlInclude(typeof(KeyTrackBoolean))]
    [XmlInclude(typeof(KeyTrackBooleanWrapper))]
    [XmlInclude(typeof(KeyTrackDoFArea))]
    [XmlInclude(typeof(KeyTrackFloat))]
    [XmlInclude(typeof(KeyTrackFrameOfReference))]
    [XmlInclude(typeof(KeyTrackLighting))]
    [XmlInclude(typeof(KeyTrackRotation))]
    [XmlInclude(typeof(KeyTrackShowClothingBoolean))]
    [XmlInclude(typeof(KeyTrackSplatterType))]
    [XmlInclude(typeof(KeyTrackTextureResource))]
    [XmlInclude(typeof(KeyTrackWrapper))]
    [XmlType(TypeName = "Track")]
    public class TrackBase
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public Guid TrackId;
        [XmlAttribute]
        public string Type;

        [XmlElement("Track")]
        public List<TrackBase> Tracks;

        public TrackBase()
        {
            TrackId = Guid.NewGuid();
            Tracks = [];
        }
    }
}
