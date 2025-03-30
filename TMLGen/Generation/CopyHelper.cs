using LSLib.LS.Enums;
using LSLib.LS;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Forms.Logging;

namespace TMLGen.Generation
{
    public static class CopyHelper
    {
        private static readonly string copiedDataDirectoryName = "Timeline Data";

        public static void CopyTimelineFiles(string sourcePath, string sourceName, string gameDataPath, string modName, bool doModCopy)
        {
            try
            {
                string copyDest;
                if (doModCopy)
                {
                    copyDest = Path.Join(gameDataPath, "Public", modName, "Timeline", "Generated");
                }
                else
                {
                    copyDest = Path.Join(copiedDataDirectoryName, sourceName);
                }
                Directory.CreateDirectory(copyDest);

                File.Copy(sourcePath, Path.Join(copyDest, Path.GetFileName(sourcePath)), true);
                string sceneName = sourceName + "_Scene.lsx";
                string scenePath = Path.Join(Path.GetDirectoryName(sourcePath), sceneName);
                string prefetchName = sourceName + "_Prefetch.lsf";
                string prefetchPath = Path.Join(Path.GetDirectoryName(sourcePath), prefetchName);
                if (File.Exists(scenePath))
                    File.Copy(scenePath, Path.Join(copyDest, sceneName), true);
                if (File.Exists(prefetchPath))
                    File.Copy(prefetchPath, Path.Join(copyDest, prefetchName), true);
            }
            catch (Exception)
            {
                LoggingHelper.Write("An error occurred when copying the timeline's files.", 2);
            }
        }

        public static void CopyTemplates(string sourceName, string templatePath, Guid timelineId, string gameDataPath, string modName, bool doModCopy)
        {
            try
            {
                string copyDest;
                if (doModCopy)
                {
                    copyDest = Path.Join(gameDataPath, "Public", modName, "TimelineTemplates", timelineId.ToString());
                }
                else
                {
                    copyDest = Path.Join(copiedDataDirectoryName, sourceName, timelineId.ToString());
                }
                Directory.CreateDirectory(copyDest);

                foreach (string file in Directory.GetFiles(templatePath))
                {
                    if (Path.GetExtension(file) == ".lsf")
                        File.Copy(file, Path.Join(copyDest, Path.GetFileName(file)), true);
                }
            }
            catch (Exception)
            {
                LoggingHelper.Write("An error occurred when copying the timeline's templates.", 2);
            }
        }

        public static string GetOutputPath(string sourceName, string gameDataPath, string modName, bool doModCopy)
        {
            try
            {
                string copyDest;
                if (doModCopy)
                {
                    copyDest = Path.Join(gameDataPath, "Editor", "Mods", modName, "Timeline", "Generated");
                }
                else
                {
                    copyDest = Path.Join(copiedDataDirectoryName, sourceName);
                }
                Directory.CreateDirectory(copyDest);

                return Path.Join(copyDest, (sourceName + ".tml"));
            }
            catch (Exception)
            {
                LoggingHelper.Write("An error occurred when getting the .tml output location.", 2);
                return string.Empty;
            }
        }

        public static void CopyGDTFile(string gdtPath, string sourceName, string gameDataPath, string modName, bool doModCopy)
        {
            try
            {
                string outputPath;
                if (doModCopy)
                {
                    outputPath = Path.Join(gameDataPath, "Public", modName, "Content", "Generated", "[PAK]_GeneratedDialogTimelines", sourceName + ".lsx");
                }
                else
                {
                    outputPath = Path.Join(copiedDataDirectoryName, sourceName, sourceName + "_GDT.lsx");
                }

                XDocument doc = XDocument.Load(gdtPath);
                var xmlSource = doc.XPathSelectElement("//attribute[@id='SourceFile']");
                xmlSource.Attribute("value").Value = "Public/" + modName + "/Timeline/Generated/" + sourceName + ".lsf";
                var xmlEditorSource = doc.XPathSelectElement("//attribute[@id='EditorSourceFile']");
                xmlEditorSource.Attribute("value").Value = "Editor/Mods/" + modName + "/Timeline/Generated/" + sourceName + ".tml";
                doc.Save(outputPath);

                Resource resource = ResourceUtils.LoadResource(outputPath, ResourceLoadParameters.FromGameVersion(Game.BaldursGate3));
                ResourceUtils.SaveResource(resource, Path.ChangeExtension(Path.GetFullPath(outputPath), ".lsf"), ResourceConversionParameters.FromGameVersion(Game.BaldursGate3));
                File.Delete(outputPath);
            }
            catch (Exception)
            {
                LoggingHelper.Write("An error occurred when copying the generated dialog timelines file.", 2);
            }
        }
    }
}
