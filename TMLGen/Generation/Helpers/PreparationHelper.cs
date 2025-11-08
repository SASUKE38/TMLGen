using LSLib.LS;
using LSLib.LS.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Forms.Logging;
using TMLGen.Generation.Collectors;
using TMLGen.Properties;

namespace TMLGen.Generation.Helpers
{
    public static class PreparationHelper
    {
        public static List<XDocument> visualFiles = [];
        public static string[] visualPaths = [];

        public static string FindGeneratedDialogTimelinesFile(string dataDirectory, string sourceName)
        {
            if (dataDirectory != null)
            {
                string[] pathsToTry =
                [
                    Path.Join([dataDirectory, "Gustav", "Public", "GustavDev", "Content", "Generated", "[PAK]_GeneratedDialogTimelines"]),
                    Path.Join([dataDirectory, "Gustav", "Public", "Gustav", "Content", "Generated", "[PAK]_GeneratedDialogTimelines"]),
                    Path.Join([dataDirectory, "Shared", "Public", "SharedDev", "Content", "Generated", "[PAK]_GeneratedDialogTimelines"]),
                    Path.Join([dataDirectory, "Shared", "Public", "Shared", "Content", "Generated", "[PAK]_GeneratedDialogTimelines"])
                ];

                for (int i = 0; i < pathsToTry.Length; i++)
                {
                    try
                    {
                        string filePath = Path.Join(pathsToTry[i], sourceName);
                        string mergedPath = Path.Join(pathsToTry[i], "_merged.lsf");
                        if (File.Exists(filePath)) return filePath;
                        if (File.Exists(mergedPath))
                        {
                            string mergedTempFile = FileHelper.SaveToLsxFile(mergedPath);
                            string element = GetGDTElementFromMerged(mergedTempFile, Path.GetFileNameWithoutExtension(sourceName));
                            if (element != null)
                                return element;
                        }
                    }
                    catch (Exception)
                    {
                        LoggingHelper.Write(String.Format(Resources.GDTFileLocatingError, sourceName), 2);
                        return null;
                    }
                }
            }
            return null;
        }

        public static string FindDialogsBinaryFile(string dataDirectory, string sourceName)
        {
            if (dataDirectory != null)
            {
                string[] pathsToTry =
                [
                    Path.Join([dataDirectory, "Gustav", "Mods", "GustavDev", "Story", "DialogsBinary"]),
                    Path.Join([dataDirectory, "Gustav", "Mods", "Gustav", "Story", "DialogsBinary"]),
                    Path.Join([dataDirectory, "Shared", "Mods", "SharedDev", "Story", "DialogsBinary"]),
                    Path.Join([dataDirectory, "Shared", "Mods", "Shared", "Story", "DialogsBinary"])
                ];

                for (int i = 0; i < pathsToTry.Length; i++)
                {
                    try
                    {
                        foreach (string file in Directory.EnumerateFiles(pathsToTry[i], "*.lsf", SearchOption.AllDirectories))
                        {
                            if (Path.GetFileName(file) == sourceName) return file;
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        LoggingHelper.Write(Resources.UnpackedDataMissingDBContent, 2);
                        return null;
                    }
                    catch (Exception ex) when (ex is IOException || ex is SecurityException || ex is UnauthorizedAccessException)
                    {
                        LoggingHelper.Write(Resources.UnpackedDataAccessError, 2);
                        return null;
                    }
                    catch (PathTooLongException)
                    {
                        LoggingHelper.Write(String.Format(Resources.DBSearchPathTooLong, sourceName), 2);
                        return null;
                    }
                }
            }
            return null;
        }

        public static string FindDialogsFile(string dataDirectory, string sourceName)
        {
            if (dataDirectory != null)
            {
                string[] pathsToTry =
                [
                    Path.Join([dataDirectory, "Gustav", "Mods", "GustavDev", "Story", "Dialogs"]),
                    Path.Join([dataDirectory, "Gustav", "Mods", "Gustav", "Story", "Dialogs"]),
                    Path.Join([dataDirectory, "Shared", "Mods", "SharedDev", "Story", "Dialogs"]),
                    Path.Join([dataDirectory, "Shared", "Mods", "Shared", "Story", "Dialogs"])
                ];

                for (int i = 0; i < pathsToTry.Length; i++)
                {
                    try
                    {
                        string sourceLsj = Path.ChangeExtension(sourceName, ".lsj");
                        foreach (string file in Directory.EnumerateFiles(pathsToTry[i], "*.lsj", SearchOption.AllDirectories))
                        {
                            if (Path.GetFileName(file) == sourceLsj) return file;
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        LoggingHelper.Write(Resources.UnpackedDataMissingDContent, 2);
                        return null;
                    }
                    catch (Exception ex) when (ex is IOException || ex is SecurityException || ex is UnauthorizedAccessException)
                    {
                        LoggingHelper.Write(Resources.UnpackedDataAccessError, 2);
                        return null;
                    }
                    catch (PathTooLongException)
                    {
                        LoggingHelper.Write(String.Format(Resources.DSearchPathTooLong, sourceName), 2);
                        return null;
                    }
                }
            }
            return null;
        }

        public static string FindTemplatesFolder(string dataDirectory, Guid timelineId)
        {
            if (dataDirectory != null)
            {
                string[] pathsToTry =
                [
                    Path.Join([dataDirectory, "Gustav", "Public", "GustavDev", "TimelineTemplates"]),
                    Path.Join([dataDirectory, "Gustav", "Public", "Gustav", "TimelineTemplates"]),
                    Path.Join([dataDirectory, "Shared", "Public", "SharedDev", "TimelineTemplates"]),
                    Path.Join([dataDirectory, "Shared", "Public", "Shared", "TimelineTemplates"])
                ];

                for (int i = 0; i < pathsToTry.Length; i++)
                {
                    try
                    {
                        string directoryPath = Path.Join(pathsToTry[i], timelineId.ToString());
                        if (Directory.Exists(directoryPath)) return directoryPath;
                    }
                    catch (Exception)
                    {
                        LoggingHelper.Write(String.Format(Resources.TTDirectoryLocatingError, timelineId));
                        return null;
                    }
                }
            }
            return null;
        }

        public static string GetGDTElementFromMerged(string gdtPath, string name)
        {
            XDocument mergedDoc = XDocument.Load(gdtPath);
            XElement gdtElement = mergedDoc.XPathSelectElement("save/region[@id='TimelineBank']/node[@id='TimelineBank']/children/node[@id='Resource'][attribute[@id='Name'][@value='" + name + "']]");
            if (gdtElement != null)
            {
                XAttribute resAtt = new("id", "TimelineBank");
                XDocument res =
                    new(new XElement("save", new XElement("region", resAtt, new XElement("node", resAtt, new XElement("children", gdtElement)))));
                res.Save(gdtPath);
                return gdtPath;
            }
            File.Delete(gdtPath);
            return null;
        }

        public static void FindCharacterVisualsFiles(string dataDirectory, string[] extraPaths)
        {
            visualFiles.Clear();
            string directoryName = "TmlVisualFilesCache";
            List<(string prefix, string package)> packageNames = [("Gustav", "GustavDev"), ("Gustav", "Gustav"), ("Shared", "SharedDev"), ("Shared", "Shared")];
            try
            {
                Directory.CreateDirectory(directoryName);
            }
            catch (Exception)
            {
                LoggingHelper.Write(Resources.VisualCacheCreationError, 2);
                return;
            }
            foreach ((string prefix, string package) in packageNames)
            {
                try
                {
                    string cachePath = Path.Join(directoryName, package + ".lsx");
                    if (!File.Exists(cachePath))
                    {
                        CreateCachedVisualFile(dataDirectory, cachePath, prefix, package);
                    }
                    visualFiles.Add(XDocument.Load(cachePath));
                }
                catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
                {
                    LoggingHelper.Write(String.Format(Resources.VisualCachePackageDoesNotExist, package), 2);
                }
                catch (Exception)
                {
                    LoggingHelper.Write(Resources.VisualCacheCreationError, 2);
                }
            }

            foreach (string path in extraPaths)
            {
                if (Path.GetExtension(path) == ".lsf")
                {
                    string lsxFile = FileHelper.SaveToLsxFile(path);
                    visualFiles.Add(XDocument.Load(lsxFile));
                    visualPaths = (string[])visualPaths.Append(lsxFile);
                }
                else
                {
                    LoggingHelper.Write(Resources.VisualCacheWrongExtension, 2);
                }
            }
        }

        private static void CreateCachedVisualFile(string dataDirectory, string cachePath, string packagePrefix, string package)
        {
            string filePath = Path.Join([dataDirectory, packagePrefix, "Public", package, "Content", "[PAK]_CharacterVisuals", "_merged.lsf"]);
            FileHelper.SaveToLsxFile(filePath, Path.GetFullPath(cachePath));
        }

        public static string FindLocalizationFile(string dataDirectory, string language)
        {
            try
            {
                string directoryName = "TmlLocalizationCache";
                Directory.CreateDirectory(directoryName);

                string cachePath = Path.Join(directoryName, language + ".xml");
                if (!File.Exists(cachePath))
                {
                    CreateCachedLocalizationFile(dataDirectory, cachePath, language);
                }
                return cachePath;
            }
            catch (Exception)
            {
                LoggingHelper.Write(Resources.LocalizationCacheCreationError, 2);
                return null;
            }
        }

        private static void CreateCachedLocalizationFile(string dataDirectory, string cachePath, string language)
        {
            string localizationPath = Path.Join(dataDirectory, language, "Localization", language, language.ToLower() + ".loca");
            LocaResource resource = LocaUtils.Load(localizationPath, LocaFormat.Loca);
            LocaUtils.Save(resource, Path.GetFullPath(cachePath), LocaFormat.Xml);
        }

        public static ReferenceFlagPaths GetReferenceFlagPaths(string dataDirectory)
        {
            bool didMissingWarning = false;
            bool CheckFlagPath(string path, Func<string, bool> predicate)
            {
                bool res = predicate(path);
                if (!res && !didMissingWarning)
                {
                    didMissingWarning = true;
                    LoggingHelper.Write(Resources.ReferenceFlagPathDoesNotExist, 2);
                }
                return res;
            }

            List<string> flagPaths =
            [
                Path.Join([dataDirectory, "Gustav", "Public", "GustavDev", "Flags"]),
                Path.Join([dataDirectory, "Gustav", "Public", "Gustav", "Flags"]),
                Path.Join([dataDirectory, "Shared", "Public", "SharedDev", "Flags"]),
                Path.Join([dataDirectory, "Shared", "Public", "Shared", "Flags"]),
            ];
            List<string> tagPaths =
            [
                Path.Join([dataDirectory, "Gustav", "Public", "GustavDev", "Tags"]),
                Path.Join([dataDirectory, "Gustav", "Public", "Gustav", "Tags"]),
                Path.Join([dataDirectory, "Shared", "Public", "SharedDev", "Tags"]),
                Path.Join([dataDirectory, "Shared", "Public", "Shared", "Tags"]),
            ];
            List<string> scriptFlagPaths =
            [
                Path.Join([dataDirectory, "Gustav", "Mods","GustavDev", "Story", "Dialogs", "ScriptFlags", "ScriptFlags.lsx"]),
                Path.Join([dataDirectory, "Gustav", "Mods","Gustav", "Story", "Dialogs", "ScriptFlags", "ScriptFlags.lsx"]),
                Path.Join([dataDirectory, "Shared", "Mods","Shared", "Story", "Dialogs", "ScriptFlags", "ScriptFlags.lsx"]),
            ];
            List<string> questFlagPaths =
            [
                Path.Join([dataDirectory, "Gustav", "Mods","GustavDev", "Story", "Journal", "quest_prototypes.lsx"])
            ];

            ReferenceFlagPaths res = new()
            {
                flagPaths = [.. flagPaths.Where(path => CheckFlagPath(path, Directory.Exists))],
                tagPaths = [.. tagPaths.Where(path => CheckFlagPath(path, Directory.Exists))],
                scriptFlagPaths = [.. scriptFlagPaths.Where(path => CheckFlagPath(path, File.Exists))],
                questFlagPaths = [.. questFlagPaths.Where(path => CheckFlagPath(path, File.Exists))]
            };
            return res;
        }
    }
}
