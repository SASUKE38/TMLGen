using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Generation.Collectors;
using TMLGen.Generation.Helpers;
using TMLGen.Models;
using TMLGen.Models.Global;

namespace TMLGen.Generation
{
    public static class GenerationDriver
    {
        public static int DoGeneration(Form sender, string sourceName, string dataPath, string sourcePath, string gdtPath, string dbPath, string dPath, string templatePath, string gameDataPath, string rawSourcePath, string modName, bool extraPathsGiven, bool separateAnimations, bool doCopy)
        {
            LoggingHelper.Write("Starting generation...");
            if (!extraPathsGiven)
            {
                dbPath = PreparationHelper.FindDialogsBinaryFile(dataPath, sourceName);
                gdtPath = PreparationHelper.FindGeneratedDialogTimelinesFile(dataPath, sourceName);
                dPath = PreparationHelper.FindDialogsFile(dataPath, sourceName);
            }
            else
            {
                gdtPath = PreparationHelper.SaveToLsxFile(gdtPath);
                gdtPath = PreparationHelper.GetGDTElementFromMerged(gdtPath, Path.GetFileNameWithoutExtension(sourceName));
            }
            dbPath = PreparationHelper.SaveToLsxFile(dbPath);

            if (dbPath == null)
            {
                LoggingHelper.Write("Failed to prepare dialogs binary file.", 2);
                return 1;
            }
            if (sourcePath == null)
            {
                LoggingHelper.Write("Failed to prepare source file.", 2);
                return 1;
            }
            if (gdtPath == null)
            {
                LoggingHelper.Write("Failed to prepare generated dialog timelines file.", 2);
                return 1;
            }

            PreparationHelper.FindCharacterVisualsFiles(dataPath, []);
            string localizationPath = PreparationHelper.FindLocalizationFile(dataPath, "English");

            CopyHelper.CopyTimelineFiles(rawSourcePath, Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);
            CopyHelper.CopyGDTFile(gdtPath, Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);

            Root root = new();
            XmlSerializer serializer = new(typeof(Root));
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add("", "");

            Timeline timeline = new();
            root.Timeline = timeline;

            XDocument sourceDoc = XDocument.Load(sourcePath);
            XDocument gdtDoc = XDocument.Load(gdtPath);
            XDocument dbDoc = XDocument.Load(dbPath);

            TimelineSettingsCollector timelineSettingsCollector = new(
                sourceDoc,
                gdtDoc,
                timeline);
            ActorCollector actorCollector = new(
                dataPath,
                Path.GetFileNameWithoutExtension(sourceName),
                templatePath,
                gameDataPath,
                modName,
                doCopy,
                sourceDoc,
                gdtDoc,
                dbDoc,
                timeline);
            ComponentCollector componentCollector = new(
                sender,
                sourceDoc,
                gdtDoc,
                dbDoc,
                timeline,
                separateAnimations);

            LoggingHelper.Write("Collecting timeline settings and actor data...");
            timelineSettingsCollector.Collect();
            actorCollector.Collect();
            LoggingHelper.Write("Collecting components...");
            componentCollector.Collect();

            LoggingHelper.Write("Serializing result...");
            string outputPath = CopyHelper.GetOutputPath(Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);
            StreamWriter writer = new(outputPath);

            if (localizationPath != null)
            {
                ReferenceCollector referenceCollector = new(dataPath, dPath, outputPath, localizationPath);
                referenceCollector.Collect();
            }

            serializer.Serialize(writer, root, namespaces);
            writer.Close();

            XDocument processed = new(CleanupHelper.DoPostProcess(XDocument.Load(outputPath).XPathSelectElement("Root")));
            processed.Save(outputPath);

            CleanupHelper.DeleteTempFiles([dbPath, sourcePath, gdtPath]);
            return 0;
        }
    }
}
