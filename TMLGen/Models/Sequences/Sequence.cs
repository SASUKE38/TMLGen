using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Component;
using TMLGen.Models.Global;

namespace TMLGen.Models.Sequences
{
    public class Sequence
    {
        [XmlAttribute]
        public Guid Id;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string DialogNodeColor;
        [XmlAttribute]
        public float TimeDuration;
        [XmlAttribute]
        public bool QuestionHoldAutomationOverrideTimelineSettings;
        [XmlAttribute]
        public bool IsQuestionHoldAutomationEnabled;
        [XmlAttribute]
        public float QuestionHoldAutomationCycleSpeed;
        [XmlAttribute]
        public float QuestionHoldAutomationCycleSpeedDeviation;
        [XmlAttribute]
        public float QuestionHoldAutomationStartOffset;
        [XmlAttribute]
        public float QuestionHoldAutomationStartOffsetDeviation;

        [XmlElement]
        public List<SequenceDialogReferenceIds> DialogNodeReference;
        [XmlElement]
        public List<ComponentContainer> Components;
        //CombatTimelineEventHandlerDataModel
        public List<GlobalSoundEvent> EnterPhaseSoundEvents;
        public List<GlobalSoundEvent> ExitPhaseSoundEvents;

        [XmlIgnore]
        public Dictionary<Guid, ComponentContainer> componentDict;
        [XmlIgnore]
        public float lsfStartTime;
        [XmlIgnore]
        public Dictionary<(Guid groupId, Guid actorId), Dictionary<Guid, List<(float startTime, float endTime)>>> usedMaterialTimings;
        [XmlIgnore]
        public Dictionary<(Guid groupId, Guid actorId), Dictionary<Guid, List<(float startTime, float endTime)>>> usedAnimationTimings;
        [XmlIgnore]
        public Dictionary<Guid, int> discoveredSoundEvents;

        public Sequence()
        {
            Id = Guid.NewGuid();
            DialogNodeColor = "#FFF4A460";
            TimeDuration = 1f;
            QuestionHoldAutomationOverrideTimelineSettings = false;
            IsQuestionHoldAutomationEnabled = true;
            QuestionHoldAutomationCycleSpeed = 5f;
            QuestionHoldAutomationCycleSpeedDeviation = 1f;
            QuestionHoldAutomationStartOffset = 0f;
            QuestionHoldAutomationStartOffsetDeviation = 0f;
            DialogNodeReference = [];
            Components = [];
            EnterPhaseSoundEvents = [];
            ExitPhaseSoundEvents = [];
            componentDict = [];
            lsfStartTime = 0f;
            usedMaterialTimings = [];
            usedAnimationTimings = [];
            discoveredSoundEvents = [];
        }

        public void ConvertDictToSerializableList()
        {
            foreach (ComponentContainer container in componentDict.Values)
            {
                Components.Add(container);
            }
        }
    }
}
