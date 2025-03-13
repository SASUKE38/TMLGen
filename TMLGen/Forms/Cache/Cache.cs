using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLGen.Forms.Cache
{
    public class Cache
    {
        public string sourcePath;
        public string gdtPath;
        public string dbPath;
        public string dataPath;
        public string templatePath;
        public string outputPath;
        public bool manual;

        public Cache()
        {
            sourcePath = string.Empty;
            gdtPath = string.Empty;
            dbPath = string.Empty;
            dataPath = string.Empty;
            templatePath = string.Empty;
            outputPath = string.Empty;
            manual = false;
        }

        public Cache(string sourcePath, string gdtPath, string dbPath, string dataPath, string templatePath, string outputPath, bool manual)
        {
            this.sourcePath = sourcePath;
            this.gdtPath = gdtPath;
            this.dbPath = dbPath;
            this.dataPath = dataPath;
            this.templatePath = templatePath;
            this.outputPath = outputPath;
            this.manual = manual;
        }
    }
}
