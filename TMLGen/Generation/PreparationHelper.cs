using LSLib.LS;
using LSLib.LS.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Forms.Logging;

namespace TMLGen.Generation
{
    public static class PreparationHelper
    {
        public static List<XDocument> visualFiles = [];
        public static string[] visualPaths = [];

        public static string SaveToLsxFile(string path)
        {
            if (Path.GetExtension(path) == ".lsf")
            {
                try
                {
                    string tempPath = Path.GetTempFileName();
                    string lsxTempPath = Path.ChangeExtension(tempPath, ".lsx");
                    File.Delete(tempPath);
                    Resource resource = ResourceUtils.LoadResource(path, ResourceLoadParameters.FromGameVersion(Game.BaldursGate3));
                    ResourceUtils.SaveResource(resource, lsxTempPath, ResourceConversionParameters.FromGameVersion(Game.BaldursGate3));
                    return lsxTempPath;
                }
                catch (IOException)
                {
                    LoggingHelper.Write("Error accessing temp file.", 2);
                    return null;
                }
            }
            return null;
        }

        public static void SaveToLsxFile(string path, string output)
        {
            if (Path.GetExtension(path) == ".lsf")
            {
                Resource resource = ResourceUtils.LoadResource(path, ResourceLoadParameters.FromGameVersion(Game.BaldursGate3));
                ResourceUtils.SaveResource(resource, output, ResourceConversionParameters.FromGameVersion(Game.BaldursGate3));
            }
        }

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
                            string mergedTempFile = SaveToLsxFile(mergedPath);
                            return GetGDTElementFromMerged(mergedTempFile, Path.GetFileNameWithoutExtension(sourceName));
                        }
                    }
                    catch (Exception)
                    {
                        LoggingHelper.Write("Error locating generated dialog timelines file.", 2);
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
                        LoggingHelper.Write("Unpacked data directory missing dialogs binary content.", 2);
                        return null;
                    }
                    catch (Exception ex) when (ex is IOException || ex is SecurityException || ex is UnauthorizedAccessException)
                    {
                        LoggingHelper.Write("Unpacked data directory could not be accessed.", 2);
                        return null;
                    }
                    catch (PathTooLongException)
                    {
                        LoggingHelper.Write("Dialogs binary search path is too long.", 2);
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
                        LoggingHelper.Write("Templates folder could not be found. This failure can be ignored if the timeline does not have templates.");
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
            string directoryName = "TmlVisualFilesCache";
            List<(string prefix, string package)> packageNames = [("Gustav", "GustavDev"), ("Gustav", "Gustav"), ("Shared", "SharedDev"), ("Shared", "Shared")];
            Directory.CreateDirectory(directoryName);
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
                    LoggingHelper.Write("Couldn't find character visuals file for " + package + ".", 2);
                }
            }

            foreach (string path in extraPaths)
            {
                if (Path.GetExtension(path) == ".lsf")
                {
                    string lsxFile = SaveToLsxFile(path);
                    visualFiles.Add(XDocument.Load(lsxFile));
                    visualPaths = (string[])visualPaths.Append(lsxFile);
                }
                else
                {
                    LoggingHelper.Write("A character visual file is not a .lsf file.", 2);
                }
            }
        }

        private static void CreateCachedVisualFile(string dataDirectory, string cachePath, string packagePrefix, string package)
        {
            string filePath = Path.Join([dataDirectory, packagePrefix, "Public", package, "Content", "[PAK]_CharacterVisuals", "_merged.lsf"]);
            SaveToLsxFile(filePath, Path.GetFullPath(cachePath));
        }
    }
}
