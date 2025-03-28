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
        public bool separateAnimations;
        public bool doCopy;

        public Cache()
        {
            sourcePath = string.Empty;
            gdtPath = string.Empty;
            dbPath = string.Empty;
            dataPath = string.Empty;
            templatePath = string.Empty;
            outputPath = string.Empty;
            manual = false;
            separateAnimations = true;
            doCopy = true;
        }

        public Cache(string sourcePath, string gdtPath, string dbPath, string dataPath, string templatePath, string outputPath, bool manual, bool separateAnimations, bool doCopy)
        {
            this.sourcePath = sourcePath;
            this.gdtPath = gdtPath;
            this.dbPath = dbPath;
            this.dataPath = dataPath;
            this.templatePath = templatePath;
            this.outputPath = outputPath;
            this.manual = manual;
            this.separateAnimations = separateAnimations;
            this.doCopy = doCopy;
        }
    }
}
