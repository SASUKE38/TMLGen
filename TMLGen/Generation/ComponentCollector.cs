using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Models.Component;
using TMLGen.Models.Core;
using TMLGen.Models.Global;
using TMLGen.Models.Key;
using TMLGen.Models.Sequences;
using TMLGen.Models.Track;
using TMLGen.Models.Track.Actor;
using TMLGen.Models.Track.Component;
using TMLGen.Models.Track.Key;

namespace TMLGen.Generation
{
    public class ComponentCollector : CollectorBase
    {
        private readonly XElement dbNodes;
        private readonly List<Guid> dbRootNodes = [];
        private readonly List<Guid> rootLocations = [];
        private readonly Dictionary<int, List<TrackBase>> globalTrackMapping = [];
        private readonly Dictionary<(Guid, Guid), List<ComponentTrackMaterial>> otherMaterialTracks = [];
        private readonly Dictionary<(Guid, Guid), List<ComponentTrackAnimation>> animationTracks = [];
        private readonly Dictionary<Guid, int> totalSoundEvents = [];
        private readonly List<ComponentTrackSoundEvent> globalSoundEventTracks = [];
        private readonly HashSet<string> foundUnsupportedComponentTypes = [];
        private static bool separateOverlappingAnimations;
        private readonly Form sender;

        public ComponentCollector(Form sender, XDocument doc, XDocument gdtDoc, XDocument dbDoc, Timeline timeline, bool separateAnimations) : base(doc, gdtDoc, timeline)
        {
            dbNodes = dbDoc.XPathSelectElement("save/region[@id='dialog']/node[@id='dialog']/children/node[@id='nodes']/children");
            IEnumerable<XElement> dbRootNodeElements = dbDoc.XPathSelectElements("save/region[@id='dialog']/node[@id='dialog']/children/node[@id='nodes']/children/node[@id='RootNodes']");
            foreach (XElement element in dbRootNodeElements)
            {
                Guid? rootId = ExtractGuid(element.XPathSelectElement("./attribute[@id='RootNodes']"));
                if (rootId.HasValue) dbRootNodes.Add((Guid)rootId);
            }
            separateOverlappingAnimations = separateAnimations;
            this.sender = sender;
        }

        public override void Collect()
        {
            int currentPhase = 0;
            XElement componentCollection = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='Effect']/children/node[@id='EffectComponents']/children");
            XElement phases = doc.XPathSelectElement("/save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='Effect']/children/node/children");
            XElement timelinePhases = doc.XPathSelectElement("/save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelinePhases']");

            if (componentCollection == null || phases == null || timelinePhases == null)
            {
                LoggingHelper.Write("Source file missing component information!", 2);
                return;
            }

            while (true)
            {
                IEnumerable<XElement> components = currentPhase == 0 ?
                    componentCollection.XPathSelectElements("./node[@id='EffectComponent'][not(attribute[@id='PhaseIndex'])]"):
                    componentCollection.XPathSelectElements("./node[@id='EffectComponent'][attribute[@id='PhaseIndex'][@value='" + currentPhase + "']]");
                if (!components.Any()) break;

                Sequence seq = SequenceInit(currentPhase, phases, timelinePhases, components);

                try
                {
                    foreach (XElement componentData in components)
                    {
                        string componentType = ExtractString(componentData.XPathSelectElement("./attribute[@id='Type']"));

                        switch (componentType)
                        {
                            case "TimelineActorPropertiesReflection":
                                HandleTimelineActorPropertiesReflection(componentData, seq);
                                break;
                            case "TLAnimation":
                                HandleTLAnimation(componentData, seq, AnimationTypeEnum.Base);
                                break;
                            case "TLLayeredAnimation":
                                HandleTLAnimation(componentData, seq, AnimationTypeEnum.Layered);
                                break;
                            case "TLAdditiveAnimation":
                                HandleTLAnimation(componentData, seq, AnimationTypeEnum.Additive);
                                break;
                            case "TLAtmosphereAndLighting":
                                HandleTLAtmosphereAndLighting(componentData, seq);
                                break;
                            case "TLAttitudeEvent":
                                HandleExclusiveImmutableComponent<ComponentTrackAttitudeEvent, ComponentAttitudeEvent, AttitudeKey, AttitudeKeyData>
                                    (componentData, seq, (int)TrackEnum.TLAttitudeEvent, ComponentAttitudeEvent.channelName);
                                break;
                            case "TLCameraDoF":
                                HandleTLCameraDoF(componentData, seq);
                                break;
                            case "TLCameraExposure":
                                HandleTLCameraExposure(componentData, seq);
                                break;
                            case "TLCameraFoV":
                                HandleExclusiveMutableComponent<ComponentTrackCameraFoV, ComponentCameraFoV, FloatKey, float>
                                    (componentData, seq, (int)TrackEnum.TLCameraFoV, ComponentCameraFoV.channelName);
                                break;
                            case "TLCameraLookAt":
                                HandleExclusiveImmutableComponent<ComponentTrackCameraLookAt, ComponentCameraLookAt, CameraLookAtKey, CameraLookAtKeyData>
                                    (componentData, seq, (int)TrackEnum.TLCameraLookAt, ComponentCameraLookAt.channelName);
                                break;
                            case "TLEffectPhaseEvent":
                                HandleExclusiveMutableComponent<ComponentTrackEffectPhase, ComponentEffectPhase, EffectPhaseKey, int>
                                    (componentData, seq, (int)TrackEnum.TLEffectPhaseEvent, ComponentEffectPhase.channelName);
                                break;
                            case "TLEmotionEvent":
                                HandleExclusiveImmutableComponent<ComponentTrackEmotionEvent, ComponentEmotionEvent, EmotionKey, EmotionKeyData>
                                    (componentData, seq, (int)TrackEnum.TLEmotionEvent, ComponentEmotionEvent.channelName);
                                break;
                            case "TLGenomeTextEvent":
                                HandleExclusiveMutableComponent<ComponentTrackGenomeTextEvent, ComponentGenomeTextEvent, StringKey, string>
                                     (componentData, seq, (int)TrackEnum.TLGenomeTextEvent, ComponentGenomeTextEvent.channelName);
                                break;
                            case "TLHandsIK":
                                HandleExclusiveMutableComponent<ComponentTrackHandsIK, ComponentHandsIK, BooleanKey, bool>
                                     (componentData, seq, (int)TrackEnum.TLHandsIK, ComponentHandsIK.channelName);
                                break;
                            case "TLLookAtEvent":
                                HandleExclusiveImmutableComponent<ComponentTrackLookAtEvent, ComponentLookAt, LookAtKey, LookAtKeyData>
                                    (componentData, seq, (int)TrackEnum.TLLookAtEvent, ComponentLookAt.channelName);
                                break;
                            case "TLMaterial":
                                HandleTLMaterial(componentData, seq);
                                break;
                            case "TLPhysics": // shares handler method with TLHandsIK
                                HandleExclusiveMutableComponent<ComponentTrackPhysics, ComponentPhysics, BooleanKey, bool>
                                     (componentData, seq, (int)TrackEnum.TLPhysics, ComponentPhysics.channelName);
                                break;
                            case "TLPlayEffectEvent":
                                HandleExclusiveMutableComponent<ComponentTrackPlayEffect, ComponentPlayEffect, BooleanKey, bool>
                                     (componentData, seq, (int)TrackEnum.TLPlayEffectEvent, ComponentPlayEffect.channelName);
                                break;
                            case "TLPlayRate":
                                HandleGlobalExclusiveMutableComponent<ComponentTrackPlayRate, ComponentPlayRate, FloatKey, float>
                                     (componentData, seq, (int)TrackEnum.TLPlayRate, ComponentPlayRate.channelName);
                                break;
                            case "TLShapeShift":
                                HandleExclusiveImmutableComponent<ComponentTrackShapeShift, ComponentShapeshift, ShapeshiftKey, ShapeshiftKeyData>
                                    (componentData, seq, (int)TrackEnum.TLShapeshift, ComponentShapeshift.channelName);
                                break;
                            case "TLShot":
                                HandleTLShot(componentData, seq);
                                break;
                            case "TLShowArmor":
                                HandleTLShowArmor(componentData, seq);
                                break;
                            case "TLShowPeanuts":
                                HandleGlobalExclusiveMutableComponent<ComponentTrackShowPeanuts, ComponentShowPeanuts, BooleanKey, bool>
                                     (componentData, seq, (int)TrackEnum.TLShowPeanuts, ComponentShowPeanuts.channelName);
                                break;
                            case "TLShowVisual":
                                HandleTLShowVisual(componentData, seq);
                                break;
                            case "TLShowWeapon":
                                HandleExclusiveMutableComponent<ComponentTrackShowWeapon, ComponentShowWeapon, BooleanKey, bool>
                                     (componentData, seq, (int)TrackEnum.TLShowWeapon, ComponentShowWeapon.channelName);
                                break;
                            case "TLSoundEvent":
                                HandleTLSoundEvent(componentData, seq);
                                break;
                            case "TLSplatter":
                                HandleTLSplatter(componentData, seq);
                                break;
                            case "TLSprings":
                                HandleTLSprings(componentData, seq);
                                break;
                            case "TLSwitchLocationEvent":
                                HandleGlobalExclusiveImmutableComponent<ComponentTrackSwitchLocationEvent, ComponentSwitchLocation, SwitchLocationKey, SwitchLocationKeyData>
                                    (componentData, seq, (int)TrackEnum.TLSwitchLocation, ComponentSwitchLocation.channelName);
                                break;
                            case "TLSwitchStageEvent":
                                HandleGlobalExclusiveImmutableComponent<ComponentTrackSwitchStageEvent, ComponentSwitchStage, SwitchStageKey, SwitchStageKeyData>
                                    (componentData, seq, (int)TrackEnum.TLSwitchStage, ComponentSwitchStage.channelName);
                                break;
                            case "TLTransform":
                                HandleTLTransform(componentData, seq);
                                break;
                            case "TLVoice":
                                HandleTLVoice(componentData, seq);
                                break;
                            default:
                                if (!foundUnsupportedComponentTypes.Contains(componentType))
                                {
                                    LoggingHelper.Write("Timeline contains unsupported component type: " + componentType, 2);
                                    foundUnsupportedComponentTypes.Add(componentType);
                                }
                                break;
                        }
                    }

                    seq.ConvertDictToSerializableList();
                    timeline.Sequences.Add(seq);

                    currentPhase++;
                }
                catch (KeyNotFoundException)
                {
                    LoggingHelper.Write("Timeline data necessary for component collection was missing. Are the input files correct?", 2);
                    throw;
                }
            }
            TrySetTimelineLocation();
        }

        // Sequence Initialization

        private Sequence SequenceInit(int currentPhase, XElement phases, XElement timelinePhases, IEnumerable<XElement> components)
        {
            Sequence seq = new();

            IEnumerable<XElement> dialogNodeData = timelinePhases.XPathSelectElements("./children/node/children/node[attribute[@id='MapValue'][@value='" + currentPhase + "']]");
            XElement phaseData = null;
            Guid? curId = null;
            foreach (XElement ele in dialogNodeData)
            {
                curId = ExtractGuid(ele.XPathSelectElement("./attribute[@id='MapKey']"));
                if (curId.HasValue)
                {
                    phaseData = phases.XPathSelectElement("./node[attribute[@id='DialogNodeId'][@value='" + curId + "']]");
                    if (phaseData != null) break;
                }
            }
            if (phaseData != null)
            {
                seq.Name = "Phase " + currentPhase;
                seq.TimeDuration = ExtractFloat(phaseData.XPathSelectElement("./attribute[@id='Duration']")) ?? seq.TimeDuration;
                SequenceDialogReferenceIds refId = new SequenceDialogReferenceIds();
                refId.DialogNodeId = refId.ReferenceId = (Guid)ExtractGuid(phaseData.XPathSelectElement("./attribute[@id='DialogNodeId']"));
                seq.DialogNodeReference.Add(refId);

                string nodeType = ExtractString(dbNodes.XPathSelectElement("./node[attribute[@id='UUID'][@value='" + curId + "']]/attribute[@id='constructor']"));
                bool? isEnd = ExtractBool(dbNodes.XPathSelectElement("./node[attribute[@id='UUID'][@value='" + curId + "']]/attribute[@id='endnode']"));
                if (isEnd.HasValue && (bool)isEnd) nodeType = "End";
                if (Enum.TryParse(nodeType, out DialogNodeColors colorType))
                {
                    uint color = (uint)Enum.Parse(typeof(DialogNodeColors), nodeType);
                    seq.DialogNodeColor = "#" + color.ToString("X");
                }
                XElement questionHoldData = phaseData.XPathSelectElement("./children/node[@id='QuestionHoldAutomation']");
                seq.QuestionHoldAutomationOverrideTimelineSettings = ExtractBool(phaseData.XPathSelectElement("./attribute[@id='IsOverridingTimelineQuestionHoldAutomationSettings']")) ?? seq.QuestionHoldAutomationOverrideTimelineSettings;
                seq.IsQuestionHoldAutomationEnabled = ExtractBool(questionHoldData.XPathSelectElement("./attribute[@id='IsEnabled']")) ?? seq.IsQuestionHoldAutomationEnabled;
                seq.QuestionHoldAutomationCycleSpeed = ExtractFloat(questionHoldData.XPathSelectElement("./attribute[@id='CycleSpeed']")) ?? seq.QuestionHoldAutomationCycleSpeed;
                seq.QuestionHoldAutomationCycleSpeedDeviation = ExtractFloat(questionHoldData.XPathSelectElement("./attribute[@id='CycleSpeedDeviation']")) ?? seq.QuestionHoldAutomationCycleSpeedDeviation;
                seq.QuestionHoldAutomationStartOffset = ExtractFloat(questionHoldData.XPathSelectElement("./attribute[@id='StartOffset']")) ?? seq.QuestionHoldAutomationStartOffset;
                seq.QuestionHoldAutomationStartOffsetDeviation = ExtractFloat(questionHoldData.XPathSelectElement("./attribute[@id='StartOffsetDeviation']")) ?? seq.QuestionHoldAutomationStartOffsetDeviation;

                seq.EnterPhaseSoundEvents.AddRange(CollectPhaseSoundEvents("EnterPhaseSoundEvents", currentPhase));
                seq.ExitPhaseSoundEvents.AddRange(CollectPhaseSoundEvents("ExitPhaseSoundEvents", currentPhase));

                if (currentPhase != 0)
                {
                    float startCand = float.MaxValue;
                    foreach (XElement componentData in components)
                    {
                        float? startTime = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='StartTime']"));
                        if (startTime.HasValue && (float)startTime < startCand) startCand = (float)startTime;
                    }
                    if (startCand < float.MaxValue) seq.lsfStartTime = startCand;
                }
            }
            return seq;
        }

        private List<GlobalSoundEvent> CollectPhaseSoundEvents(string evType, int currentPhase)
        {
            IEnumerable<XElement> events = doc.XPathSelectElements("save/region[@id='" + evType + "']/node[@id='" + evType + "']/children/node/children/node[attribute[@id='MapKey'][@value='" + currentPhase + "']]/children/node/children/node");
            return CollectGlobalSoundEvents(events);
        }

        // Component Initialization

        /// <summary>
        /// Should be called when a new component needs to be added to a UNIQUE
        /// (actor can only have one of this type) track.
        /// Gets the associated track to be added to if it exists in the actor's 
        /// track collection and creates a new one if not. Obtains the track ID,
        /// new component container, and new component.
        /// </summary>
        /// <typeparam name="TrackType"></typeparam>
        /// <typeparam name="ComponentType"></typeparam>
        /// <param name="actorId"></param>
        /// <param name="componentData"></param>
        /// <param name="seq"></param>
        /// <param name="trackKey"></param>
        /// <param name="trackId"></param>
        /// <returns></returns>
        private static ComponentType GetExclusiveComponent<TrackType, ComponentType>(Guid actorId, XElement componentData, Sequence seq, int trackKey, out Guid trackId, out ComponentContainer curContainer)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
        {
            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                if (!actorTrack.actorChildTracks.TryGetValue(trackKey, out List<TrackBase> trackList))
                {
                    TrackType newTrack = new();
                    trackId = newTrack.TrackId;
                    actorTrack.actorChildTracks.Add(trackKey, [newTrack]);
                    actorTrack.Tracks.Add(newTrack);
                }
                else
                {
                    trackId = trackList[0].TrackId;
                }
                ComponentType curComponent = GetComponentInternal<ComponentType>(trackId, componentData, seq, out curContainer);

                return curComponent;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Should be called when a new component needs to be added to a UNIQUE 
        /// global track. Gets the associated track to be added to if it exists
        /// in the actor's track collection and creates a new one if not.
        /// Obtains the track ID, new component container, and new component.
        /// </summary>
        /// <typeparam name="TrackType"></typeparam>
        /// <typeparam name="ComponentType"></typeparam>
        /// <param name="componentData"></param>
        /// <param name="seq"></param>
        /// <param name="trackKey"></param>
        /// <param name="trackId"></param>
        /// <param name="curContainer"></param>
        /// <returns></returns>
        private ComponentType GetExclusiveGlobalComponent<TrackType, ComponentType>(XElement componentData, Sequence seq, int trackKey, out Guid trackId, out ComponentContainer curContainer)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
        {
            if (!globalTrackMapping.TryGetValue(trackKey, out List<TrackBase> trackList))
            {
                TrackType newTrack = new();
                trackId = newTrack.TrackId;
                globalTrackMapping.Add(trackKey, [newTrack]);
                timeline.Tracks.Add(newTrack);
            }
            else
            {
                trackId = trackList[0].TrackId;
            }
            ComponentType curComponent = GetComponentInternal<ComponentType>(trackId, componentData, seq, out curContainer);

            return curComponent;
        }

        /// <summary>
        /// Should be called when a new component needs to be added to
        /// a UNIQUE track that could be global or belong to an actor.
        /// Gets the associated track to be added to if it exists in the actor's 
        /// track collection and creates a new one if not. Obtains the track ID,
        /// new component container, and new component.
        /// </summary>
        /// <typeparam name="TrackType"></typeparam>
        /// <typeparam name="ComponentType"></typeparam>
        /// <param name="componentData"></param>
        /// <param name="seq"></param>
        /// <param name="trackKey"></param>
        /// <param name="trackId"></param>
        /// <param name="curContainer"></param>
        /// <returns></returns>
        private ComponentType GetAmbiguousExclsuiveComponent<TrackType, ComponentType>(XElement componentData, Sequence seq, int trackKey, out Guid trackId, out ComponentContainer curContainer)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
        {
            Guid? actorId = GetComponentActor(componentData);
            ComponentType curComponent = actorId.HasValue ?
                GetExclusiveComponent<TrackType, ComponentType>((Guid)actorId, componentData, seq, trackKey, out trackId, out curContainer) :
                GetExclusiveGlobalComponent<TrackType, ComponentType>(componentData, seq, trackKey, out trackId, out curContainer);
            return curComponent;
        }

        private static ComponentAnimation GetAnimationComponent(Guid actorId, XElement componentData, Sequence seq, out Guid trackId, out ComponentContainer curContainer)
        {
            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                int trackKey = (int)TrackEnum.TLAnimation;
                trackId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='AnimationGroup']")) ?? Guid.NewGuid();
                if (!actorTrack.actorChildTracks.TryGetValue(trackKey, out List<TrackBase> trackList))
                {
                    ComponentTrackAnimation newTrack = GetNewAnimationTrack(componentData, trackId);
                    actorTrack.actorChildTracks.Add(trackKey, [newTrack]);
                    actorTrack.Tracks.Add(newTrack);
                }
                else if (!ActorHasChildTrack(trackId, trackList))
                {
                    ComponentTrackAnimation newTrack = GetNewAnimationTrack(componentData, trackId);
                    trackList.Add(newTrack);
                    actorTrack.Tracks.Add(newTrack);
                }
                return GetComponentInternal<ComponentAnimation>(trackId, componentData, seq, out curContainer);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        // Combine this animation separation stuff with the material track logic
        private ComponentAnimation GetSeparatedAnimationComponent(Guid actorId, XElement componentData, Sequence seq, out Guid trackId, out ComponentContainer curContainer)
        {
            float startTime = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='StartTime']")) ?? 0f;
            float endTime = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='EndTime']")) ?? 0f;

            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                int trackKey = (int)TrackEnum.TLAnimation;
                trackId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='AnimationGroup']")) ?? Guid.NewGuid();
                if (!actorTrack.actorChildTracks.TryGetValue(trackKey, out List<TrackBase> trackList))
                {
                    ComponentTrackAnimation newTrack = GetNewAnimationTrack(componentData, trackId);
                    actorTrack.actorChildTracks.Add(trackKey, [newTrack]);
                    actorTrack.Tracks.Add(newTrack);
                    animationTracks.Add((trackId, actorId), [newTrack]);
                    var val = new Dictionary<Guid, List<(float startTime, float endTime)>> { { newTrack.TrackId, [(startTime, endTime)] } };
                    seq.usedAnimationTimings.Add((trackId, actorId), val);
                }
                else if (!animationTracks.TryGetValue((trackId, actorId), out List<ComponentTrackAnimation> actorMaterialTracks))
                {
                    ComponentTrackAnimation newTrack = GetNewAnimationTrack(componentData, trackId);
                    trackList.Add(newTrack);
                    actorTrack.Tracks.Add(newTrack);
                    animationTracks.Add((trackId, actorId), [newTrack]);
                    var val = new Dictionary<Guid, List<(float startTime, float endTime)>> { { newTrack.TrackId, [(startTime, endTime)] } };
                    seq.usedAnimationTimings.Add((trackId, actorId), val);
                }
                else
                {
                    ComponentTrackAnimation selectedTrack = GetCompatibleAnimationTrack(componentData, seq, trackId, actorId, startTime, endTime);
                    if (selectedTrack == null)
                    {
                        selectedTrack = GetNewAnimationTrack(componentData, trackId);
                        selectedTrack.TrackId = Guid.NewGuid();
                        trackList.Add(selectedTrack);
                        actorTrack.Tracks.Add(selectedTrack);
                        animationTracks[(trackId, actorId)].Add(selectedTrack);
                        var newTimingMap = seq.usedAnimationTimings[(trackId, actorId)];
                        newTimingMap.Add(selectedTrack.TrackId, [(startTime, endTime)]);
                    }
                    trackId = selectedTrack.TrackId;
                }
                return GetComponentInternal<ComponentAnimation>(trackId, componentData, seq, out curContainer);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private static ComponentTrackAnimation GetNewAnimationTrack(XElement componentData, Guid trackId)
        {
            ComponentTrackAnimation newTrack = new() { TrackId = trackId };
            string slotKey = ExtractString(componentData.XPathSelectElement("./attribute[@id='AnimationSlot']"));
            if (slotKey != null && ComponentTrackAnimation.slotIdMapping.TryGetValue(slotKey, out Guid value))
            {
                newTrack.SlotId = value;
            }
            return newTrack;
        }

        private ComponentTrackAnimation GetCompatibleAnimationTrack(XElement componentData, Sequence seq, Guid groupId, Guid actorId, float startTime, float endTime)
        {
            if (!seq.usedAnimationTimings.TryGetValue((groupId, actorId), out Dictionary<Guid, List<(float startTime, float endTime)>> idDict))
            {
                ComponentTrackAnimation selected = animationTracks[(groupId, actorId)][0];
                var val = new Dictionary<Guid, List<(float startTime, float endTime)>> { { selected.TrackId, [(startTime, endTime)] } };
                seq.usedAnimationTimings.Add((groupId, actorId), val);
                return selected;
            }
            else
            {
                foreach (ComponentTrackAnimation track in animationTracks[(groupId, actorId)])
                {
                    if (idDict.TryGetValue(track.TrackId, out List<(float startTime, float endTime)> timeList))
                    {
                        if (IsTrackTimingValid(timeList, startTime, endTime))
                        {
                            timeList.Add((startTime, endTime));
                            return track;
                        }
                    }
                    else if (timeList == null)
                    {
                        idDict[track.TrackId] = [(startTime, endTime)];
                        return track;
                    }
                }
                return null;
            }
        }

        private ComponentMaterial GetMaterialComponent(Guid actorId, XElement componentData, Sequence seq, out ComponentTrackMaterial trackToUse, out Guid trackId, out ComponentContainer curContainer)
        {
            Guid materialGroup = (Guid)ExtractGuid(componentData.XPathSelectElement("./attribute[@id='GroupId']"));
            XElement slotSettings = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='MaterialGroupLookup']/children/node[@id='Object']/children/node[@id='Object'][attribute[@id='MapKey'][@value='" + materialGroup + "']]");
            return slotSettings == null ? 
                GetOtherMaterialComponent(actorId, componentData, seq, materialGroup, out trackToUse, out trackId, out curContainer) :
                GetSlotMaterialComponent(actorId, componentData, seq, slotSettings, materialGroup, out trackToUse, out trackId, out curContainer);
        }

        private ComponentMaterial GetSlotMaterialComponent(Guid actorId, XElement componentData, Sequence seq, XElement slotSettings, Guid groupId, out ComponentTrackMaterial trackToUse, out Guid trackId, out ComponentContainer curContainer)
        {
            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                int trackKey = (int)TrackEnum.TLMaterial;
                if (!actorTrack.actorChildTracks.TryGetValue(trackKey, out List<TrackBase> trackList))
                {
                    ComponentTrackMaterial newTrack = GetNewSlotMaterialTrack(actorTrack.ActorId, slotSettings, groupId);
                    actorTrack.actorChildTracks.Add(trackKey, [newTrack]);
                    actorTrack.Tracks.Add(newTrack);
                    trackToUse = newTrack;
                }
                else if (!ActorHasChildTrack(groupId, trackList))
                {
                    ComponentTrackMaterial newTrack = GetNewSlotMaterialTrack(actorTrack.ActorId, slotSettings, groupId);
                    trackList.Add(newTrack);
                    actorTrack.Tracks.Add(newTrack);
                    trackToUse = newTrack;
                }
                else
                {
                    trackToUse = trackList.Find(t => t.TrackId == groupId) as ComponentTrackMaterial;
                }
                trackId = groupId;
                return GetComponentInternal<ComponentMaterial>(trackId, componentData, seq, out curContainer);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private ComponentTrackMaterial GetNewSlotMaterialTrack(Guid actorId, XElement slotSettings, Guid groupId)
        {
            ComponentTrackMaterial newTrack = new() { Name = "Slot Material", TrackId = groupId, MaterialGroupId = groupId };
            XElement subSettings = slotSettings.XPathSelectElement("./children/node[@id='MapValue']/children/node[@id='strMaterialReference']");
            Guid materialId = ExtractGuid(slotSettings.XPathSelectElement("./children/node[@id='MapValue']/attribute[@id='MaterialResourceID']")) ?? Guid.Empty;
            newTrack.Slot = ExtractInt(subSettings.XPathSelectElement("./attribute[@id='SlotIndex']")) ?? 0;
            newTrack.VisualResourceId = ExtractGuid(subSettings.XPathSelectElement("./attribute[@id='VisualResourceID']")) ?? newTrack.VisualResourceId;
            newTrack.CharacterVisualResourceId = FindCharacterVisualId(actorId, materialId, newTrack.VisualResourceId);
            return newTrack;
        }

        private ComponentMaterial GetOtherMaterialComponent(Guid actorId, XElement componentData, Sequence seq, Guid groupId, out ComponentTrackMaterial trackToUse, out Guid trackId, out ComponentContainer curContainer)
        {
            float startTime = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='StartTime']")) ?? 0f;
            float endTime = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='EndTime']")) ?? 0f;
            
            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                int trackKey = (int)TrackEnum.TLMaterial;
                if (!actorTrack.actorChildTracks.TryGetValue(trackKey, out List<TrackBase> trackList))
                {
                    ComponentTrackMaterial newTrack = GetNewOtherMaterialTrack(componentData, groupId);
                    actorTrack.actorChildTracks.Add(trackKey, [newTrack]);
                    actorTrack.Tracks.Add(newTrack);
                    otherMaterialTracks.Add((groupId, actorId), [newTrack]);
                    var val = new Dictionary<Guid, List<(float startTime, float endTime)>> { { newTrack.TrackId, [(startTime, endTime)] } };
                    seq.usedMaterialTimings.Add((groupId, actorId), val);
                    trackId = newTrack.TrackId;
                    trackToUse = newTrack;
                }
                else if (!otherMaterialTracks.TryGetValue((groupId, actorId), out List<ComponentTrackMaterial> actorMaterialTracks))
                {
                    ComponentTrackMaterial newTrack = GetNewOtherMaterialTrack(componentData, groupId);
                    trackList.Add(newTrack);
                    actorTrack.Tracks.Add(newTrack);
                    otherMaterialTracks.Add((groupId, actorId), [newTrack]);
                    var val = new Dictionary<Guid, List<(float startTime, float endTime)>> { { newTrack.TrackId, [(startTime, endTime)] } };
                    seq.usedMaterialTimings.Add((groupId, actorId), val);
                    trackId = newTrack.TrackId;
                    trackToUse = newTrack;
                }
                else
                {
                    ComponentTrackMaterial selectedTrack = GetCompatibleMaterialTrack(componentData, seq, groupId, actorId, startTime, endTime);
                    if (selectedTrack == null)
                    {
                        selectedTrack = GetNewOtherMaterialTrack(componentData, groupId);
                        trackList.Add(selectedTrack);
                        actorTrack.Tracks.Add(selectedTrack);
                        otherMaterialTracks[(groupId, actorId)].Add(selectedTrack);
                        var newTimingMap = seq.usedMaterialTimings[(groupId, actorId)];
                        newTimingMap.Add(selectedTrack.TrackId, [(startTime, endTime)]);
                    }
                    trackId = selectedTrack.TrackId;
                    trackToUse = selectedTrack;
                }
                return GetComponentInternal<ComponentMaterial>(trackId, componentData, seq, out curContainer);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private ComponentTrackMaterial GetCompatibleMaterialTrack(XElement componentData, Sequence seq, Guid groupId, Guid actorId, float startTime, float endTime)
        {
            if (!seq.usedMaterialTimings.TryGetValue((groupId, actorId), out Dictionary<Guid, List<(float startTime, float endTime)>> idDict))
            {
                ComponentTrackMaterial selected = otherMaterialTracks[(groupId, actorId)][0];
                var val = new Dictionary<Guid, List<(float startTime, float endTime)>> { {selected.TrackId, [(startTime, endTime)] } };
                seq.usedMaterialTimings.Add((groupId, actorId), val);
                return selected;
            }
            else
            {
                foreach (ComponentTrackMaterial track in otherMaterialTracks[(groupId, actorId)])
                {
                    if (idDict.TryGetValue(track.TrackId, out List<(float startTime, float endTime)> timeList))
                    {
                        if (IsTrackTimingValid(timeList, startTime, endTime))
                        {
                            timeList.Add((startTime, endTime));
                            return track;
                        }
                    }
                    else if (timeList == null)
                    {
                        idDict[track.TrackId] = [(startTime, endTime)];
                        return track;
                    }
                }
                return null;
            }
        }

        private static bool IsTrackTimingValid(List<(float startTime, float endTime)> timeList, float startA, float endA)
        {
            foreach ((float startB, float endB) in timeList)
            {
                if (startA < endB && startB < endA)
                {
                    return false;
                }
            }
            return true;
        }

        private static ComponentTrackMaterial GetNewOtherMaterialTrack(XElement componentData, Guid groupId)
        {
            ComponentTrackMaterial newTrack = new() { MaterialGroupId = groupId };
            newTrack.IsOverlay = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsOverlay']")) ?? newTrack.IsOverlay;
            newTrack.OverlayPriority = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='OverlayPriority']")) ?? newTrack.OverlayPriority;
            if (newTrack.IsOverlay) newTrack.Name = "Overlay Material";
            return newTrack;
        }

        private ComponentSoundEvent GetSoundEventComponent(Guid actorId, XElement componentData, Sequence seq, out Guid trackId, out ComponentContainer curContainer)
        {
            bool shouldAddTrack = false;
            totalSoundEvents.TryGetValue(actorId, out int totalEvents);
            seq.discoveredSoundEvents.TryGetValue(actorId, out int discoveredEvents);
            if (totalEvents == 0) 
            {
                shouldAddTrack = true;
                totalSoundEvents[actorId] = 1;
            };
            if (discoveredEvents == 0)
                seq.discoveredSoundEvents[actorId] = 1;
            else 
                seq.discoveredSoundEvents[actorId] += 1;
            if (seq.discoveredSoundEvents[actorId] > totalSoundEvents[actorId]) 
            {
                shouldAddTrack = true;
                totalSoundEvents[actorId] += 1;
            }

            if (actorId == Guid.Empty)
            {
                if (shouldAddTrack)
                {
                    ComponentTrackSoundEvent newTrack = new();
                    timeline.Tracks.Add(newTrack);
                    globalSoundEventTracks.Add(newTrack);
                    trackId = newTrack.TrackId;
                }
                else 
                    trackId = globalSoundEventTracks[discoveredEvents].TrackId;
            }
            else
            {
                try
                {
                    ActorTrackBase actorTrack = trackMapping[actorId];
                    int trackKey = (int)TrackEnum.TLSoundEvent;
                    if (shouldAddTrack)
                    {
                        ComponentTrackSoundEvent newTrack = new();
                        if (!actorTrack.actorChildTracks.TryGetValue(trackKey, out List<TrackBase> trackList))
                            actorTrack.actorChildTracks.Add(trackKey, [newTrack]);
                        else
                            trackList.Add(newTrack);
                        actorTrack.Tracks.Add(newTrack);
                        trackId = newTrack.TrackId;
                    }
                    else
                        trackId = actorTrack.actorChildTracks[trackKey][discoveredEvents].TrackId;
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
            }
            return GetComponentInternal<ComponentSoundEvent>(trackId, componentData, seq, out curContainer);
        }

        private static ComponentSprings GetSpringsComponent(Guid actorId, XElement componentData, Sequence seq, out Guid trackId, out ComponentContainer curContainer, out string trackName)
        {
            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                uint visualFlag = ExtractUint(componentData.XPathSelectElement("./attribute[@id='VisualFlag']")) ?? (uint)SpringsVisualFlag.AllVisuals;
                string visualName = Enum.GetName(typeof(SpringsVisualFlag), visualFlag);
                trackName = ComponentSprings.channelNameBase + " " + visualName;
                if (!actorTrack.actorChildSpringTracks.TryGetValue(visualFlag, out TrackBase track))
                {
                    ComponentTrackSprings newTrack = new() { VisualTargets = visualName, Name = trackName };
                    actorTrack.actorChildSpringTracks.Add(visualFlag, newTrack);
                    actorTrack.Tracks.Add(newTrack);
                    trackId = newTrack.TrackId;
                }
                else
                {
                    trackId = track.TrackId;
                }
                return GetComponentInternal<ComponentSprings>(trackId, componentData, seq, out curContainer);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        private static bool ActorHasChildTrack(Guid trackId, List<TrackBase> trackList)
        {
            foreach (TrackBase track in trackList)
            {
                if (track.TrackId == trackId) return true;
            }
            return false;
        }

        private static ComponentType GetComponentInternal<ComponentType>(Guid trackId, XElement componentData, Sequence seq, out ComponentContainer curContainer)
            where ComponentType : ComponentBase, new()
        {
            curContainer = GetContainer(seq, trackId);
            ComponentType curComponent = new();
            ComponentInit(componentData, seq, curComponent);

            return curComponent;
        }

        private static Guid? GetComponentActor(XElement componentData)
        {
            return ExtractGuid(componentData.XPathSelectElement("./children/node[@id='Actor']/attribute[@id='UUID']"));
        }

        private static void ComponentInit(XElement componentData, Sequence seq, ComponentBase res)
        {
            res.ComponentId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='ID']")) ?? Guid.NewGuid();
            float lsfStart = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='StartTime']")) ?? 0f;
            res.TimeStart = lsfStart - seq.lsfStartTime;
            float lsfEnd = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='EndTime']")) ?? 0f;
            res.TimeDuration = lsfEnd - lsfStart;

            bool? snappedToEnd = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsSnappedToEnd']"));
            //if (snappedToEnd.HasValue && (bool)snappedToEnd && (res.TimeDuration == seq.TimeDuration)) res.IsInfinite = true;
        }

        private static ComponentContainer GetContainer(Sequence seq, Guid trackId)
        {
            if (!seq.componentDict.TryGetValue(trackId, out ComponentContainer curContainer))
                curContainer = new ComponentContainer { TrackId = trackId };

            return curContainer;
        }

        private static void HandleExclusiveMutableComponent<TrackType, ComponentType, KeyType, KeyDataType>(XElement componentData, Sequence seq, int trackKey, string channelName)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
            where KeyType : MutableKeyBase<KeyDataType>, new()
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentType curComponent =
                    GetExclusiveComponent<TrackType, ComponentType>((Guid)actorId, componentData, seq, trackKey, out Guid trackId, out ComponentContainer curContainer);

                Channel newChannel = new() { Name = channelName };
                foreach (XElement keyData in GetKeyDataCollection(componentData))
                {
                    KeyType key = GetMutableKey<KeyType, KeyDataType>(keyData, seq);

                    switch (trackKey)
                    {
                        case (int)TrackEnum.TLCameraFoV:
                            HandleTLCameraFoV(keyData, key as FloatKey);
                            break;
                        case (int)TrackEnum.TLEffectPhaseEvent:
                            HandleTLEffectPhase(keyData, key as EffectPhaseKey);
                            break;
                        case (int)TrackEnum.TLGenomeTextEvent:
                            HandleTLGenomeTextEvent(keyData, key as StringKey);
                            break;
                        case (int)TrackEnum.TLHandsIK:
                        case (int)TrackEnum.TLPhysics: // shares handler with TLHandsIK
                            HandleTLHandsIKOrTLPhysics(keyData, key as BooleanKey);
                            break;
                        case (int)TrackEnum.TLPlayEffectEvent:
                            HandleTLPlayEffect(keyData, key as BooleanKey);
                            break;
                        case (int)TrackEnum.TLShowWeapon:
                            HandleTLShowWeapon(keyData, key as BooleanKey);
                            break;
                    }
                    BindKeyToChannel(key, newChannel);
                }
                BindChannelAndComponent(newChannel, curComponent, curContainer, seq, trackId);
            }
        }

        private void HandleGlobalExclusiveMutableComponent<TrackType, ComponentType, KeyType, KeyDataType>(XElement componentData, Sequence seq, int trackKey, string channelName)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
            where KeyType : MutableKeyBase<KeyDataType>, new()
        {
            ComponentType curComponent =
                GetExclusiveGlobalComponent<TrackType, ComponentType>(componentData, seq, trackKey, out Guid trackId, out ComponentContainer curContainer);

            Channel newChannel = new() { Name = channelName };
            foreach (XElement keyData in GetKeyDataCollection(componentData))
            {
                KeyType key = GetMutableKey<KeyType, KeyDataType>(keyData, seq);
                
                switch (trackKey)
                {
                    case (int)TrackEnum.TLPlayRate:
                        HandleTLPlayRate(keyData, key as FloatKey);
                        break;
                    case (int)TrackEnum.TLShowPeanuts:
                        HandleTLShowPeanuts(keyData, key as BooleanKey);
                        break;
                }                
                BindKeyToChannel(key, newChannel);
            }
            BindChannelAndComponent(newChannel, curComponent, curContainer, seq, trackId);
        }

        private static void HandleExclusiveImmutableComponent<TrackType, ComponentType, KeyType, KeyDataType>(XElement componentData, Sequence seq, int trackKey, string channelName)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
            where KeyType : ImmutableKeyBase<KeyDataType>, new()
            where KeyDataType : new()
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentType curComponent =
                    GetExclusiveComponent<TrackType, ComponentType>((Guid)actorId, componentData, seq, trackKey, out Guid trackId, out ComponentContainer curContainer);

                Channel newChannel = new() { Name = channelName };
                foreach (XElement keyData in GetKeyDataCollection(componentData))
                {
                    GetImmutableKey(keyData, seq, out KeyType key, out KeyDataType keyValue);

                    switch (trackKey)
                    {
                        case (int)TrackEnum.TLAttitudeEvent:
                            HandleTLAttitudeEvent(keyData, keyValue as AttitudeKeyData);
                            break;
                        case (int)TrackEnum.TLCameraLookAt:
                            HandleTLCameraLookAt(keyData, keyValue as CameraLookAtKeyData);
                            break;
                        case (int)TrackEnum.TLEmotionEvent:
                            HandleTLEmotionEvent(keyData, keyValue as EmotionKeyData);
                            break;
                        case (int)TrackEnum.TLLookAtEvent:
                            HandleTLLookAtEvent(keyData, keyValue as LookAtKeyData);
                            break;
                        case (int)TrackEnum.TLShapeshift:
                            HandleTLShapeshift(keyData, keyValue as ShapeshiftKeyData);
                            break;
                    }
                    BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
                }
                BindChannelAndComponent(newChannel, curComponent, curContainer, seq, trackId);
            }
        }

        private void HandleGlobalExclusiveImmutableComponent<TrackType, ComponentType, KeyType, KeyDataType>(XElement componentData, Sequence seq, int trackKey, string channelName)
            where TrackType : ComponentTrackBase, new()
            where ComponentType : ComponentBase, new()
            where KeyType : ImmutableKeyBase<KeyDataType>, new()
            where KeyDataType : new()
        {
            ComponentType curComponent =
                GetExclusiveGlobalComponent<TrackType, ComponentType>(componentData, seq, trackKey, out Guid trackId, out ComponentContainer curContainer);

            Channel newChannel = new() { Name = channelName };
            foreach (XElement keyData in GetKeyDataCollection(componentData))
            {
                GetImmutableKey(keyData, seq, out KeyType key, out KeyDataType keyValue);

                switch (trackKey)
                {
                    case (int)TrackEnum.TLSwitchLocation:
                        HandleTLSwitchLocation(keyData, keyValue as SwitchLocationKeyData);
                        TryAddRootLocation(key as SwitchLocationKey, keyValue as SwitchLocationKeyData, seq);
                        break;
                    case (int)TrackEnum.TLSwitchStage:
                        HandleTLSwitchStage(keyData, keyValue as SwitchStageKeyData);
                        break;
                }

                BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
            }
            BindChannelAndComponent(newChannel, curComponent, curContainer, seq, trackId);
        }

        // TimelineActorPropertiesReflection

        private static void HandleTimelineActorPropertiesReflection(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentActorPropertiesReflection curComponent =
                    GetExclusiveComponent<ComponentTrackActorPropertiesReflection, ComponentActorPropertiesReflection>((Guid)actorId, componentData, seq, (int)TrackEnum.TimelineActorPropertiesReflection, out Guid trackId, out ComponentContainer curContainer);

                curComponent.IsInfinite = false;

                string channelPrefix = "Properties.";
                IEnumerable<XElement> propertyParameters = componentData.XPathSelectElements("./children/node[@id='ChannelContainer']");
                foreach (XElement paramData in propertyParameters)
                {
                    string parameterName = ExtractString(paramData.XPathSelectElement("./attribute[@id='PropertyParameter']")) ?? string.Empty;
                    int parameterDimensions = ExtractInt(paramData.XPathSelectElement("./attribute[@id='Dimensions']")) ?? 1;
                    bool parameterIsBoolean = ComponentTrackActorPropertiesReflection.booleanParameters.Contains(parameterName);
                    TryAddPropertiesSubTrack((Guid) actorId, parameterName, parameterDimensions, parameterIsBoolean);

                    if (parameterIsBoolean)
                    {
                        GetActorPropertiesBoolData(paramData, seq, curComponent, channelPrefix + parameterName);
                    }
                    else
                    {
                        GetActorPropertiesFloatData(paramData, seq, curComponent, channelPrefix + parameterName, parameterDimensions);
                    }
                }
                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        private static void GetActorPropertiesBoolData(XElement paramData, Sequence seq, ComponentActorPropertiesReflection curComponent, string channelName)
        {
            XElement keyChannel = paramData.XPathSelectElement("./children/node[@id='PropertyParameterChannel']");
            GetBooleanFloatKeys(keyChannel, curComponent, "Value", seq, channelName, true);
        }

        private static void GetActorPropertiesFloatData(XElement paramData, Sequence seq, ComponentActorPropertiesReflection curComponent, string channelName, int parameterDimensions)
        {
            for (int i = 1; i <= parameterDimensions; i++)
            {
                string fullChannelName = channelName;
                if (parameterDimensions > 1)
                {
                    switch (i)
                    {
                        case 1:
                            fullChannelName += "." + ComponentActorPropertiesReflection.parameterDimXName;
                            break;
                        case 2:
                            fullChannelName += "." + ComponentActorPropertiesReflection.parameterDimYName;
                            break;
                        case 3:
                            fullChannelName += "." + ComponentActorPropertiesReflection.parameterDimZName;
                            break;
                        case 4:
                            fullChannelName += "." + ComponentActorPropertiesReflection.parameterDimWName;
                            break;
                    }
                }
                XElement keyChannel = paramData.XPathSelectElement("./children/node[@id='PropertyParameterChannel'][" + i + "]");
                GetFloatKeys(keyChannel, curComponent, "Value", seq, fullChannelName, 0);
            }
        }

        private static void TryAddPropertiesSubTrack(Guid actorId, string parameterName, int parameterDimensions, bool isBool)
        {
            try
            {
                ActorTrackBase actorTrack = trackMapping[actorId];
                ComponentTrackActorPropertiesReflection mainTrack = actorTrack.actorChildTracks[(int)TrackEnum.TimelineActorPropertiesReflection][0] as ComponentTrackActorPropertiesReflection;
                if (!mainTrack.subTrackTypes.Contains(parameterName))
                {
                    if (isBool)
                    {
                        KeyTrackBoolean newTrack = new() { Name = parameterName };
                        mainTrack.Tracks.Add(newTrack);
                    }
                    else if (parameterDimensions == 1)
                    {
                        KeyTrackFloat newTrack = new() { Name = parameterName };
                        mainTrack.Tracks.Add(newTrack);
                    }
                    else
                    {
                        KeyTrackWrapper newTrack = new() { Name = parameterName };
                        for (int i = 0; i < parameterDimensions; i++)
                        {
                            KeyTrackFloat subTrack = new();
                            switch (i)
                            {
                                case 0:
                                    subTrack.Name = ComponentActorPropertiesReflection.parameterDimXName;
                                    break;
                                case 1:
                                    subTrack.Name = ComponentActorPropertiesReflection.parameterDimYName;
                                    break;
                                case 2:
                                    subTrack.Name = ComponentActorPropertiesReflection.parameterDimZName;
                                    break;
                                case 3:
                                    subTrack.Name = ComponentActorPropertiesReflection.parameterDimWName;
                                    break;
                            }
                            newTrack.Tracks.Add(subTrack);
                        }
                        mainTrack.Tracks.Add(newTrack);
                    }
                    mainTrack.subTrackTypes.Add(parameterName);
                }
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        // TLAnimation, TLAdditiveAnimation, TLLayeredAnimation

        private void HandleTLAnimation(XElement componentData, Sequence seq, object animType)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentAnimation curComponent = separateOverlappingAnimations ? 
                    GetSeparatedAnimationComponent((Guid)actorId, componentData, seq, out Guid trackId, out ComponentContainer curContainer) : 
                    GetAnimationComponent((Guid)actorId, componentData, seq, out trackId, out curContainer);
                
                curComponent.IsInfinite = false;
                curComponent.AnimationSourceId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='AnimationSourceId']")) ?? Guid.Empty;
                curComponent.PlayRate = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='AnimationPlayRate']")) ?? curComponent.PlayRate;
                curComponent.PlayStartOffset = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='AnimationPlayStartOffset']")) ?? curComponent.PlayStartOffset;
                curComponent.IsContinuous = ExtractBool(componentData.XPathSelectElement("./attribute[@id='Continuous']")) ?? curComponent.IsContinuous;
                curComponent.Hold = ExtractBool(componentData.XPathSelectElement("./attribute[@id='HoldAnimation']")) ?? curComponent.Hold;
                curComponent.IsMirrored = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsMirrored']")) ?? curComponent.IsMirrored;
                curComponent.FadeIn = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeIn']")) ?? curComponent.FadeIn;
                curComponent.FadeOut = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeOut']")) ?? curComponent.FadeOut;
                curComponent.FadeInOffset = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeInOffset']")) ?? curComponent.FadeInOffset;
                curComponent.FadeOutOffset = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeOutOffset']")) ?? curComponent.FadeOutOffset;
                curComponent.EnableRootMotion = ExtractBool(componentData.XPathSelectElement("./attribute[@id='EnableRootMotion']")) ?? curComponent.EnableRootMotion;
                curComponent.BoneGroupId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='BoneGroupId']")) ?? curComponent.BoneGroupId;
                curComponent.AnimationType = Enum.GetName(typeof(AnimationTypeEnum), animType);
                
                curComponent.Transform = GetAnimationTransform(componentData);
                
                int? offsetType = ExtractInt(componentData.XPathSelectElement("./attribute[@id='OffsetType']"));
                if (offsetType.HasValue) curComponent.OffsetType = Enum.GetName(typeof(AnimationOffsetType), offsetType);

                GetAnimationHideVfxKeys(componentData, curComponent, seq.lsfStartTime);

                curContainer.Component.Add(curComponent);
                seq.componentDict[trackId] = curContainer;
            }
        }

        private static string GetAnimationTransform(XElement componentData)
        {
            XElement transformData = componentData.XPathSelectElement("./children/node[@id='TargetTransform']");
            string res = string.Empty;
            if (transformData != null)
            {
                float scale = ExtractFloat(transformData.XPathSelectElement("./attribute[@id='Scale']")) ?? 1f;
                Vector3 position = ExtractVector3(transformData.XPathSelectElement("./attribute[@id='Position']")) ?? new Vector3();
                Quat rotation = ExtractQuat(transformData.XPathSelectElement("./attribute[@id='RotationQuat']")) ?? new Quat();
                res = "{{" + rotation.ToAnimationString() + "}{" + position.ToAnimationString() + "}{" + scale + "}}";
            }
            return res;
        }

        private static void GetAnimationHideVfxKeys(XElement componentData, ComponentAnimation animationComponent, float lsfStartTime)
        {
            XElement hideVfxChannel = componentData.XPathSelectElement("./children/node[@id='HideVfxChannel']");
            GetBooleanKeys(hideVfxChannel, animationComponent, "Value", lsfStartTime, ComponentAnimation.hideVfxPath, false);
        }

        // TLAtmosphereAndLighting

        private void HandleTLAtmosphereAndLighting(XElement componentData, Sequence seq)
        {
            ComponentAtmosphereAndLighting curComponent =
                GetExclusiveGlobalComponent<ComponentTrackAtmosphereAndLighting, ComponentAtmosphereAndLighting>(componentData, seq, (int) TrackEnum.TLAtmosphereAndLighting, out Guid trackId, out ComponentContainer curContainer);
        
            for (int i = 1; i < 3; i++)
            {
                string channelName = i == 1 ? ComponentAtmosphereAndLighting.atmospherePath : ComponentAtmosphereAndLighting.lightingPath;

                XElement channel = componentData.XPathSelectElement("./children/node[@id='Channels']/children/node[@id='Channel'][" + i + "]");
                if (channel != null)
                {
                    Channel newChannel = new() { Name = channelName };
                    foreach (XElement keyData in GetKeyDataCollection(channel))
                    {
                        GetImmutableKey<AtmosphereKey, AtmosphereKeyData>(keyData, seq, out AtmosphereKey key, out AtmosphereKeyData keyValue);
                        keyValue.Id = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='id']")) ?? keyValue.Id;
                        keyValue.FadeTime = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='fadeTime']")) ?? keyValue.FadeTime;
                        BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
                    }
                    if (newChannel.Key.Count > 0) curComponent.Channel.Add(newChannel);
                }
            }
            BindComponent(curComponent, curContainer, seq, trackId);
        }

        // TLAttitudeEvent

        private static void HandleTLAttitudeEvent(XElement keyData, AttitudeKeyData keyValue)
        {
            keyValue.Pose = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='Pose']")) ?? keyValue.Pose;
            keyValue.Transition = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='Transition']")) ?? keyValue.Transition;
        }

        // TLCameraDoF

        private static void HandleTLCameraDoF(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentCameraDoF curComponent =
                    GetExclusiveComponent<ComponentTrackDoF, ComponentCameraDoF>((Guid)actorId, componentData, seq, (int)TrackEnum.TLCameraDoF, out Guid trackId, out ComponentContainer curContainer);
                
                // 1-indexed for XPath
                for (int i = 1; i < 8; i++)
                {
                    string channelName = null;
                    float fallback = 0f;
                    bool boolFallback = false;
                    switch (i)
                    {
                        case 1:
                            channelName = ComponentCameraDoF.doFFocalDistancePath;
                            fallback = ComponentTrackDoF.fallbackFocalDistance;
                            break;
                        case 2:
                            channelName = ComponentCameraDoF.doFAperturePath;
                            fallback = ComponentTrackDoF.fallbackAperture;
                            break;
                        case 3:
                            channelName = ComponentCameraDoF.doFEnabledPath;
                            boolFallback = ComponentTrackDoF.fallbackEnabled;
                            break;
                        case 4:
                            channelName = ComponentCameraDoF.doFAutoFocusEnabledPath;
                            boolFallback = ComponentTrackDoF.fallbackAutoFocus;
                            break;
                        case 5:
                            channelName = ComponentCameraDoF.doFNearSharpnessPath;
                            break;
                        case 6:
                            channelName = ComponentCameraDoF.doFFarSharpnessPath;
                            break;
                        case 7:
                            channelName = ComponentCameraDoF.doFAreaPath;
                            break;
                    }

                    XElement channel = componentData.XPathSelectElement("./children/node[@id='Channels']/children/node[@id='Channel'][" + i + "]");
                    if (channel != null)
                    {
                        if (i <= 2 || i == 5 || i == 6)
                            GetFloatKeys(channel, curComponent, "Value", seq, channelName, fallback);
                        else if (i == 3 || i == 4)
                            GetBooleanFloatKeys(channel, curComponent, "Value", seq, channelName, boolFallback);
                        else
                            GetEnumFloatKeys(channel, curComponent, "Value", seq, channelName, ComponentTrackDoF.fallbackDoFArea, typeof(DOFArea));
                    }
                }
                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        // TLCameraExposure

        private void HandleTLCameraExposure(XElement componentData, Sequence seq)
        {
            ComponentCameraExposure curComponent =
                GetAmbiguousExclsuiveComponent<ComponentTrackCameraExposure, ComponentCameraExposure>(componentData, seq, (int) TrackEnum.TLCameraExposure, out Guid trackId, out ComponentContainer curContainer);
            curComponent.IsContinuous = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsContinuous']")) ?? curComponent.IsContinuous;
            
            for (int i = 1; i < 5; i++)
            {
                string channelName = null;
                switch (i)
                {
                    case 1:
                        channelName = ComponentCameraExposure.exposureEnabledPath;
                        break;
                    case 2:
                        channelName = ComponentCameraExposure.exposureDeltaCompensationPath;
                        break;
                    case 3:
                        channelName = ComponentCameraExposure.exposureDeltaMinPath;
                        break;
                    case 4:
                        channelName = ComponentCameraExposure.exposureDeltaMaxPath;
                        break;
                }
                XElement channel = componentData.XPathSelectElement("./children/node[@id='Channels']/children/node[@id='Channel'][" + i + "]");
                if (channel != null)
                {
                    if (i == 1)
                        GetBooleanFloatKeys(channel, curComponent, "Value", seq, channelName, false);
                    else
                        GetFloatKeys(channel, curComponent, "Value", seq, channelName, 0f);
                }
            }
            BindComponent(curComponent, curContainer, seq, trackId);
        }

        // TLCameraFoV

        private static void HandleTLCameraFoV(XElement keyData, FloatKey key)
        {
            key.Value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='FoV']")) ?? ComponentCameraFoV.defaultFov;
        }

        // TLCameraLookAt

        private static void HandleTLCameraLookAt(XElement keyData, CameraLookAtKeyData keyValue)
        {
            GetImmutableTargetKeyTarget(keyData, keyValue);
            keyValue.TargetBone = ExtractString(keyData.XPathSelectElement("./attribute[@id='Bone']")) ?? keyValue.TargetBone;
            keyValue.Framing = SafeExtractVector2(keyData, "Framing") ?? keyValue.Framing;
            keyValue.FramingOffset = SafeExtractVector2(keyData, "FramingOffset") ?? keyValue.FramingOffset;
            keyValue.FreeBounds = SafeExtractVector2(keyData, "FreeBounds") ?? keyValue.FreeBounds;
            keyValue.SoftBounds = SafeExtractVector2(keyData, "SoftBounds") ?? keyValue.SoftBounds;
            keyValue.DampingStrength = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='DampingStrength']")) ?? keyValue.DampingStrength;
            keyValue.FreeZoneDelay = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='FreeZoneDelay']")) ?? keyValue.FreeZoneDelay;
            keyValue.SoftZoneDelay = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='SoftZoneDelay']")) ?? keyValue.SoftZoneDelay;
            keyValue.SoftZoneRampTime = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='SoftZoneRampTime']")) ?? keyValue.SoftZoneRampTime;
        }

        // TLEffectPhaseEvent

        private static void HandleTLEffectPhase(XElement keyData, EffectPhaseKey key)
        {
            key.Value = ExtractInt(keyData.XPathSelectElement("./attribute[@id='EffectPhase']")) ?? ComponentEffectPhase.phaseFallback;
        }

        // TLEmotion

        private static void HandleTLEmotionEvent(XElement keyData, EmotionKeyData keyValue)
        {
            int? emotionType = ExtractInt(keyData.XPathSelectElement("./attribute[@id='Emotion']"));
            if (emotionType.HasValue)
            {
                keyValue.Emotion = Enum.GetName(typeof(EmotionType), (int)emotionType);
            }
            keyValue.Variation = ExtractInt(keyData.XPathSelectElement("./attribute[@id='Variation']")) ?? keyValue.Variation;
            keyValue.AppliesMaterials = ExtractBool(keyData.XPathSelectElement("./attribute[@id='AppliesMaterials']")) ?? keyValue.AppliesMaterials;
            keyValue.IsSustainedEmotion = ExtractBool(keyData.XPathSelectElement("./attribute[@id='IsSustainedEmotion']")) ?? keyValue.IsSustainedEmotion;
        }

        // TLGenomeTextEvent

        private static void HandleTLGenomeTextEvent(XElement keyData, StringKey key)
        {
            key.Value = ExtractString(keyData.XPathSelectElement("./attribute[@id='EventName']")) ?? string.Empty;
        }

        // TLHandsIK, TLPhysics

        private static void HandleTLHandsIKOrTLPhysics(XElement keyData, BooleanKey key)
        {
            key.Value = ExtractBool(keyData.XPathSelectElement("./attribute[@id='InverseKinematics']")) ?? true;
        }

        // TLLookAtEvent

        private static void HandleTLLookAtEvent(XElement keyData, LookAtKeyData keyValue)
        {
            GetImmutableTargetKeyTarget(keyData, keyValue);
            keyValue.TargetBone = ExtractString(keyData.XPathSelectElement("./attribute[@id='Bone']")) ?? keyValue.TargetBone;
            keyValue.TrackingMode = GetNameFromEnum<LookAtTrackingMode>(keyData, "TrackingMode") ?? keyValue.TrackingMode;
            keyValue.LookAtMode = GetNameFromEnum<LookAtLookAtMode>(keyData, "LookAtMode") ?? keyValue.LookAtMode;
            keyValue.LookAtInterpMode = GetNameFromEnum<LookAtLookAtInterpMode>(keyData, "LookAtInterpMode") ?? keyValue.LookAtInterpMode;
            keyValue.TurnMode = GetNameFromEnum<LookAtTurnMode>(keyData, "TurnMode") ?? keyValue.TurnMode;
            keyValue.TurnSpeedMultiplier = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='TurnSpeedMultiplier']")) ?? keyValue.TurnSpeedMultiplier;
            keyValue.TorsoTurnSpeedMultiplier = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='TorsoTurnSpeedMultiplier']")) ?? keyValue.TorsoTurnSpeedMultiplier;
            keyValue.HeadTurnSpeedMultiplier = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='HeadTurnSpeedMultiplier']")) ?? keyValue.HeadTurnSpeedMultiplier;
            keyValue.Weight = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='Weight']")) ?? keyValue.Weight;
            keyValue.SafeZoneAngle = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='SafeZoneAngle']")) ?? keyValue.SafeZoneAngle;
            keyValue.HeadSafeZoneAngle = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='HeadSafeZoneAngle']")) ?? keyValue.HeadSafeZoneAngle;
            Vector3 offset = ExtractVector3(keyData.XPathSelectElement("./attribute[@id='Offset']"));
            if (offset != null) keyValue.Offset = offset.ToString();
            keyValue.IsEyeOverrideEnabled = ExtractBool(keyData.XPathSelectElement("./attribute[@id='IsEyeLookAtEnabled']")) ?? keyValue.IsEyeOverrideEnabled;
            Guid? eyeTarget = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='EyeLookAtTargetId']"));
            if (eyeTarget.HasValue) keyValue.EyeOverrideTargetId = actorTrackMapping[(Guid)eyeTarget];
            keyValue.EyeOverrideTargetBone = ExtractString(keyData.XPathSelectElement("./attribute[@id='EyeLookAtBone']")) ?? keyValue.EyeOverrideTargetBone;
            Vector3 eyeOffset = ExtractVector3(keyData.XPathSelectElement("./attribute[@id='EyeLookAtOffset']"));
            if (eyeOffset != null) keyValue.EyeOverrideOffset = eyeOffset.ToString();
        }

        // TLMaterial

        private void HandleTLMaterial(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentMaterial curComponent =
                    GetMaterialComponent((Guid) actorId, componentData, seq, out ComponentTrackMaterial materialTrack, out Guid trackId, out ComponentContainer curContainer);

                curComponent.IsInfinite = false;
                curComponent.IsContinuous = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsContinuous']")) ?? curComponent.IsContinuous;

                XElement visibilityChannel = componentData.XPathSelectElement("./children/node[@id='VisibilityChannel']");
                string channelPrefix;
                if (materialTrack.IsOverlay) channelPrefix = "Overlay Material.";
                else if (materialTrack.Slot > -1) channelPrefix = "Slot Material.";
                else channelPrefix = "Material.";
                if (visibilityChannel != null) GetBooleanKeys(visibilityChannel, curComponent, "Value", seq.lsfStartTime, channelPrefix + ComponentMaterial.visibilityName, true);

                IEnumerable<XElement> materialParameters = componentData.XPathSelectElements("./children/node[@id='MaterialParameter']/children/node[@id='Node']");
                foreach (XElement paramData in materialParameters)
                {
                    string parameterName = ExtractString(paramData.XPathSelectElement("./attribute[@id='MaterialParameterName']")) ?? string.Empty;
                    bool isTexture = ExtractBool(paramData.XPathSelectElement("./attribute[@id='TextureParam']")) ?? false;
                    bool isVirtual = ExtractBool(paramData.XPathSelectElement("./attribute[@id='IsVirtual']")) ?? false;
                    int parameterDimensions = ExtractInt(paramData.XPathSelectElement("./attribute[@id='Dimensions']")) ?? 1;
                    TryAddMaterialSubTrack(materialTrack, parameterName, parameterDimensions, isTexture, isVirtual);

                    if (isTexture)
                    {
                        GetMaterialParameterTextureData(paramData, seq, curComponent, channelPrefix + parameterName);
                    }
                    else
                    {
                        GetMaterialParameterData(paramData, seq, curComponent, channelPrefix + parameterName, parameterDimensions);
                    }
                }
                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        private static void GetMaterialParameterData(XElement paramData, Sequence seq, ComponentMaterial curComponent, string channelName, int parameterDimensions)
        {
            for (int i = 1; i <= parameterDimensions; i++)
            {
                string fullChannelName = channelName;
                if (parameterDimensions > 1)
                {
                    switch (i)
                    {
                        case 1:
                            fullChannelName += "." + ComponentMaterial.parameterDimXName;
                            break;
                        case 2:
                            fullChannelName += "." + ComponentMaterial.parameterDimYName;
                            break;
                        case 3:
                            fullChannelName += "." + ComponentMaterial.parameterDimZName;
                            break;
                        case 4:
                            fullChannelName += "." + ComponentMaterial.parameterDimWName;
                            break;
                    }
                }
                XElement keyChannel = paramData.XPathSelectElement("./children/node[@id='MaterialParameter'][" + i + "]");
                GetFloatKeys(keyChannel, curComponent, "Value", seq, fullChannelName, 0);
            }
        }

        private static void GetMaterialParameterTextureData(XElement paramData, Sequence seq, ComponentMaterial curComponent, string channelName)
        {
            Channel newChannel = new() { Name = channelName };
            XElement keyChannel = paramData.XPathSelectElement("./children/node[@id='TextureChannel']");
            foreach (XElement keyData in GetKeyDataCollection(keyChannel))
            {
                GetImmutableKey(keyData, seq, out TextureResourceKey key, out TextureResourceKeyData keyValue);
                keyValue.TextureResourceId = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='Value']")) ?? Guid.Empty;
                BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
            }
            curComponent.Channel.Add(newChannel);
        }

        private static void TryAddMaterialSubTrack(ComponentTrackMaterial materialTrack, string parameterName, int parameterDimensions, bool isTexture, bool isVirtual)
        {
            if (!materialTrack.subTrackTypes.Contains(parameterName))
            {
                if (isTexture)
                {
                    KeyTrackTextureResource newTrack = new() { Name = parameterName, IsVirtual = isVirtual };
                    materialTrack.Tracks.Add(newTrack);
                }
                else if (parameterDimensions == 1)
                {
                    KeyTrackFloat newTrack = new() { Name = parameterName };
                    materialTrack.Tracks.Add(newTrack);
                }
                else
                {
                    KeyTrackWrapper newTrack = new() { Name = parameterName };
                    for (int i = 0; i < parameterDimensions; i++)
                    {
                        KeyTrackFloat subTrack = new();
                        switch (i)
                        {
                            case 0:
                                subTrack.Name = ComponentMaterial.parameterDimXName;
                                break;
                            case 1:
                                subTrack.Name = ComponentMaterial.parameterDimYName;
                                break;
                            case 2:
                                subTrack.Name = ComponentMaterial.parameterDimZName;
                                break;
                            case 3:
                                subTrack.Name = ComponentMaterial.parameterDimWName;
                                break;
                        }
                        newTrack.Tracks.Add(subTrack);
                    }
                    materialTrack.Tracks.Add(newTrack);
                }
                materialTrack.subTrackTypes.Add(parameterName);
            }
        }

        private Guid FindCharacterVisualId(Guid actorId, Guid materialId, Guid resourceId)
        {
            Dictionary<string, Guid> candidates = [];
            foreach (XDocument visualDoc in PreparationHelper.visualFiles)
            {
                // RealMaterialOverrides
                IEnumerable<XElement> overrideParents = visualDoc.XPathSelectElements("save/region[@id='CharacterVisualBank']/node[@id='CharacterVisualBank']/children/node[@id='Resource'][children[node[@id='RealMaterialOverrides'][children[node[@id='Object'][attribute[@id='MapValue'][@value='" + materialId + "']]]]]]");
                GetVisualIdCandidates(overrideParents, candidates);
                if (candidates.Count == 1) return candidates.Values.First();

                // VisualResource
                IEnumerable<XElement> resourceParents = visualDoc.XPathSelectElements("save/region[@id='CharacterVisualBank']/node[@id='CharacterVisualBank']/children/node[@id='Resource'][children[node[@id='Slots'][attribute[@id='VisualResource'][@value='" + resourceId + "']]]]");
                GetVisualIdCandidates(resourceParents, candidates);
                if (candidates.Count == 1) return candidates.Values.First();

                // BodySetVisual
                IEnumerable<XElement> setVisualResources = visualDoc.XPathSelectElements("save/region[@id='CharacterVisualBank']/node[@id='CharacterVisualBank']/children/node[@id='Resource'][attribute[@id='BodySetVisual'][@value='" + resourceId + "']]");
                GetVisualIdCandidates(setVisualResources, candidates);
                if (candidates.Count == 1) return candidates.Values.First();
            }
            if (candidates.Count > 1)
            {
                Guid actorAttempt = TryGetCharacterVisualIdWithActorId(actorId, candidates);
                return actorAttempt == Guid.Empty ? (Guid)sender.Invoke(MainForm.materialSelectionDelegate, candidates) : actorAttempt;
            }
            return Guid.Empty;
        }

        private Guid TryGetCharacterVisualIdWithActorId(Guid actorId, Dictionary<string, Guid> candidates)
        {
            foreach ((string name, Guid id) in candidates)
            {
                string guidSub = name[(name.LastIndexOf('_') + 1)..];
                if (Guid.TryParse(guidSub, out Guid result))
                    if (result == actorId) return id;
            }
            return Guid.Empty;
        }

        private static void GetVisualIdCandidates(IEnumerable<XElement> elements, Dictionary<string, Guid> candidates)
        {
            foreach (XElement ele in elements)
            {
                string name = ExtractString(ele.XPathSelectElement("./attribute[@id='Name']")) ?? string.Empty;
                Guid id = ExtractGuid(ele.XPathSelectElement("./attribute[@id='ID']")) ?? Guid.Empty;
                candidates.TryAdd(name, id);
            }
        }

        // TLPlayEffectEvent

        private static void HandleTLPlayEffect(XElement keyData, BooleanKey key)
        {
            key.Value = ExtractBool(keyData.XPathSelectElement("./attribute[@id='PlayEffect']")) ?? true;
        }

        // TLPlayRate

        private static void HandleTLPlayRate(XElement keyData, FloatKey key)
        {
            key.Value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='Speed']")) ?? ComponentPlayRate.defaultPlayRate;
        }

        // TLShapeShift

        private static void HandleTLShapeshift(XElement keyData, ShapeshiftKeyData keyValue)
        {
            keyValue.TemplateId = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='TemplateId']")) ?? keyValue.TemplateId;
        }

        // TLShot

        private void HandleTLShot(XElement componentData, Sequence seq)
        {
            Guid trackId;
            if (timeline.Tracks[0].Type != "ComponentTrackCameraShot")
                timeline.Tracks.Insert(0, new ComponentTrackCameraShot());
            trackId = timeline.Tracks[0].TrackId;

            ComponentContainer curContainer = GetContainer(seq, trackId);
            ComponentCameraShot curComponent = new ComponentCameraShot();
            ComponentInit(componentData, seq, curComponent);

            curComponent.IsInfinite = false;
            curComponent.IsAutomatedLighting = ExtractBool(componentData.XPathSelectElement("./attribute[@id='AutomatedLighting']")) ?? curComponent.IsAutomatedLighting;
            curComponent.IsAutomatedCamera = ExtractBool(componentData.XPathSelectElement("./attribute[@id='AutomatedCamera']")) ?? curComponent.IsAutomatedCamera;
            curComponent.IsConditionalStagingDisabled = ExtractBool(componentData.XPathSelectElement("./attribute[@id='DisableConditionalStaging']")) ?? curComponent.IsConditionalStagingDisabled;
            curComponent.IsLogicEnabled = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsLogicEnabled']")) ?? curComponent.IsLogicEnabled;
            curComponent.SwitchInterval = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='SwitchInterval']")) ?? curComponent.SwitchInterval;
            curComponent.IsLooping = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsLooping']")) ?? curComponent.IsLooping;
            curComponent.IsJCutEnabled = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsJCutEnabled']")) ?? curComponent.IsJCutEnabled;
            curComponent.JCutLength = ExtractFloat(componentData.XPathSelectElement("./attribute[@id='JCutLength']")) ?? curComponent.JCutLength;
            curComponent.CompanionCameraA = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='CompanionCameraA']")) ?? Guid.Empty;
            curComponent.CompanionCameraB = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='CompanionCameraB']")) ?? Guid.Empty;
            curComponent.CompanionCameraC = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='CompanionCameraC']")) ?? Guid.Empty;
            curComponent.CameraContainer.AddRange(GetCameras(componentData));

            curContainer.Component.Add(curComponent);
            seq.componentDict[trackId] = curContainer;
        }

        private static List<Camera> GetCameras(XElement componentData)
        {
            List<Camera> res = [];
            IEnumerable<XElement> cameraData = componentData.XPathSelectElements("./children/node[@id='CameraContainer']");
            foreach (XElement data in cameraData)
            {
                Guid? actorId = ExtractGuid(data.XPathSelectElement("./attribute[@id='Object']"));
                if (actorId.HasValue && (Guid) actorId != Guid.Empty)
                {
                    Guid trackId = actorTrackMapping[(Guid)actorId];
                    res.Add(new Camera { CameraId = trackId });
                }
                else if ((Guid) actorId == Guid.Empty)
                {
                    res.Add(new Camera { CameraId = Guid.Empty });
                }
            }
            return res;
        }

        // TLShowArmor

        private static void HandleTLShowArmor(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentShowArmor curComponent =
                    GetExclusiveComponent<ComponentTrackShowArmor, ComponentShowArmor>((Guid)actorId, componentData, seq, (int)TrackEnum.TLShowArmor, out Guid trackId, out ComponentContainer curContainer);

                string[] clothingSlots = Enum.GetNames(typeof(CharacterSlot));
                for (int i = 0; i < clothingSlots.Length; i++)
                {
                    XElement keyChannel = componentData.XPathSelectElement("./children/node[@id='Channels']/children/node[" + (i + 1) + "]");
                    
                    if (keyChannel != null)
                    {
                        GetBooleanKeys(keyChannel, curComponent, "Value", seq.lsfStartTime, ComponentShowArmor.channelPrefix + clothingSlots[i], true);
                    }
                }
                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        // TLShowPeanuts

        private static void HandleTLShowPeanuts(XElement keyData, BooleanKey key)
        {
            key.Value = ExtractBool(keyData.XPathSelectElement("./attribute[@id='ShowPeanuts']")) ?? true;
        }

        // TLShowVisual

        private static void HandleTLShowVisual(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentShowActor curComponent =
                    GetExclusiveComponent<ComponentTrackShowActor, ComponentShowActor>((Guid)actorId, componentData, seq, (int)TrackEnum.TLShowVisual, out Guid trackId, out ComponentContainer curContainer);

                GetBooleanKeys(componentData, curComponent, "ShowVisual", seq.lsfStartTime, ComponentShowActor.channelName, true);

                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        // TLShowWeapon

        private static void HandleTLShowWeapon(XElement keyData, BooleanKey key)
        {
            key.Value = ExtractBool(keyData.XPathSelectElement("./attribute[@id='ShowWeapon']")) ?? true;
        }

        // TLSoundEvent

        private void HandleTLSoundEvent(XElement componentData, Sequence seq)
        {
            Guid actorId = GetComponentActor(componentData) ?? Guid.Empty;
            ComponentSoundEvent curComponent =
                GetSoundEventComponent(actorId, componentData, seq, out Guid trackId, out ComponentContainer curContainer);

            Channel newChannel = new() { Name = ComponentSoundEvent.channelName };
            foreach (XElement keyData in GetKeyDataCollection(componentData))
            {
                GetImmutableKey(keyData, seq, out SoundEventKey key, out SoundEventKeyData keyValue);

                int? soundType = ExtractInt(keyData.XPathSelectElement("./attribute[@id='SoundType']"));
                if (soundType.HasValue) keyValue.Type = Enum.GetName(typeof(Models.Track.Component.SoundType), (int) soundType);
                keyValue.Event = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='SoundEventID']")) ?? keyValue.Event;
                keyValue.KeepAlive = ExtractBool(keyData.XPathSelectElement("./attribute[@id='KeepAlive']")) ?? keyValue.KeepAlive;
                keyValue.LoopLifetime = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='LoopLifetime']")) ?? keyValue.LoopLifetime;
                int? objectIndex = ExtractInt(keyData.XPathSelectElement("./attribute[@id='SoundObjectIndex']"));
                if (objectIndex.HasValue) keyValue.ObjectIndex = Enum.GetName(typeof(SoundObjectIndex), (int)objectIndex);
                int? foleyType = ExtractInt(keyData.XPathSelectElement("./attribute[@id='FoleyType']"));
                if (foleyType.HasValue) keyValue.FoleyType = Enum.GetName(typeof(FoleyTypeEnum), (int)foleyType);
                int? foleyIntensity = ExtractInt(keyData.XPathSelectElement("./attribute[@id='FoleyIntensity']"));
                if (foleyIntensity.HasValue) keyValue.FoleyIntensity = Enum.GetName(typeof(FoleyIntensityEnum), (int)foleyIntensity);
                int? vocalType = ExtractInt(keyData.XPathSelectElement("./attribute[@id='VocalType']"));
                if (vocalType.HasValue) keyValue.VocalType = Enum.GetName(typeof(VocalTypeEnum), (int)vocalType);

                BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
            }
            BindChannelAndComponent(newChannel, curComponent, curContainer, seq, trackId);
        }

        // TLSplatter

        private static void HandleTLSplatter(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentSplatter curComponent =
                    GetExclusiveComponent<ComponentTrackSplatter, ComponentSplatter>((Guid)actorId, componentData, seq, (int)TrackEnum.TLSplatter, out Guid trackId, out ComponentContainer curContainer);
            
                for (int i = 1; i < 5; i++)
                {
                    string channelName = null;
                    switch (i)
                    {
                        case 1:
                            channelName = ComponentSplatter.bloodPath;
                            break;
                        case 2:
                            channelName = ComponentSplatter.dirtPath;
                            break;
                        case 3:
                            channelName = ComponentSplatter.bruisePath;
                            break;
                        case 4:
                            channelName = ComponentSplatter.sweatPath;
                            break;
                    }
                    XElement channel = componentData.XPathSelectElement("./children/node[@id='Channels']/children/node[@id='Channel'][" + i + "]");
                    if (channel != null)
                    {
                        Channel newChannel = new() { Name = channelName };
                        foreach (XElement keyData in GetKeyDataCollection(channel))
                        {
                            GetImmutableKey(keyData, seq, out SplatterKey key, out SplatterKeyData keyValue);
                            int type = ExtractInt(channel.XPathSelectElement("./attribute[@id='SplatterType']")) ?? 0;
                            keyValue.SplatterType = Enum.GetName(typeof(SplatterTypeEnum), type);
                            int changeMode = ExtractInt(keyData.XPathSelectElement("./attribute[@id='SplatterChangeMode']")) ?? 0;
                            keyValue.SplatterChangeMode = Enum.GetName(typeof(SplatterChangeModeEnum), changeMode);
                            keyValue.Value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='Value']")) ?? keyValue.Value;

                            BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
                        }
                        if (newChannel.Key.Count > 0) curComponent.Channel.Add(newChannel);
                    }
                }
                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        // TLSprings

        private static void HandleTLSprings(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentSprings curComponent =
                    GetSpringsComponent((Guid) actorId, componentData, seq, out Guid trackId, out ComponentContainer curContainer, out string trackName);

                GetBooleanKeys(componentData, curComponent, "EnableSprings", seq.lsfStartTime, trackName, true);

                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        // TLSwitchLocationEvent

        private static void HandleTLSwitchLocation(XElement keyData, SwitchLocationKeyData keyValue)
        {
            keyValue.Event = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='SwitchLocationEventID']")) ?? keyValue.Event;
            keyValue.LevelTemplateId = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='s_LevelTemplateID']")) ?? keyValue.LevelTemplateId;
        }

        private void TryAddRootLocation(SwitchLocationKey key, SwitchLocationKeyData keyValue, Sequence seq)
        {
            if (dbRootNodes.Contains(seq.DialogNodeReference.First().DialogNodeId) && key.Time == 0)
            {
                rootLocations.Add(keyValue.Event);
            }
        }

        private void TrySetTimelineLocation()
        {
            int locations = rootLocations.Count;
            if (locations == 1)
            {
                timeline.TimelinePosition.BoundSceneId = rootLocations[0];
            }
            else if (locations > 1)
            {
                if (!rootLocations.Any(o => o != rootLocations[0]))
                {
                    timeline.TimelinePosition.BoundSceneId = rootLocations[0];
                }
                else
                {
                    Guid selected = (Guid)sender.Invoke(MainForm.locationSelectionDelegate, rootLocations);
                    timeline.TimelinePosition.BoundSceneId = selected;
                }
            }
        }

        // TLSwitchStageEvent

        private static void HandleTLSwitchStage(XElement keyData, SwitchStageKeyData keyValue)
        {
            keyValue.Event = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='SwitchStageEventID']")) ?? keyValue.Event;
            keyValue.ForceTransformUpdate = ExtractBool(keyData.XPathSelectElement("./attribute[@id='ForceTransformUpdate']")) ?? keyValue.ForceTransformUpdate;
            keyValue.ForceUpdateCameraBehavior = ExtractBool(keyData.XPathSelectElement("./attribute[@id='ForceUpdateCameraBehavior']")) ?? keyValue.ForceUpdateCameraBehavior;
        }

        // TLTransform

        private static void HandleTLTransform(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentTransform curComponent = 
                    GetExclusiveComponent<ComponentTrackTransform, ComponentTransform>((Guid)actorId, componentData, seq, (int)TrackEnum.TLTransform, out Guid trackId, out ComponentContainer curContainer);

                curComponent.IsContinuous = ExtractBool(componentData.XPathSelectElement("./attribute[@id='Continuous']")) ?? curComponent.IsContinuous;
                curComponent.IsInfinite = false;

                GetTransformFloatKeys(componentData, 1, curComponent, seq.lsfStartTime);
                GetTransformFloatKeys(componentData, 2, curComponent, seq.lsfStartTime);
                GetTransformFloatKeys(componentData, 3, curComponent, seq.lsfStartTime);
                GetTransformRotationKeys(componentData, curComponent, seq.lsfStartTime);
                GetTransformFloatKeys(componentData, 5, curComponent, seq.lsfStartTime);
                GetTransformParentKeys(componentData, curComponent, seq.lsfStartTime);

                BindComponent(curComponent, curContainer, seq, trackId);
            }
        }

        private static void GetTransformFloatKeys(XElement componentData, int channelNumber, ComponentTransform transformComponent, float lsfStartTime)
        {
            XElement transformChannel = componentData.XPathSelectElement("./children/node[@id='TransformChannels']/children/node[@id='TransformChannel'][" + channelNumber + "]");
            if (transformChannel != null)
            {
                string name = null;
                switch (channelNumber)
                {
                    case 1:
                        name = ComponentTransform.translateXPath;
                        break;
                    case 2:
                        name = ComponentTransform.translateYPath;
                        break;
                    case 3:
                        name = ComponentTransform.translateZPath;
                        break;
                    case 5:
                        name = ComponentTransform.scalePath;
                        break;
                }
                Channel newChannel = new() { Name = name };
                foreach (XElement keyData in GetKeyDataCollection(transformChannel))
                {
                    FloatKey key = new();
                    GetKeyTimeAndInterpolation(keyData, key, lsfStartTime);
                    key.Value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='Value']")) ?? key.Value;
                    newChannel.Key.Add(key);
                }
                transformComponent.Channel.Add(newChannel);
            }
        }

        private static void GetTransformRotationKeys(XElement componentData, ComponentTransform transformComponent, float lsfStartTime)
        {
            XElement transformChannel = componentData.XPathSelectElement("./children/node[@id='TransformChannels']/children/node[@id='TransformChannel'][4]");
            if (transformChannel != null)
            {
                Channel newChannel = new() { Name = ComponentTransform.rotatePath };
                IEnumerable<XElement> transformKeys = GetKeyDataCollection(transformChannel);
                foreach (XElement keyData in transformKeys)
                {
                    StringKey key = new();
                    GetKeyTimeAndInterpolation(keyData, key, lsfStartTime);
                    Quat rotationQuat = ExtractQuat(keyData.XPathSelectElement("./attribute[@id='Value']"));

                    string eulerString = Quat.ToEulerAngles(rotationQuat).ToString();
                    key.Value = eulerString;

                    newChannel.Key.Add(key);
                }
                transformComponent.Channel.Add(newChannel);
            }
        }

        private static void GetTransformParentKeys(XElement componentData, ComponentTransform transformComponent, float lsfStartTime)
        {
            XElement transformChannel = componentData.XPathSelectElement("./children/node[@id='TransformChannels']/children/node[@id='TransformChannel'][6]");
            if (transformChannel != null)
            {
                Channel newChannel = new() { Name = ComponentTransform.frameOfReferencePath };
                IEnumerable<XElement> transformKeys = GetKeyDataCollection(transformChannel);
                foreach (XElement keyData in transformKeys)
                {
                    FrameOfReferenceKey key = new();
                    FrameOfReferenceKeyData keyValue = new();
                    GetKeyTimeAndInterpolation(keyData, key, lsfStartTime);
                    XElement subData = keyData.XPathSelectElement("./children/node[@id='Value']/children/node[@id='frameOfReference']");

                    Guid targetId = ExtractGuid(subData.XPathSelectElement("./attribute[@id='targetId']")) ?? Guid.Empty;
                    trackMapping.TryGetValue(targetId, out ActorTrackBase actorTrack);
                    keyValue.TargetId = actorTrack != null ? actorTrack.TrackId : targetId;

                    keyValue.TargetBone = ExtractString(subData.XPathSelectElement("./attribute[@id='targetBone']")) ?? keyValue.TargetBone;
                    keyValue.KeepScale = ExtractBool(subData.XPathSelectElement("./attribute[@id='KeepScale']")) ?? keyValue.KeepScale;
                    keyValue.OneFrameOnly = ExtractBool(subData.XPathSelectElement("./attribute[@id='OneFrameOnly']")) ?? keyValue.OneFrameOnly;

                    BindKeyDataToKeyAndChannel(key, keyValue, newChannel);
                }
                transformComponent.Channel.Add(newChannel);
            }
        }

        // TLVoice

        private static void HandleTLVoice(XElement componentData, Sequence seq)
        {
            Guid? actorId = GetComponentActor(componentData);
            if (actorId.HasValue)
            {
                ComponentVoice curComponent =
                    GetExclusiveComponent<ComponentTrackVoice, ComponentVoice>((Guid)actorId, componentData, seq, (int)TrackEnum.TLVoice, out Guid trackId, out ComponentContainer curContainer);

                curComponent.IsInfinite = false;
                int? faceType = ExtractInt(componentData.XPathSelectElement("./attribute[@id='FaceType']"));
                if (faceType.HasValue)
                    curComponent.FaceTech = Enum.GetName(typeof(FaceTech), (int) faceType);
                curComponent.IsAliased = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsAliased']")) ?? curComponent.IsAliased;
                curComponent.LeftBuffer = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='LeftBuffer']")) ?? curComponent.LeftBuffer;
                curComponent.HoldMocap = ExtractBool(componentData.XPathSelectElement("./attribute[@id='HoldMocap']")) ?? curComponent.HoldMocap;
                curComponent.DisableMocap = ExtractBool(componentData.XPathSelectElement("./attribute[@id='DisableMocap']")) ?? curComponent.DisableMocap;
                curComponent.PerformanceFade = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='PerformanceFade']")) ?? curComponent.PerformanceFade;
                curComponent.FadeIn = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeIn']")) ?? curComponent.FadeIn;
                curComponent.FadeOut = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeOut']")) ?? curComponent.FadeOut;
                curComponent.FadeInOffset = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeInOffset']")) ?? curComponent.FadeInOffset;
                curComponent.FadeOutOffset = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='FadeOutOffset']")) ?? curComponent.FadeOutOffset;
                int? performanceDriftType = ExtractInt(componentData.XPathSelectElement("./attribute[@id='PerformanceDriftType']"));
                if (performanceDriftType.HasValue)
                    curComponent.PerformanceDrift = Enum.GetName(typeof(PerformanceDriftType), (int) performanceDriftType);
                curComponent.HeadPitchCorrectionDegrees = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='HeadPitchCorrection']")) ?? curComponent.HeadPitchCorrectionDegrees;
                curComponent.HeadYawCorrectionDegrees = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='HeadYawCorrection']")) ?? curComponent.HeadYawCorrectionDegrees;
                curComponent.HeadRollCorrectionDegrees = ExtractDouble(componentData.XPathSelectElement("./attribute[@id='HeadRollCorrection']")) ?? curComponent.HeadRollCorrectionDegrees;
                curComponent.IsMirrored = ExtractBool(componentData.XPathSelectElement("./attribute[@id='IsMirrored']")) ?? curComponent.IsMirrored;

                SequenceDialogReferenceIds refIds = new SequenceDialogReferenceIds();
                refIds.DialogNodeId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='DialogNodeId']")) ?? refIds.DialogNodeId;
                refIds.ReferenceId = ExtractGuid(componentData.XPathSelectElement("./attribute[@id='ReferenceId']")) ?? refIds.ReferenceId;
                curComponent.DialogNodeReference = refIds;

                CheckDialogRefPair(refIds, seq);

                curContainer.Component.Add(curComponent);
                seq.componentDict[trackId] = curContainer;
            }
        }

        private static void CheckDialogRefPair(SequenceDialogReferenceIds refIds, Sequence seq)
        {
            foreach (SequenceDialogReferenceIds idPair in seq.DialogNodeReference)
            {
                if (idPair == refIds)
                {
                    if (idPair.ReferenceId != refIds.ReferenceId)
                        idPair.ReferenceId = refIds.ReferenceId;
                    return;
                }
            }
            seq.DialogNodeReference.Add(refIds);
        }

        // Key Helpers

        private static string SafeExtractVector2(XElement keyData, string attribute)
        {
            Vector2 res = ExtractVector2(keyData.XPathSelectElement("./attribute[@id='" + attribute + "']"));
            return res?.ToString();
        }

        private static void GetImmutableTargetKeyTarget(XElement keyData, ImmutableTargetKeyDataBase keyValue)
        {
            Guid? target = ExtractGuid(keyData.XPathSelectElement("./attribute[@id='Target']"));
            if (target.HasValue) keyValue.TargetId = actorTrackMapping[(Guid)target];
        }

        private static void BindComponent(ComponentBase curComponent, ComponentContainer curContainer, Sequence seq, Guid trackId)
        {
            curContainer.Component.Add(curComponent);
            seq.componentDict[trackId] = curContainer;
        }

        /// <summary>
        /// Used with immutable keys. Binds the channel to the component and the component to the sequence.
        /// </summary>
        /// <param name="newChannel"></param>
        /// <param name="curComponent"></param>
        /// <param name="curContainer"></param>
        /// <param name="seq"></param>
        /// <param name="trackId"></param>
        private static void BindChannelAndComponent(Channel newChannel, ComponentBase curComponent, ComponentContainer curContainer, Sequence seq, Guid trackId)
        {
            if (newChannel.Key.Count > 0) curComponent.Channel.Add(newChannel);
            BindComponent(curComponent, curContainer, seq, trackId);
        }

        /// <summary>
        /// Used with immutable keys. Binds the key data to the key and the key to the channel.
        /// </summary>
        /// <typeparam name="KeyDataType"></typeparam>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        /// <param name="newChannel"></param>
        private static void BindKeyDataToKeyAndChannel<KeyDataType>(ImmutableKeyBase<KeyDataType> key, KeyDataType keyValue, Channel newChannel)
        {
            key.Value = keyValue;
            newChannel.Key.Add(key);
        }

        /// <summary>
        /// Used with mutable keys. Binds the key to the channel.
        /// </summary>
        private static void BindKeyToChannel<KeyDataType>(MutableKeyBase<KeyDataType> key, Channel newChannel)
        {
            newChannel.Key.Add(key);
        }

        private static KeyType GetMutableKey<KeyType, KeyDataType>(XElement keyData, Sequence seq)
            where KeyType : MutableKeyBase<KeyDataType>, new()
        {
            KeyType key = new();
            GetKeyTimeAndInterpolation(keyData, key, seq.lsfStartTime);
            return key;
        }

        private static void GetImmutableKey<KeyType, KeyDataType>(XElement keyData, Sequence seq, out KeyType key, out KeyDataType keyValue)
            where KeyType : KeyBase, new()
            where KeyDataType : new()
        {
            key = new();
            keyValue = new();
            GetKeyTimeAndInterpolation(keyData, key, seq.lsfStartTime);
        }

        private static void GetKeyTimeAndInterpolation(XElement keyData, KeyBase key, float lsfStartTime)
        {
            key.Time = GetKeyTime(keyData, key, lsfStartTime);
            int? interpNumber = ExtractInt(keyData.XPathSelectElement("./attribute[@id='InterpolationType']"));
            if (interpNumber.HasValue)
            {
                key.Interpolation = Enum.GetName(typeof(KeyInterpolation), (int)interpNumber);
            }
        }

        private static void GetEnumFloatKeys(XElement keyCollection, ComponentBase curComponent, string attributeName, Sequence seq, string channelName, string fallback, Type enumType)
        {
            if (keyCollection != null)
            {
                Channel newChannel = new() { Name = channelName };
                foreach (XElement keyData in GetKeyDataCollection(keyCollection))
                {
                    StringKey key = GetMutableKey<StringKey, string>(keyData, seq);
                    GetEnumFloatKeyData(keyData, key, attributeName, fallback, enumType);
                    newChannel.Key.Add(key);
                }
                curComponent.Channel.Add(newChannel);
            }
        }

        private static void GetEnumFloatKeyData(XElement keyData, StringKey key, string attribute, string fallback, Type enumType)
        {
            float? value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='" + attribute + "']"));
            if (value.HasValue)
                key.Value = Enum.GetName(enumType, (int) value);
            else
                key.Value = fallback;
        }

        private static void GetFloatKeys(XElement keyCollection, ComponentBase curComponent, string attributeName, Sequence seq, string channelName, float fallback)
        {
            if (keyCollection != null)
            {
                Channel newChannel = new() { Name = channelName };
                foreach (XElement keyData in GetKeyDataCollection(keyCollection))
                {
                    FloatKey key = GetMutableKey<FloatKey, float>(keyData, seq);
                    GetFloatKeyData(keyData, key, attributeName, fallback);
                    newChannel.Key.Add(key);
                }
                curComponent.Channel.Add(newChannel);
            }
        }

        private static void GetFloatKeyData(XElement keyData, FloatKey key, string attribute, float fallback)
        {
            key.Value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='" + attribute + "']")) ?? fallback;
        }

        private static void GetBooleanFloatKeys(XElement keyCollection, ComponentBase curComponent, string attributeName, Sequence seq, string channelName, bool fallback)
        {
            if (keyCollection != null)
            {
                Channel newChannel = new() { Name = channelName };
                foreach (XElement keyData in GetKeyDataCollection(keyCollection))
                {
                    BooleanKey key = GetMutableKey<BooleanKey, bool>(keyData, seq);
                    GetBooleanFloatKeyData(keyData, key, attributeName, fallback);
                    newChannel.Key.Add(key);
                }
                curComponent.Channel.Add(newChannel);
            }
        }

        private static void GetBooleanFloatKeyData(XElement keyData, BooleanKey key, string attribute, bool fallback)
        {
            float? value = ExtractFloat(keyData.XPathSelectElement("./attribute[@id='" + attribute + "']"));
            if (value.HasValue)
                key.Value = value == 1;
            else
                key.Value = fallback;
        }

        private static void GetBooleanKeys(XElement keyCollection, ComponentBase curComponent, string attributeName, float lsfStartTime, string channelName, bool fallback)
        {
            if (keyCollection != null)
            {
                Channel newChannel = new() { Name = channelName };
                foreach (XElement keyData in GetKeyDataCollection(keyCollection))
                {
                    BooleanKey key = new();
                    GetBooleanKeyData(keyData, key, lsfStartTime, attributeName, fallback);
                    newChannel.Key.Add(key);
                }
                curComponent.Channel.Add(newChannel);
            }
        }

        private static void GetBooleanKeyData(XElement keyData, BooleanKey key, float lsfStartTime, string attribute, bool fallback)
        {
            key.Time = GetKeyTime(keyData, key, lsfStartTime);
            key.Value = ExtractBool(keyData.XPathSelectElement("./attribute[@id='" + attribute + "']")) ?? fallback;
        }

        private static float GetKeyTime(XElement keyData, KeyBase key, float lsfStartTime)
        {
            return ExtractFloat(keyData.XPathSelectElement("./attribute[@id='Time']")) - lsfStartTime ?? key.Time;
        }

        private static IEnumerable<XElement> GetKeyDataCollection(XElement channel)
        {
            return channel.XPathSelectElements("./children/node[@id='Keys']/children/node[@id='Key']");
        }
    }
}
