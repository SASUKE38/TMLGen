using LSLib.LS;
using LSLib.LS.Enums;
using System;
using System.Diagnostics;
using System.IO;
using TMLGen.Forms.Logging;
using TMLGen.Properties;

namespace TMLGen.Generation.Helpers
{
    public static class FileHelper
    {
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
                    LoggingHelper.Write(Resources.TempFilesAccessError, 2);
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

        public static void OpenPath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Start(path);
                }
                else
                {
                    LoggingHelper.Write(String.Format(Resources.OpenPathDoesNotExist, path), 2);
                }
            }
            catch
            {
                LoggingHelper.Write(String.Format(Resources.OpenPathError, path), 2);
            }
        }

        public static void Start(string path)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
    }
}
