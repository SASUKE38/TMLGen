using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TMLGen.Forms.Logging;
using TMLGen.Properties;

namespace TMLGen.Generation.Helpers
{
    public static class CleanupHelper
    {
        public static void WriteException(Exception e)
        {
            try
            {
                string exceptionDir = "ExceptionLogs";
                Directory.CreateDirectory(exceptionDir);
                DateTime time = DateTime.Now;
                string timeString = time.Month + "-" + time.Day + "-" + time.Year + " " + time.Hour + "-" + time.Minute + "-" + time.Second + "-" + time.Millisecond;
                File.WriteAllText(Path.Combine(exceptionDir, timeString + ".txt"), e.ToString());
            }
            catch (Exception)
            {
                LoggingHelper.Write(String.Format(Resources.LoggingExceptionError, e), 3);
            }
        }

        public static XElement DoPostProcess(XElement root)
        {
            List<XAttribute> remo = [];
            foreach (XElement ele in root.DescendantsAndSelf())
            {
                ele.Name = ele.Name.LocalName;
                foreach (XAttribute at in ele.Attributes().Where(xa => xa.ToString().Contains(':')))
                {
                    remo.Add(at);
                }
            }
            
            foreach(XAttribute r in remo)
            {
                r.Remove();
            }
            return root;
        }

        public static void DeleteTempFiles(string[] files)
        {
            try
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (IOException)
            {
                LoggingHelper.Write(Resources.TempFilesDeletionFailed, 2);
                return;
            }
        }
    }
}
