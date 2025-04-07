namespace TMLGen.Generation.Helpers
{
    public static class VersionHelper
    {
        private static readonly int major = 0;
        private static readonly int minor = 3;
        private static readonly int patch = 1;

        public static string GetVersion()
        {
            return string.Format("{0}.{1}.{2}", major, minor, patch);
        }
    }
}
