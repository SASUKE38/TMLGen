using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TMLGen.Forms.Logging;

namespace TMLGen.Generation
{
    public static class CleanupHelper
    {
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

        public static void EmptyStaticDictionaries()
        {
            CollectorBase.actorTrackMapping.Clear();
            CollectorBase.trackMapping.Clear();
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
                LoggingHelper.Write("Failed to delete temp files.", 2);
                return;
            }
        }
    }
}
