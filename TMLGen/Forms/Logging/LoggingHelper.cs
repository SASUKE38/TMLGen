using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TMLGen.Forms.Logging
{
    public static class LoggingHelper
    {
        private static Logger log;
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
            log = logToUse;
            form = formToUse;
        }

        public static void Write(string text, int colorNum)
        {
            typeDict.TryGetValue(colorNum, out Color color);
            log?.AddToLog(ConvertString(text), color);
            form.Invoke(MainForm.logDelegate);
        }

        public static void Write(string text)
        {
            Write(text, 0);
        }

        // https://stackoverflow.com/questions/4795709/how-to-convert-a-string-to-rtf-in-c
        private static string ConvertString(string text)
        {
            //first take care of special RTF chars
            StringBuilder backslashed = new StringBuilder(text);
            backslashed.Replace(@"\", @"\\");
            backslashed.Replace(@"{", @"\{");
            backslashed.Replace(@"}", @"\}");

            //then convert the string char by char
            StringBuilder sb = new();
            foreach (char character in backslashed.ToString())
            {
                if (character <= 127)
                    sb.Append(character);
                else
                    sb.Append("\\u" + Convert.ToUInt32(character) + "?");
            }
            return sb.ToString();
        }

        public static string GetOutput()
        {
            if (log != null)
            {
                return log.GetLogAsRichText(false);
            }
            return null;
        }
    }
}
