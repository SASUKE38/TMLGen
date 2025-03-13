using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TMLGen.Forms.Logging;

namespace TMLGen.Forms.Cache
{
    public static class CacheHelper
    {
        private static readonly XmlSerializer serializer = new(typeof(Cache));
        private static readonly string settingsCachePath = "TMLSettingsCache.xml";

        public static bool TryPrepareSettingsCache()
        {
            if (!File.Exists(settingsCachePath))
            {
                Cache cache = new();
                WriteCache(cache);
                return false;
            }
            return true;
        }

        public static Cache ReadSettingsCache()
        {
            try
            {
                FileStream fs = new FileStream(settingsCachePath, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);
                Cache res = (Cache)serializer.Deserialize(reader);
                fs.Close();
                return res;
            }
            catch (Exception)
            {
                LoggingHelper.Write("Error reading from settings cache.");
            }
            return null;
        }

        public static void WriteCache(Cache cache)
        {
            StreamWriter writer = new(settingsCachePath);
            serializer.Serialize(writer, cache);
            writer.Close();
        }
    }
}
