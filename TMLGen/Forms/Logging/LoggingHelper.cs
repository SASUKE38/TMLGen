using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TMLGen.Forms.Logging
{
    public static class LoggingHelper
    {
        private static Logger log;
        private static int quantity;
        private static Form form;

        private static readonly Dictionary<int, Color> typeDict = new()
        {
            { 0, Color.White },
            { 1, Color.Green },
            { 2, Color.Yellow },
            { 3, Color.Red },
        };

        public static void Set(Logger logToUse, RichTextBox consoleToUse, Form formToUse)
        {
            quantity = 0;
            log = logToUse;
            form = formToUse;
        }

        public static void Write(string text, int colorNum)
        {
            typeDict.TryGetValue(colorNum, out Color color);
            log?.AddToLog(text, color);
            form.Invoke(MainForm.logDelegate);
        }

        public static void Write(string text)
        {
            Write(text, 0);
        }

        public static string GetOutput()
        {
            if (log != null)
            {
                int count = log.GetLogCount();
                if (count != quantity)
                {
                    quantity = count;
                    return log.GetLogAsRichText(false);
                }
            }
            return null;
        }
    }
}
