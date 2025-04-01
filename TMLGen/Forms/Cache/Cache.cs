using System.Collections.Generic;

namespace TMLGen.Forms.Cache
{
    public class Cache
    {
        public string sourcePath;
        public string gdtPath;
        public string dbPath;
        public string dataPath;
        public string templatePath;
        public string gameDataPath;
        public bool manual;
        public bool separateAnimations;
        public bool doCopy;
        public List<string> mods;
        public int modIndex;

        public Cache()
        {
            sourcePath = string.Empty;
            gdtPath = string.Empty;
            dbPath = string.Empty;
            dataPath = string.Empty;
            templatePath = string.Empty;
            gameDataPath = string.Empty;
            manual = false;
            separateAnimations = true;
            doCopy = true;
            mods = [];
            modIndex = -1;
        }

        public Cache(string sourcePath, string gdtPath, string dbPath, string dataPath, string templatePath, string gameDataPath, List<string> mods, int modIndex, bool manual, bool separateAnimations, bool doCopy)
        {
            this.sourcePath = sourcePath;
            this.gdtPath = gdtPath;
            this.dbPath = dbPath;
            this.dataPath = dataPath;
            this.templatePath = templatePath;
            this.gameDataPath = gameDataPath;
            this.manual = manual;
            this.separateAnimations = separateAnimations;
            this.doCopy = doCopy;
            this.mods = mods;
            this.modIndex = modIndex;
        }
    }
}
