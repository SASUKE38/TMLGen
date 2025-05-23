using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Generation.Collectors;
using TMLGen.Generation.Helpers;
using TMLGen.Models;
using TMLGen.Models.Global;
using TMLGen.Properties;

namespace TMLGen.Generation
{
    public static class GenerationDriver
    {
        public static int DoGeneration(Form sender)
        {
            string dataPath = PathConfigurationSettings.Default.UnpackedDataDirectory;
            string gameDataPath = PathConfigurationSettings.Default.GameDataDirectory;
            bool extraPathsGiven = Settings.Default.Manual;
            bool separateAnimations = Settings.Default.SeparateAnimations;
            bool doCopy = Settings.Default.DoCopy;
            string sourceName = Path.GetFileName(Settings.Default.SourceFile);
            string sourcePath = null;
            string gdtPath = extraPathsGiven ? Settings.Default.GeneratedDialogTimelinesFile : null;
            string dbPath = extraPathsGiven ? Settings.Default.DialogsBinaryFile : null;
            string dPath = extraPathsGiven ? Settings.Default.DialogsFile : null;
            string templatePath = extraPathsGiven ? Settings.Default.TimelineTemplatesDirectory : null;
            string rawSourcePath = Settings.Default.SourceFile;
            string modName = Settings.Default.SelectedMod;

            LoggingHelper.Write(Resources.ProgressStarting);
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
            sourcePath = PreparationHelper.SaveToLsxFile(rawSourcePath);

            if (!CheckFilePreparation(dbPath, sourcePath, gdtPath, sourceName, dPath, false))
                return 1;

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

            LoggingHelper.Write(Resources.ProgressSettingsAndActors);
            timelineSettingsCollector.Collect();
            actorCollector.Collect();
            LoggingHelper.Write(Resources.ProgressComponents);
            componentCollector.Collect();

            LoggingHelper.Write(Resources.ProgressSerializing);
            string outputPath = CopyHelper.GetOutputPath(Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);
            StreamWriter writer = new(outputPath);

            if (localizationPath != null && dPath != null)
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

        public static int DoBatchGeneration(BackgroundWorker worker, DoWorkEventArgs e, Form sender)
        {
            string inputPath = Settings.Default.BatchSourceDirectory;
            string dataPath = PathConfigurationSettings.Default.UnpackedDataDirectory;
            string gameDataPath = PathConfigurationSettings.Default.GameDataDirectory;
            string modName = Settings.Default.SelectedMod;
            bool separateAnimations = Settings.Default.SeparateAnimations;
            bool doCopy = Settings.Default.DoCopy;

            LoggingHelper.Write(Resources.ProgressStartingBatch);

            PreparationHelper.FindCharacterVisualsFiles(dataPath, []);
            string localizationPath = PreparationHelper.FindLocalizationFile(dataPath, "English");

            XmlSerializer serializer = new(typeof(Root));
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add("", "");

            string[] fileCollection = [.. Directory.GetFiles(inputPath, "*.lsf").Where(x => !x.EndsWith("_Scene.lsf") && !x.EndsWith("_Prefetch.lsf"))];

            for (int i = 0; i < fileCollection.Length; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    try
                    {
                        string sourceFile = fileCollection[i];
                        string sourceName = Path.GetFileName(sourceFile);
                        string rawSourceName = Path.GetFileNameWithoutExtension(sourceFile);

                        sender.Invoke(MainForm.currentBatchFileDelegate, sourceName);

                        if (!File.Exists(sourceFile))
                        {
                            LoggingHelper.Write(String.Format(Resources.SourceFileDoesNotExistBatch, sourceName), 2);
                            continue;
                        }

                        string sourceLsx = PreparationHelper.SaveToLsxFile(sourceFile);
                        string dbPath = PreparationHelper.SaveToLsxFile(PreparationHelper.FindDialogsBinaryFile(dataPath, sourceName));
                        string gdtPath = PreparationHelper.FindGeneratedDialogTimelinesFile(dataPath, sourceName);
                        string dPath = PreparationHelper.FindDialogsFile(dataPath, sourceName);

                        if (!CheckFilePreparation(dbPath, sourceFile, gdtPath, sourceName, dPath, true))
                            continue;

                        CopyHelper.CopyTimelineFiles(sourceFile, Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);
                        CopyHelper.CopyGDTFile(gdtPath, Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);

                        Root root = new();
                        Timeline timeline = new();
                        root.Timeline = timeline;

                        XDocument sourceDoc = XDocument.Load(sourceLsx);
                        XDocument gdtDoc = XDocument.Load(gdtPath);
                        XDocument dbDoc = XDocument.Load(dbPath);

                        TimelineSettingsCollector timelineSettingsCollector = new(
                            sourceDoc,
                            gdtDoc,
                            timeline);
                        ActorCollector actorCollector = new(
                            dataPath,
                            Path.GetFileNameWithoutExtension(sourceName),
                            null,
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

                        timelineSettingsCollector.Collect();
                        actorCollector.Collect();
                        componentCollector.Collect();

                        string outputPath = CopyHelper.GetOutputPath(Path.GetFileNameWithoutExtension(sourceName), gameDataPath, modName, doCopy);
                        StreamWriter writer = new(outputPath);

                        if (localizationPath != null && dPath != null)
                        {
                            ReferenceCollector referenceCollector = new(dataPath, dPath, outputPath, localizationPath);
                            referenceCollector.Collect();
                        }

                        serializer.Serialize(writer, root, namespaces);
                        writer.Close();

                        XDocument processed = new(CleanupHelper.DoPostProcess(XDocument.Load(outputPath).XPathSelectElement("Root")));
                        processed.Save(outputPath);

                        CleanupHelper.DeleteTempFiles([dbPath, sourceLsx, gdtPath]);
                        CleanupHelper.EmptyStaticCollections();

                        worker.ReportProgress((int)(((double)i / fileCollection.Length) * 100));
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.Write(String.Format(Resources.GenerationErrorBatch, Path.GetFileName(fileCollection[i])), 3);
                        CleanupHelper.WriteException(ex);
                        CleanupHelper.EmptyStaticCollections();
                        continue;
                    }
                }
            }
            return 0;
        }

        private static bool CheckFilePreparation(string dbPath, string sourcePath, string gdtPath, string sourceName, string dPath, bool isBatch)
        {
            if (dbPath == null)
            {
                LoggingHelper.Write(isBatch ? String.Format(Resources.DBFilePreparationFailureBatch, sourceName) : Resources.DBFilePreparationFailure, 2);
                return false;
            }
            if (sourcePath == null)
            {
                LoggingHelper.Write(isBatch ? String.Format(Resources.SourceFilePreparationFailureBatch, sourceName) : Resources.SourceFilePreparationFailure, 2);
                return false;
            }
            if (gdtPath == null)
            {
                LoggingHelper.Write(isBatch ? String.Format(Resources.GDTFilePreparationFailureBatch, sourceName) : Resources.GDTFilePreparationFailure, 2);
                return false;
            }
            if (dPath == null)
            {
                LoggingHelper.Write(isBatch ? String.Format(Resources.DFilePreparationFailureBatch, sourceName) : Resources.DFilePreparationFailure, 2);
            }

            return true;
        }
    }
}
