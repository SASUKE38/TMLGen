using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Models.Global;

namespace TMLGen.Generation
{
    public class TimelineSettingsCollector : CollectorBase
    {
        public TimelineSettingsCollector(XDocument doc, XDocument gdtDoc, Timeline timeline) : base(doc, gdtDoc, timeline)
        {
            
        }

        public override void Collect()
        {
            CollectHeader();
            CollectPosition();
            CollectExposureSettings();
            CollectWorldCinematicSettings();

            if (timeline.OverrideDefaultFoleySettings) CollectFoleySettings();
            if (timeline.QuestionHoldAutomation.OverrideGlobalSettings) CollectQuestionHoldAutomation();

            timeline.EnterSoundEvents.AddRange(CollectSoundEvents("EnterSoundEvents"));
            timeline.ExitSoundEvents.AddRange(CollectSoundEvents("ExitSoundEvents"));
        }

        private void CollectHeader()
        {
            try
            {
                XElement settings = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelineSettings']");
                timeline.DialogResourceId = Guid.Parse(gdtDoc.XPathSelectElement("save/region[@id='TimelineBank']/node[@id='TimelineBank']/children/node[@id='Resource']/attribute[@id='DialogResourceId']").Attribute("value").Value);
                if (settings != null)
                {
                    timeline.CanShowPeanuts = ExtractBool(settings.XPathSelectElement("attribute[@id='CanShowPeanuts']")) ?? timeline.CanShowPeanuts;
                    timeline.CanShareDummyTemplates = ExtractBool(settings.XPathSelectElement("attribute[@id='CanShareDummyTemplates']")) ?? timeline.CanShareDummyTemplates;
                    timeline.BodyPartForMocap = ExtractInt(settings.XPathSelectElement("attribute[@id='BodyPartForMocap']")) ?? timeline.BodyPartForMocap;
                    timeline.OverrideDefaultFoleySettings = ExtractBool(settings.XPathSelectElement("attribute[@id='OverrideDefaultFoleySettings']")) ?? timeline.OverrideDefaultFoleySettings;
                    timeline.QuestionHoldAutomation.OverrideGlobalSettings = ExtractBool(settings.XPathSelectElement("attribute[@id='OverrideGlobalQuestionHoldAutomationSettings']")) ?? timeline.QuestionHoldAutomation.OverrideGlobalSettings;
                    timeline.Offset = ExtractFloat(settings.XPathSelectElement("attribute[@id='MaxStepOffset']")) ?? timeline.Offset;
                }
            }
            catch (NullReferenceException)
            {
                LoggingHelper.Write("Generated dialogs binary file missing dialog resource ID. Is this file correct?", 2);
            }
        }

        private void CollectPosition()
        {
            IEnumerable<string> ids = from node in doc.XPathSelectElements("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='AdditionalLocations']/children//node/attribute")
                        select node.Attribute("value").Value;
            foreach (string id in ids)
            {
                AdditionalBoundSceneId boundToAdd = new()
                {
                    BoundSceneId = Guid.Parse(id)
                };
                timeline.TimelinePosition.AdditionalBoundSceneIds.Add(boundToAdd);
            }
        }

        private void CollectFoleySettings()
        {
            XElement settings = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelineSettings']/children/node[@id='FoleySettings']");
            if (settings != null)
            {
                timeline.FoleySettings.ShouldPlay = ExtractBool(settings.XPathSelectElement("attribute[@id='ShouldPlay']")) ?? timeline.FoleySettings.ShouldPlay;
                timeline.FoleySettings.CameraCullingEnabled = ExtractBool(settings.XPathSelectElement("attribute[@id='CameraCullingEnabled']")) ?? timeline.FoleySettings.CameraCullingEnabled;
                timeline.FoleySettings.MinVelocityThreshold = ExtractFloat(settings.XPathSelectElement("attribute[@id='MinVelocityThreshold']")) ?? timeline.FoleySettings.MinVelocityThreshold;
                timeline.FoleySettings.MaxVelocityThreshold = ExtractFloat(settings.XPathSelectElement("attribute[@id='MaxVelocityThreshold']")) ?? timeline.FoleySettings.MaxVelocityThreshold;
                timeline.FoleySettings.VelocitySpikeThreshold = ExtractFloat(settings.XPathSelectElement("attribute[@id='VelocitySpikeThreshold']")) ?? timeline.FoleySettings.VelocitySpikeThreshold;
                timeline.FoleySettings.MinRotationThreshold = ExtractFloat(settings.XPathSelectElement("attribute[@id='MinRotationThreshold']")) ?? timeline.FoleySettings.MinRotationThreshold;
                timeline.FoleySettings.MaxRotationThreshold = ExtractFloat(settings.XPathSelectElement("attribute[@id='MaxRotationThreshold']")) ?? timeline.FoleySettings.MaxRotationThreshold;
                timeline.FoleySettings.RotationSpikeThreshold = ExtractFloat(settings.XPathSelectElement("attribute[@id='RotationSpikeThreshold']")) ?? timeline.FoleySettings.RotationSpikeThreshold;
            }

            XElement armorElement = settings.XPathSelectElement("./attribute[@id='ArmorType']");
            if (armorElement != null)
            {
                timeline.FoleySettings.ArmorType = Enum.GetName(typeof(ArmorFoleyType), int.Parse(armorElement.Attribute("value").Value));
            }
        }

        private void CollectExposureSettings()
        {
            XElement settings = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelineSettings']/children/node[@id='ExposureSettings']");
            if (settings != null)
            {
                timeline.ExposureSettings.DeltaMin = ExtractFloat(settings.XPathSelectElement("attribute[@id='DeltaMin']")) ?? timeline.ExposureSettings.DeltaMin;
                timeline.ExposureSettings.DeltaMax = ExtractFloat(settings.XPathSelectElement("attribute[@id='DeltaMax']")) ?? timeline.ExposureSettings.DeltaMax;
                timeline.ExposureSettings.DeltaCompensation = ExtractFloat(settings.XPathSelectElement("attribute[@id='DeltaCompensation']")) ?? timeline.ExposureSettings.DeltaCompensation;
            }
        }

        private void CollectQuestionHoldAutomation()
        {
            XElement settings = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelineSettings']/children/node[@id='QuestionHoldAutomation']");
            if (settings != null)
            {
                timeline.QuestionHoldAutomation.IsQuestionHoldAutomationEnabled = ExtractBool(settings.XPathSelectElement("attribute[@id='IsEnabled']")) ?? timeline.QuestionHoldAutomation.IsQuestionHoldAutomationEnabled;
                timeline.QuestionHoldAutomation.QuestionHoldAutomationCycleSpeed = ExtractFloat(settings.XPathSelectElement("attribute[@id='CycleSpeed']")) ?? timeline.QuestionHoldAutomation.QuestionHoldAutomationCycleSpeed;
                timeline.QuestionHoldAutomation.QuestionHoldAutomationCycleSpeedDeviation = ExtractFloat(settings.XPathSelectElement("attribute[@id='CycleSpeedDeviation']")) ?? timeline.QuestionHoldAutomation.QuestionHoldAutomationCycleSpeedDeviation;
                timeline.QuestionHoldAutomation.QuestionHoldAutomationStartOffset = ExtractFloat(settings.XPathSelectElement("attribute[@id='StartOffset']")) ?? timeline.QuestionHoldAutomation.QuestionHoldAutomationStartOffset;
                timeline.QuestionHoldAutomation.QuestionHoldAutomationStartOffsetDeviation = ExtractFloat(settings.XPathSelectElement("attribute[@id='StartOffsetDeviation']")) ?? timeline.QuestionHoldAutomation.QuestionHoldAutomationStartOffsetDeviation;
            }
        }

        private List<GlobalSoundEvent> CollectSoundEvents(string evType)
        {
            IEnumerable <XElement> events = doc.XPathSelectElements("save/region[@id='" + evType + "']/node[@id='" + evType + "']/children//node");
            return CollectGlobalSoundEvents(events);
        }

        private void CollectWorldCinematicSettings()
        {
            XElement settings = doc.XPathSelectElement("save/region[@id='TimelineContent']/node[@id='TimelineContent']/children/node[@id='TimelineSettings']");
            if (settings != null)
            {
                int? playCount = ExtractInt(settings.XPathSelectElement("attribute[@id='PlayCount']"));
                timeline.WorldCinematicSettings.PlayCount = playCount ?? timeline.WorldCinematicSettings.PlayCount;
                timeline.WorldCinematicSettings.IsInfinite = playCount.HasValue && playCount == -1;

                timeline.WorldCinematicSettings.LoopDelay = ExtractFloat(settings.XPathSelectElement("attribute[@id='LoopDelay']")) ?? timeline.WorldCinematicSettings.LoopDelay;
                timeline.WorldCinematicSettings.MinPlayCountBound = ExtractInt(settings.XPathSelectElement("attribute[@id='MinPlayCountBound']")) ?? timeline.WorldCinematicSettings.MinPlayCountBound;
                timeline.WorldCinematicSettings.MaxPlayCountBound = ExtractInt(settings.XPathSelectElement("attribute[@id='MaxPlayCountBound']")) ?? timeline.WorldCinematicSettings.MaxPlayCountBound;
            }
        }
    }
}
