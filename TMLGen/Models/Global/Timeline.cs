using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Sequences;
using TMLGen.Models.Track;

namespace TMLGen.Models.Global
{
    public class Timeline
    {   
        public GenerationMetadata GenerationMetadata;
        public TimelinePositionManager TimelinePosition;
        public List<TrackBase> Tracks;
        public List<Sequence> Sequences;
        public PlaylistContainer Playlists;
        public TimelineFoleySettings FoleySettings;
        public TimelineExposureSettings ExposureSettings;
        public TimelineQuestionHoldAutomationSettings QuestionHoldAutomation;
        public List<GlobalSoundEvent> EnterSoundEvents;
        public List<GlobalSoundEvent> ExitSoundEvents;
        public TimelineWorldCinematicSettings WorldCinematicSettings;
        public TimelinePersistentPreviewSettings PersistentPreviewSettings;

        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public Guid DialogResourceId;
        [XmlAttribute]
        public bool CanShowPeanuts;
        [XmlAttribute]
        public bool CanShareDummyTemplates;
        [XmlAttribute]
        public bool ShowSubtitles;
        [XmlAttribute]
        public bool OverrideDefaultFoleySettings;
        [XmlAttribute]
        //public PlayerMimicTypes AllowedPeanutTypes;
        public string AllowedPeanutTypes;
        [XmlAttribute]
        public float Offset;
        [XmlAttribute]
        public float MaxKeyDelay;

        [XmlIgnore]
        public int BodyPartForMocap;

        public Timeline()
        {
            Tracks = new List<TrackBase>();
            Sequences = new List<Sequence>();
            Playlists = new PlaylistContainer();
            GenerationMetadata = new GenerationMetadata();
            EnterSoundEvents = new List<GlobalSoundEvent>();
            ExitSoundEvents = new List<GlobalSoundEvent>();
            Id = Guid.NewGuid();
            DialogResourceId = Guid.Empty;
            CanShowPeanuts = true;
            CanShareDummyTemplates = false;
            ShowSubtitles = true;
            OverrideDefaultFoleySettings = false;
            //this.AllowedPeanutTypes = PlayerMimicTypes.Emotion | PlayerMimicTypes.Attitude | PlayerMimicTypes.LookAt;
            AllowedPeanutTypes = "Emotion, Attitude, Look At";
            QuestionHoldAutomation = new TimelineQuestionHoldAutomationSettings();
            WorldCinematicSettings = new TimelineWorldCinematicSettings();
            ExposureSettings = new TimelineExposureSettings();
            FoleySettings = new TimelineFoleySettings();
            PersistentPreviewSettings = new TimelinePersistentPreviewSettings();
            TimelinePosition = new TimelinePositionManager();
        }
    }
}
