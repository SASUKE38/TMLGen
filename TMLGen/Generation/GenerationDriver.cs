using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Models;
using TMLGen.Models.Global;

namespace TMLGen.Generation
{
    public static class GenerationDriver
    {
        public static int DoGeneration(Form sender, string sourceName, string dataPath, string sourcePath, string gdtPath, string dbPath, string templatePath, string outputPath, string rawSourcePath, bool extraPathsGiven, bool separateAnimations, bool doCopy)
        {
            LoggingHelper.Write("Starting generation...");
            if (!extraPathsGiven)
            {
                dbPath = PreparationHelper.FindDialogsBinaryFile(dataPath, sourceName);
                gdtPath = PreparationHelper.FindGeneratedDialogTimelinesFile(dataPath, sourceName);
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
            if (doCopy)
            {
                PreparationHelper.CopyTimelineFiles(rawSourcePath, Path.GetFileNameWithoutExtension(sourceName));
            }
            
            Root root = new();
            XmlSerializer serializer = new(typeof(Root));
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add("", "");

            Timeline t = new();
            root.Timeline = t;

            TimelineSettingsCollector ts = new(
                XDocument.Load(sourcePath),
                XDocument.Load(gdtPath),
                t);

            ActorCollector a = new(
                dataPath,
                Path.GetFileNameWithoutExtension(sourceName),
                templatePath,
                doCopy,
                XDocument.Load(sourcePath),
                XDocument.Load(gdtPath),
                XDocument.Load(dbPath),
                t);
            ComponentCollector c = new(
                sender,
                XDocument.Load(sourcePath),
                XDocument.Load(gdtPath),
                XDocument.Load(dbPath),
                t,
                separateAnimations);
            LoggingHelper.Write("Collecting timeline settings and actor data...");
            ts.Collect();
            a.Collect();
            LoggingHelper.Write("Collecting components...");
            c.Collect();

            LoggingHelper.Write("Serializing result...");
            StreamWriter writer = new(outputPath);
            serializer.Serialize(writer, root, namespaces);
            writer.Close();

            XDocument processed = new(CleanupHelper.DoPostProcess(XDocument.Load(outputPath).XPathSelectElement("Root")));
            processed.Save(outputPath);

            CleanupHelper.DeleteTempFiles([dbPath, sourcePath, gdtPath]);
            return 0;
        }
    }
}
