using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Models.Core;
using TMLGen.Models.Global;
using TMLGen.Models.Track;
using TMLGen.Models.Track.Actor;
using TMLGen.Models.Track.Component;

namespace TMLGen.Generation
{
    public class ActorCollector : CollectorBase
    {
        private readonly Dictionary<Guid, int> peanutMapping;
        private readonly Dictionary<Guid, Guid> speakerTrackMapping;
        private readonly PeanutFolderTrack peanutContainer;
        private readonly FolderTrack speakerContainer;
        private readonly FolderTrack cameraContainer;
        private readonly FolderTrack effectContainer;
        private readonly FolderTrack extraActorsContainer;
        private readonly SceneLightsFolderTrack lightsContainer;
        private readonly XElement dbSpeakerList;
        
        private static readonly Guid narratorSpeakerMappingId = Guid.Parse("a346318f-15b3-49ad-ab97-ddf8283dc339");
        private static readonly Guid narratorActorId = Guid.Parse("bbb9c649-e86d-43a1-b171-d0a8006e8b5e");
        private static readonly int narratorSpeakerId = -666;

        private int initiatorCount;
        private int additionalCount;
        private string templatePath;
        private bool didTemplatesCopy;
        private bool doCopy;
        private string sourceName;
        private Guid timelineId;
        private string modName;
        private string gameDataPath;

        public ActorCollector(string dataDirectory, string sourceName, string templateDirectory, string gameDataPath, string modName, bool doCopy, XDocument doc, XDocument gdtDoc, XDocument dbDoc, Timeline timeline) : base(doc, gdtDoc, timeline)
        {
            if (templateDirectory == null)
            {
                Guid timelineId = (Guid)ExtractGuid(gdtDoc.XPathSelectElement("save/region[@id='TimelineBank']/node[@id='TimelineBank']/children/node[@id='Resource']/attribute[@id='ID']"));
                this.timelineId = timelineId;
                templatePath = PreparationHelper.FindTemplatesFolder(dataDirectory, this.timelineId);
                templatePath ??= string.Empty;
            }
            else
            {
                templatePath = templateDirectory;
            }
            this.sourceName = sourceName;
            this.doCopy = doCopy;
            this.modName = modName;
            this.gameDataPath = gameDataPath;
            peanutMapping = [];
            speakerTrackMapping = [];
            peanutContainer = new PeanutFolderTrack();
            speakerContainer = new FolderTrack { Name = "Speakers" };
            cameraContainer = new FolderTrack { Name = "Cameras" };
            effectContainer = new FolderTrack { Name = "Effects" };
            extraActorsContainer = new FolderTrack { Name = "Extra Actors" };
            lightsContainer = new SceneLightsFolderTrack();
            dbSpeakerList = dbDoc.XPathSelectElement("save/region[@id='dialog']/node[@id='dialog']/children/node[@id='speakerlist']/children");
        }

        public override void Collect()
        {
            CollectPeanutMapping();
            CollectActorData();
        }

        private void CollectPeanutMapping()
        {
            IEnumerable<XElement> peanutMappingElements = doc.XPathSelectElements("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='PeanutSlotIdMap']/children/node/children//node");
            foreach (XElement element in peanutMappingElements)
            {
                XAttribute idAtt = element.XPathSelectElement("./attribute[@id='MapKey']").Attribute("value");
                XAttribute slotAtt = element.XPathSelectElement("./attribute[@id='MapValue']").Attribute("value");
                if (slotAtt != null && idAtt != null)
                {
                    peanutMapping.Add(Guid.Parse(idAtt.Value), int.Parse(slotAtt.Value));
                }
            }
        }

        private void CollectActorData()
        {
            IEnumerable<XElement> actorDataElements = doc.XPathSelectElements("/save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelineActorData']/children/node/children/node[@id='Object']");
            foreach(XElement element in actorDataElements)
            {
                Guid? idAtt = ExtractGuid(element.XPathSelectElement("./attribute[@id='MapKey']"));
                XElement data = element.XPathSelectElement("./children/node");
                int? speakerSlot = ExtractInt(data.XPathSelectElement("./attribute[@id='Speaker']"));
                string actorType = ExtractString(data.XPathSelectElement("./attribute[@id='ActorTypeId']"));

                if (speakerSlot.HasValue)
                {
                    HandleSpeaker(data, (int) speakerSlot, actorType);
                }
                else if (actorType == "peanut")
                {
                    HandlePeanut(data, (Guid) idAtt);
                }
                else if (actorType == "scenecam")
                {
                    HandleScenecam(data, (Guid) idAtt);
                }
                else if (actorType == "scenelight")
                {
                    HandleScenelight(data, (Guid) idAtt);
                }
                else if (actorType == "effect")
                {
                    HandleEffect(data, (Guid) idAtt);
                }
                else
                {
                    if (actorType != "timeline") HandleOther(data, (Guid)idAtt, actorType);
                }
            }

            timeline.Tracks.Add(speakerContainer);
            timeline.Tracks.Add(peanutContainer);
            timeline.Tracks.Add(cameraContainer);
            timeline.Tracks.Add(effectContainer);
            timeline.Tracks.Add(extraActorsContainer);
            timeline.Tracks.Add(lightsContainer);
        }

        private void HandleSpeaker(XElement data, int speakerSlot, string ActorType)
        {
            ActorTrackSpeaker res = new();
            XElement dbSpeakerData = dbSpeakerList.XPathSelectElement("./node/attribute[@id='index' and @value='" + speakerSlot + "']/..");

            if (speakerSlot == narratorSpeakerId || dbSpeakerData != null)
            {
                if (speakerSlot != narratorSpeakerId)
                {
                    res.IsPeanut = ExtractBool(dbSpeakerData.XPathSelectElement("./attribute[@id='IsPeanutSpeaker']")) ?? res.IsPeanut;
                    res.SceneActorType = Enum.GetName(typeof(SceneActorType), int.Parse(data.XPathSelectElement("./attribute[@id='SceneActorType']").Attribute("value").Value));
                    res.SpeakerMappingId = ExtractGuid(dbSpeakerData.XPathSelectElement("./attribute[@id='SpeakerMappingId']")) ?? res.SpeakerMappingId;
                    string actorIdList = ExtractString(dbSpeakerData.XPathSelectElement("./attribute[@id='list']"));
                    if (actorIdList != null)
                    {
                        int delimiterIndex = actorIdList.IndexOf(';');
                        res.ActorId = delimiterIndex == -1 ? Guid.Parse(actorIdList) :  Guid.Parse(actorIdList.Substring(0, delimiterIndex));
                    }
                    else
                    {
                        res.ActorId = Guid.Empty;
                    }

                    if (res.SceneActorType == "Initiator") res.Name = "Initiator " + (1 + initiatorCount++);
                    else res.Name = "Additional " + (1 + additionalCount++);
                }
                else
                {
                    res.Name = "Narrator";
                    res.SpeakerMappingId = narratorSpeakerMappingId;
                    res.ActorId = narratorActorId;
                }

                res.SpeakerId = speakerSlot;
                res.IsPlayer = ExtractBool(data.XPathSelectElement("./attribute[@id='IsPlayer']")) ?? res.IsPlayer;
                res.IsImportantForStaging = !ExtractBool(data.XPathSelectElement("./attribute[@id='ShouldIgnoreForStaging']")) ?? res.IsImportantForStaging;
                res.SceneActorIndex = ExtractInt(data.XPathSelectElement("./attribute[@id='SceneActorIndex']")) ?? res.SceneActorIndex;
                res.AlwaysInclude = ExtractBool(data.XPathSelectElement("./attribute[@id='AlwaysInclude']")) ?? res.AlwaysInclude;
                res.ActorType = ActorType;
                SetAttenuation(data, res);
                CheckAutomatedLookAt(data, res);

                speakerTrackMapping.Add(res.SpeakerMappingId, res.TrackId);
                actorTrackMapping.Add(res.SpeakerMappingId, res.TrackId);

                trackMapping.Add(res.SpeakerMappingId, res);
                speakerContainer.Tracks.Add(res);
            }
        }

        private void HandlePeanut(XElement data, Guid peanutId)
        {
            int slot = peanutMapping[peanutId];
            PeanutTrack res = new()
            {
                ActorType = "peanut",
                ActorId = peanutId,
                Name = "Peanut " + (slot + 1),
                PeanutSlotIndex = slot
            };
            CheckAutomatedLookAt(data, res);

            actorTrackMapping.Add(peanutId, peanutId);
            trackMapping.Add(res.ActorId, res);
            peanutContainer.Tracks.Add(res);
        }

        private void HandleScenecam(XElement data, Guid cameraId)
        {
            ActorTrackSceneCamera res = new()
            {
                ActorId = cameraId,
                ActorType = "scenecam"
            };
            res.SceneCameraId = ExtractGuid(data.XPathSelectElement("./attribute[@id='Camera']")) ?? res.SceneCameraId;

            Guid? lookAtActor = ExtractGuid(data.XPathSelectElement("./attribute[@id='LookAt']"));
            Guid? attachToActor = ExtractGuid(data.XPathSelectElement("./attribute[@id='AttachTo']"));
            try
            {
                if (lookAtActor.HasValue) res.SceneCameraLookAt = speakerTrackMapping[(Guid) lookAtActor];
                if (attachToActor.HasValue) res.SceneCameraAttachTo = speakerTrackMapping[(Guid) attachToActor];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Could not find camera target");
            }

            actorTrackMapping.Add(cameraId, res.TrackId);
            trackMapping.Add(cameraId, res);
            cameraContainer.Tracks.Add(res);
        }

        private void HandleScenelight(XElement data, Guid actorId)
        {
            ActorTrackSceneLight res = new()
            {
                ActorId = actorId,
                ActorType = "scenelight"
            };
            res.SceneLightId = ExtractGuid(data.XPathSelectElement("./attribute[@id='Light']")) ?? res.SceneLightId;

            actorTrackMapping.Add(actorId, res.TrackId);
            trackMapping.Add(actorId, res);
            lightsContainer.Tracks.Add(res);
        }

        private void HandleEffect(XElement data, Guid effectId)
        {
            ActorTrackDefault res = new()
            {
                ActorId = effectId,
                Name = "Effect " + (effectContainer.Tracks.Count() + 1),
                ActorType = "effect"
            };
            res.ResourceId = ExtractGuid(data.XPathSelectElement("./attribute[@id='ResourceId']")) ?? res.ResourceId;
            res.IsResource = res.ResourceId != Guid.Empty;
            res.AlwaysInclude = ExtractBool(data.XPathSelectElement("./attribute[@id='AlwaysInclude']")) ?? res.AlwaysInclude;
            SetExtraActorTransform(data.XPathSelectElement("./children/node"), res);

            actorTrackMapping.Add(effectId, res.TrackId);
            trackMapping.Add(effectId, res);
            effectContainer.Tracks.Add(res);
        }

        private void HandleOther(XElement data, Guid actorId, string actorType)
        {
            ActorTrackDefault res = new()
            {
                ActorId = actorId,
                ActorType = actorType
            };
            SetExtraActorTransform(data.XPathSelectElement("./children/node"), res);
            SetAttenuation(data, res);
            CheckAutomatedLookAt(data, res);

            string templateFilePath = Path.Join(templatePath, actorId.ToString());
            templateFilePath = Path.ChangeExtension(templateFilePath, ".lsf");
            res.IsTemplate = File.Exists(templateFilePath);
            res.Name = res.IsTemplate ? "TimelineTemplate_" + res.ActorId : "Extra Actor " + (extraActorsContainer.Tracks.Count + 1);
            res.AlwaysInclude = ExtractBool(data.XPathSelectElement("./attribute[@id='AlwaysInclude']")) ?? res.AlwaysInclude;

            // if not template, check for parent template?
            actorTrackMapping.Add(actorId, res.TrackId);
            trackMapping.Add(actorId, res);
            extraActorsContainer.Tracks.Add(res);

            if (!didTemplatesCopy && res.IsTemplate)
            {
                didTemplatesCopy = true;
                CopyHelper.CopyTemplates(sourceName, templatePath, timelineId, gameDataPath, modName, doCopy);
            }
        }

        private void SetExtraActorTransform(XElement transformData, ActorTrackDefault actor)
        {
            if (transformData != null)
            {
                float? scale = ExtractFloat(transformData.XPathSelectElement("./attribute[@id='Scale']"));
                Vector3 position = ExtractVector3(transformData.XPathSelectElement("./attribute[@id='Position']"));
                Quat rotation = ExtractQuat(transformData.XPathSelectElement("./attribute[@id='RotationQuat']"));

                if (scale != null && position != null && rotation != null) 
                    actor.SetTransform(rotation, (float) scale, position);
            }
        }

        private void SetAttenuation(XElement data, ActorTrackBase actor)
        {
            float? attenuation = ExtractFloat(data.XPathSelectElement("./attribute[@id='SoundAttenuationScale']"));
            if (attenuation.HasValue)
            {
                actor.CustomActorData.AttenuationScale = (float)attenuation;
            }
        }

        private static void CheckAutomatedLookAt(XElement data, ActorTrackBase actor)
        {
            bool? automatedLookAt = ExtractBool(data.XPathSelectElement("./attribute[@id='IsAutomatedLookatEnabled']"));
            if (automatedLookAt.HasValue && (bool) automatedLookAt)
            {
                ComponentTrackLookAtEvent newTrack = new() { IsAutomatedLookatEnabled = true };
                actor.Tracks.Add(newTrack);
                actor.actorChildTracks.Add((int) TrackEnum.TLLookAtEvent, [newTrack]);
            }
        }
    }
}
