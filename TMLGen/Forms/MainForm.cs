using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TMLGen.Forms;
using TMLGen.Forms.Cache;
using TMLGen.Forms.Logging;
using TMLGen.Generation;

#pragma warning disable CA1416
namespace TMLGen
{
    public partial class MainForm : Form
    {
        public delegate void UpdateLog();
        public static UpdateLog logDelegate;
        public delegate Guid ShowMaterialSelection(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId);
        public static ShowMaterialSelection materialSelectionDelegate;
        public delegate Guid ShowLocationSelection(List<Guid> candidates);
        public static ShowLocationSelection locationSelectionDelegate;

        public MainForm()
        {
            InitializeComponent();
            LoggingHelper.Set(new(100u), formConsole, this);
            logDelegate = new UpdateLog(UpdateLogMethod);
            materialSelectionDelegate = new ShowMaterialSelection(MaterialSelectionMethod);
            locationSelectionDelegate = new ShowLocationSelection(LocationSelectionMethod);
        }

        public void UpdateLogMethod()
        {
            formConsole.Rtf = LoggingHelper.GetOutput();
        }

        public Guid MaterialSelectionMethod(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId)
        {
            Guid selectionRes = Guid.Empty;
            SlotMaterialSelection selection = new(candidates, materialId, resourceId);
            DialogResult diaRes = selection.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                selectionRes = selection.selected;
            }
            selection.Dispose();
            return selectionRes;
        }

        public Guid LocationSelectionMethod(List<Guid> candidates)
        {
            Guid selectionRes = Guid.Empty;
            LocationSelection selection = new(candidates);
            DialogResult diaRes = selection.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                selectionRes = selection.selected;
            }
            selection.Dispose();
            return selectionRes;
        }

        private struct GenerationArgs
        {
            public string sourceName;
            public string dataDirectory;
            public string sourceFile;
            public string gdtFile;
            public string dbFile;
            public string templateDirectory;
            public string outputPath;
            public bool manual;
            public bool separateAnimations;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            labelGDT.Enabled = !labelGDT.Enabled;
            buttonGDTBrowse.Enabled = !buttonGDTBrowse.Enabled;
            textBoxGDT.Enabled = !textBoxGDT.Enabled;
            labelDB.Enabled = !labelDB.Enabled;
            buttonDBBrowse.Enabled = !buttonDBBrowse.Enabled;
            textBoxDB.Enabled = !textBoxDB.Enabled;
            labelTT.Enabled = !labelTT.Enabled;
            buttonTTBrowse.Enabled = !buttonTTBrowse.Enabled;
            textBoxTT.Enabled = !textBoxTT.Enabled;
        }

        private void buttonSourceBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxSource.Text = openFileDialogSource.FileName;
            }
        }

        private void buttonGDTBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogGDT.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxGDT.Text = openFileDialogGDT.FileName;
            }
        }

        private void buttonDBBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogDB.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxDB.Text = openFileDialogDB.FileName;
            }
        }

        private void buttonTTBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogTemplates.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxTT.Text = folderBrowserDialogTemplates.SelectedPath;
            }
        }

        private void buttonDataBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogData.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxData.Text = folderBrowserDialogData.SelectedPath;
            }
        }

        private void buttonOutputBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialogOutput.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxOutput.Text = saveFileDialogOutput.FileName;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (CheckFiles())
            {
                string sourceName = Path.GetFileName(textBoxSource.Text);
                string sourceFile = PreparationHelper.SaveToLsxFile(textBoxSource.Text);
                if (sourceFile == null)
                {
                    LoggingHelper.Write("Failed to prepare source file.", 2);
                    return;
                }
                string gdtFile = null;
                string dbFile = null;
                string templateDirectory = null;
                string dataDirectory = textBoxData.Text;
                if (checkBoxManual.Checked)
                {
                    dbFile = textBoxDB.Text;
                    gdtFile = textBoxGDT.Text;
                    templateDirectory = textBoxTT.Text;
                }

                GenerationArgs args = new()
                {
                    sourceName = sourceName,
                    dataDirectory = dataDirectory,
                    sourceFile = sourceFile,
                    gdtFile = gdtFile,
                    dbFile = dbFile,
                    templateDirectory = templateDirectory,
                    outputPath = textBoxOutput.Text,
                    manual = checkBoxManual.Checked,
                    separateAnimations = checkBoxSeparateAnimations.Checked
                };
                buttonGenerate.Enabled = false;
                WriteSettingsToCache();
                backgroundWorker1.RunWorkerAsync(args);
            }
            else
            {
                LoggingHelper.Write("Generation failed.", 3);
            }
        }

        private bool CheckFiles()
        {
            return CheckExistence() && CheckExtensions();
        }

        private bool CheckExtensions()
        {
            bool checkSuccess = true;

            if (Path.GetExtension(textBoxSource.Text) != ".lsf")
            {
                LoggingHelper.Write("Source file must have a .lsf file extension.", 2);
                checkSuccess = false;
            }
            if (Path.GetExtension(textBoxOutput.Text) != ".tml")
            {
                LoggingHelper.Write("Output file must have a .tml file extension.", 2);
                checkSuccess = false;
            }
            if (checkBoxManual.Checked)
            {
                if (Path.GetExtension(textBoxGDT.Text) != ".lsf")
                {
                    LoggingHelper.Write("Generated dialog timelines file must have a .lsf file extension.", 2);
                    checkSuccess = false;
                }
                if (Path.GetExtension(textBoxDB.Text) != ".lsf")
                {
                    LoggingHelper.Write("Dialogs binary file must have a .lsf file extension.", 2);
                    checkSuccess = false;
                }
            }

            return checkSuccess;
        }

        private bool CheckExistence()
        {
            bool checkSuccess = true;
            if (!File.Exists(textBoxSource.Text))
            {
                LoggingHelper.Write("Failed to locate source file.", 2);
                checkSuccess = false;
            }
            if (textBoxOutput.Text.Length == 0)
            {
                LoggingHelper.Write("No output destination provided.", 2);
                checkSuccess = false;
            }
            if (!Directory.Exists(textBoxData.Text))
            {
                LoggingHelper.Write("Failed to locate unpacked data directory.", 2);
                checkSuccess = false;
            }
            if (checkBoxManual.Checked)
            {
                if (!File.Exists(textBoxGDT.Text))
                {
                    LoggingHelper.Write("Failed to locate generated dialog timelines file.", 2);
                    checkSuccess = false;
                }
                if (!File.Exists(textBoxDB.Text))
                {
                    LoggingHelper.Write("Failed to locate dialogs binary file.", 2);
                    checkSuccess = false;
                }
                if (!Directory.Exists(textBoxTT.Text))
                {
                    LoggingHelper.Write("Failed to locate timeline templates directory. This failure should be ignored if the timeline does not have templates.", 2);
                }
            }
            return checkSuccess;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GenerationArgs args = (GenerationArgs)e.Argument;
            e.Result = GenerationDriver.DoGeneration(this, args.sourceName, args.dataDirectory, args.sourceFile, args.gdtFile, args.dbFile, args.templateDirectory, args.outputPath, args.manual, args.separateAnimations);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null || (int)e.Result > 0)
                LoggingHelper.Write("Generation failed.", 3);
            else
                LoggingHelper.Write("Generation finished.", 1);
            buttonGenerate.Enabled = true;
            CleanupHelper.EmptyStaticCollections();
            CleanupHelper.DeleteTempFiles(PreparationHelper.visualPaths);
        }

        private void formConsole_TextChanged(object sender, EventArgs e)
        {
            formConsole.SelectionStart = formConsole.Text.Length;
            formConsole.ScrollToCaret();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (CacheHelper.TryPrepareSettingsCache())
            {
                Cache cache = CacheHelper.ReadSettingsCache();
                if (cache != null)
                {
                    textBoxSource.Text = cache.sourcePath;
                    textBoxGDT.Text = cache.gdtPath;
                    textBoxDB.Text = cache.dbPath;
                    textBoxData.Text = cache.dataPath;
                    textBoxTT.Text = cache.templatePath;
                    textBoxOutput.Text = cache.outputPath;
                    checkBoxManual.Checked = cache.manual;
                    checkBoxSeparateAnimations.Checked = cache.separateAnimations;
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            WriteSettingsToCache();
        }

        private void WriteSettingsToCache()
        {
            CacheHelper.WriteCache(new Cache(textBoxSource.Text, textBoxGDT.Text, textBoxDB.Text, textBoxData.Text, textBoxTT.Text, textBoxOutput.Text, checkBoxManual.Checked, checkBoxSeparateAnimations.Checked));
        }
    }
}
