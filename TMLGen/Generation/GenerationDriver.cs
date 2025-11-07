using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
        /// <summary>
        /// Generates the .tml and _ref.json files  for a given timeline source file specified in <see cref="Settings.Default.SourceFile"/>.
        /// <see cref="Settings.Default.Save"/> must be called before the thread running this method starts.
        /// </summary>
        /// <param name="sender">The <see cref="Form"/> that created the <see cref="BackgroundWorker"/> running this method.</param>
        /// <returns>0 if generation was successful or an <see cref="int"/> greater than 0 if an error occurred.</returns>
        public static int DoGeneration(Form sender)
        {
            string dataPath = PathConfigurationSettings.Default.UnpackedDataDirectory;
            string gameDataPath = PathConfigurationSettings.Default.GameDataDirectory;
            bool extraPathsGiven = Settings.Default.Manual;
            bool separateAnimations = Settings.Default.SeparateAnimations;
            bool doCopy = Settings.Default.DoCopy;
            bool skipShowArmor = Settings.Default.SkipShowArmor;
            string sourceName = Path.GetFileName(Settings.Default.SourceFile);
            string sourceNameExtensionless = Path.GetFileNameWithoutExtension(sourceName);
            string sourcePath = null;
            string gdtPath = extraPathsGiven ? Settings.Default.GeneratedDialogTimelinesFile : null;
            string dbPath = extraPathsGiven ? Settings.Default.DialogsBinaryFile : null;
            string dPath = extraPathsGiven ? Settings.Default.DialogsFile : null;
            string templatePath = extraPathsGiven ? Settings.Default.TimelineTemplatesDirectory : null;
            string rawSourcePath = Settings.Default.SourceFile;
            string modName = Settings.Default.SelectedMod;
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

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
                gdtPath = PreparationHelper.GetGDTElementFromMerged(gdtPath, sourceNameExtensionless);
            }
            dbPath = PreparationHelper.SaveToLsxFile(dbPath);
            sourcePath = PreparationHelper.SaveToLsxFile(rawSourcePath);

            if (!CheckFilePreparation(dbPath, sourcePath, gdtPath, sourceName, dPath, false))
                return 1;

            string localizationPath = PrepareLocalizationAndVisualFiles(dataPath, [], "English");
            ReferenceFlagPaths flagPaths = PreparationHelper.GetReferenceFlagPaths(dataPath);
            CopyTimelineFiles(gdtPath, rawSourcePath, sourceNameExtensionless, gameDataPath, modName, doCopy);
            GetSerializer(out XmlSerializer serializer, out XmlSerializerNamespaces namespaces);

            Generate(
                sender,
                serializer,
                namespaces,
                dataPath,
                gameDataPath,
                templatePath,
                modName,
                localizationPath,
                sourcePath,
                gdtPath,
                dbPath,
                dPath,
                sourceNameExtensionless,
                separateAnimations,
                skipShowArmor,
                doCopy,
                true,
                flagPaths,
                options,
                null);
            return 0;
        }

        /// <summary>
        /// Generates the .tml and _ref.json files for a given batch of timeline source files specified in <see cref="Settings.Default.BatchSourceDirectory"/>.
        /// <see cref="Settings.Default.Save"/> must be called before the thread running this method starts.
        /// </summary>
        /// <param name="worker">The <see cref="BackgroundWorker"/> running this method. Must support cancellation.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> obtained from starting this method in a <see cref="BackgroundWorker"/>.</param>
        /// <param name="sender">The <see cref="Form"/> that created the <see cref="BackgroundWorker"/> running this method.</param>
        /// <returns>0 if generation was successful or an <see cref="int"/> greater than 0 if an error occurred.</returns>
        public static int DoBatchGeneration(BackgroundWorker worker, DoWorkEventArgs e, Form sender)
        {
            string inputPath = Settings.Default.BatchSourceDirectory;
            string dataPath = PathConfigurationSettings.Default.UnpackedDataDirectory;
            string gameDataPath = PathConfigurationSettings.Default.GameDataDirectory;
            string modName = Settings.Default.SelectedMod;
            bool separateAnimations = Settings.Default.SeparateAnimations;
            bool doCopy = Settings.Default.DoCopy;
            bool skipShowArmor = Settings.Default.SkipShowArmor;
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            LoggingHelper.Write(Resources.ProgressStartingBatch);

            string localizationPath = PrepareLocalizationAndVisualFiles(dataPath, [], "English");
            ReferenceFlagPaths flagPaths = PreparationHelper.GetReferenceFlagPaths(dataPath);
            GetSerializer(out XmlSerializer serializer, out XmlSerializerNamespaces namespaces);

            string[] fileCollection = [.. Directory.GetFiles(inputPath, "*.lsf").Where(x => !x.EndsWith("_Scene.lsf") && !x.EndsWith("_Prefetch.lsf"))];
            CancellationTokenSource cts = new();

            try
            {
                object lockObj = new();
                int total = fileCollection.Length;
                int current = 0;

                Parallel.ForEach(fileCollection, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount, CancellationToken = cts.Token }, (file, state) =>
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        cts.Cancel();
                        state.Break();
                    }
                    else
                    {
                        string sourceFile = file;
                        string sourceName = Path.GetFileName(sourceFile);
                        string sourceNameExtensionless = Path.GetFileNameWithoutExtension(sourceFile);

                        if (!File.Exists(sourceFile))
                        {
                            LoggingHelper.Write(String.Format(Resources.SourceFileDoesNotExistBatch, sourceName), 2);
                            return;
                        }

                        string sourceLsx = PreparationHelper.SaveToLsxFile(sourceFile);
                        string dbPath = PreparationHelper.SaveToLsxFile(PreparationHelper.FindDialogsBinaryFile(dataPath, sourceName));
                        string gdtPath = PreparationHelper.FindGeneratedDialogTimelinesFile(dataPath, sourceName);
                        string dPath = PreparationHelper.FindDialogsFile(dataPath, sourceName);

                        if (!CheckFilePreparation(dbPath, sourceFile, gdtPath, sourceName, dPath, true))
                            return;

                        CopyTimelineFiles(gdtPath, sourceFile, sourceNameExtensionless, gameDataPath, modName, doCopy);

                        try
                        {
                            Generate(
                                sender,
                                serializer,
                                namespaces,
                                dataPath,
                                gameDataPath,
                                null,
                                modName,
                                localizationPath,
                                sourceLsx,
                                gdtPath,
                                dbPath,
                                dPath,
                                sourceNameExtensionless,
                                separateAnimations,
                                skipShowArmor,
                                doCopy,
                                false,
                                flagPaths,
                                options,
                                cts.Token);
                        }
                        catch (Exception ex) when (ex is not OperationCanceledException)
                        {
                            LoggingHelper.Write(String.Format(Resources.GenerationErrorBatch, Path.GetFileName(file)), 3);
                            CleanupHelper.WriteException(ex);
                        }

                        lock (lockObj)
                        {
                            if (!cts.IsCancellationRequested)
                            {
                                worker.ReportProgress((int)(((double)current / total) * 100));
                                current++;
                            }
                        }
                    }
                });
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                cts.Dispose();
            }
            return 0;
        }

        /// <summary>
        /// Generates a .tml and a _ref.json file for a given collection of input files.
        /// </summary>
        /// <param name="sender">The <see cref="Form"/> that initialized generation.</param>
        /// <param name="serializer">The <see cref="XmlSerializer"/> to use.</param>
        /// <param name="namespaces">The <see cref="XmlSerializerNamespaces"/> to use. Should have the namespace pair "", "".</param>
        /// <param name="dataPath">The path to the user's unpacked data.</param>
        /// <param name="gameDataPath">The path to the user's game data.</param>
        /// <param name="templatePath">The path to the timeline's template directory. Should be null if none is provided or if in batch generation mode.</param>
        /// <param name="modName">The name of the mod. Should appear as it does in file paths. Used for automatic overriding.</param>
        /// <param name="localizationPath">The path to the stored localization file. Obtained from <see cref="PreparationHelper.FindLocalizationFile"/>.</param>
        /// <param name="sourceLsx">The source file, as a temporary .lsx file. Will be deleted after generation.</param>
        /// <param name="gdtPath">The generated dialog timelines file, as a temporary .lsx file. Will be deleted after generation.</param>
        /// <param name="dbPath">The dialogs binary file, as a temporary .lsx file. Will be deleted after generation.</param>
        /// <param name="dPath">The dialogs file, as a .lsj file. Not temporary.</param>
        /// <param name="sourceNameExtensionless">The name of the source file, as obtained from <see cref="Path.GetFileNameWithoutExtension"/>.</param>
        /// <param name="separateAnimations">Whether or not to separate animation tracks in the generated .tml.</param>
        /// <param name="skipShowArmor">Whether or not to include the TLShowArmor component in the generated .tml.</param>
        /// <param name="doCopy">Whether or not to output the files to the user's mod or to the default Timeline Data directory.</param>
        /// <param name="doProgressLog">Whether or not to log each stage of generation.</param>
        /// <param name="flagPaths">The paths to flag files that should be used to create the _ref.json file.</param>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> to use when serializing the _ref.json file.</param>
        /// <param name="token">The <see cref="CancellationToken"/> for the parallel loop that this function was called from, if applicable.</param>
        private static void Generate(
            Form sender,
            XmlSerializer serializer,
            XmlSerializerNamespaces namespaces,
            string dataPath,
            string gameDataPath,
            string templatePath,
            string modName,
            string localizationPath,
            string sourceLsx,
            string gdtPath,
            string dbPath,
            string dPath,
            string sourceNameExtensionless,
            bool separateAnimations,
            bool skipShowArmor,
            bool doCopy,
            bool doProgressLog,
            ReferenceFlagPaths flagPaths,
            JsonSerializerOptions options,
            CancellationToken? token)
        {
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
            if (doProgressLog) LoggingHelper.Write(Resources.ProgressSettingsAndActors);
            timelineSettingsCollector.Collect();
            ActorCollector actorCollector = new(
                dataPath,
                sourceNameExtensionless,
                templatePath,
                gameDataPath,
                modName,
                doCopy,
                sourceDoc,
                gdtDoc,
                dbDoc,
                timeline);
            actorCollector.Collect();
            ComponentCollector componentCollector = new(
                sender,
                sourceNameExtensionless,
                sourceDoc,
                gdtDoc,
                dbDoc,
                timeline,
                separateAnimations,
                skipShowArmor,
                actorCollector.actorTrackMapping,
                actorCollector.trackMapping,
                token);
            if (doProgressLog) LoggingHelper.Write(Resources.ProgressComponents);
            componentCollector.Collect();

            if (doProgressLog) LoggingHelper.Write(Resources.ProgressSerializing);
            if (token.HasValue && token.Value.IsCancellationRequested) return;

            string outputPath = CopyHelper.GetOutputPath(sourceNameExtensionless, gameDataPath, modName, doCopy);
            StreamWriter writer = new(outputPath);

            if (localizationPath != null && dPath != null)
            {
                ReferenceCollector referenceCollector = new(dPath, outputPath, localizationPath, flagPaths, options);
                referenceCollector.Collect();
            }

            serializer.Serialize(writer, root, namespaces);
            writer.Close();

            XDocument processed = new(CleanupHelper.DoPostProcess(XDocument.Load(outputPath).XPathSelectElement("Root")));
            if (token.HasValue && token.Value.IsCancellationRequested) return;
            processed.Save(outputPath);

            CleanupHelper.DeleteTempFiles([dbPath, sourceLsx, gdtPath]);
        }

        /// <summary>
        /// Creates the localization and visual files cache for a given data path.
        /// </summary>
        /// <param name="dataPath">The path to the user's game data.</param>
        /// <returns>The path to the cached localization file.</returns>
        private static string PrepareLocalizationAndVisualFiles(string dataPath, string[] extraVisualPaths, string language)
        {
            PreparationHelper.FindCharacterVisualsFiles(dataPath, extraVisualPaths);
            return PreparationHelper.FindLocalizationFile(dataPath, language);
        }

        /// <summary>
        /// Gets an <see cref="XmlSerializer"/> and an <see cref="XmlSerializerNamespaces"/> that should be used for .tml generation. 
        /// <paramref name="serializer"/> is initialized to serialize objects of type <see cref="Root"/> and <paramref name="namespaces"/> contains the pair ("", "").
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="namespaces"></param>
        private static void GetSerializer(out XmlSerializer serializer, out XmlSerializerNamespaces namespaces)
        {
            serializer = new(typeof(Root));
            namespaces = new();
            namespaces.Add("", "");
        }

        /// <summary>
        /// Copies the timeline source, scene, prefetch, and gdt files to the output destination (either Timeline Data or the mod specified in <paramref name="modName"/>).
        /// </summary>
        /// <param name="gdtPath">The path to the generated dialog timelines file, as a .lsx file.</param>
        /// <param name="sourceFile">The source file, as a .lsx file.</param>
        /// <param name="sourceNameExtensionless">The name of the source file, as obtained from <see cref="Path.GetFileNameWithoutExtension"/></param>
        /// <param name="gameDataPath">The path to the user's game data.</param>
        /// <param name="modName">The name of the mod. Should appear as it does in file paths. Used for automatic overriding.</param>
        /// <param name="doCopy">Whether or not to output the files to the user's mod or to the default Timeline Data directory.</param>
        private static void CopyTimelineFiles(string gdtPath, string sourceFile, string sourceNameExtensionless, string gameDataPath, string modName, bool doCopy)
        {
            CopyHelper.CopyTimelineFiles(sourceFile, sourceNameExtensionless, gameDataPath, modName, doCopy);
            CopyHelper.CopyGDTFile(gdtPath, sourceNameExtensionless, gameDataPath, modName, doCopy);
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
