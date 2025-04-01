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
        private string modName = string.Empty;

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
            public string dFile;
            public string templateDirectory;
            public string outputPath;
            public string rawSourcePath;
            public string modName;
            public bool manual;
            public bool separateAnimations;
            public bool doCopy;
        }

        private void checkBoxManual_CheckedChanged(object sender, EventArgs e)
        {
            labelGDT.Enabled = !labelGDT.Enabled;
            buttonGDTBrowse.Enabled = !buttonGDTBrowse.Enabled;
            textBoxGDT.Enabled = !textBoxGDT.Enabled;
            labelDB.Enabled = !labelDB.Enabled;
            buttonDBBrowse.Enabled = !buttonDBBrowse.Enabled;
            textBoxDB.Enabled = !textBoxDB.Enabled;
            labelD.Enabled = !labelD.Enabled;
            buttonDBrowse.Enabled = !buttonDBrowse.Enabled;
            textBoxD.Enabled = !textBoxD.Enabled;
            labelTT.Enabled = !labelTT.Enabled;
            buttonTTBrowse.Enabled = !buttonTTBrowse.Enabled;
            textBoxTT.Enabled = !textBoxTT.Enabled;
        }

        private void checkBoxCopy_CheckedChanged(object sender, EventArgs e)
        {
            labelGameData.Enabled = !labelGameData.Enabled;
            buttonGameDataBrowse.Enabled = !buttonGameDataBrowse.Enabled;
            textBoxGameData.Enabled = !textBoxGameData.Enabled;
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

        private void buttonDBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxD.Text = openFileDialogD.FileName;
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

        private void buttonGameDataBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogGameData.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxGameData.Text = folderBrowserDialogGameData.SelectedPath;
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
                string dFile = null;
                string templateDirectory = null;
                string dataDirectory = textBoxData.Text;
                if (checkBoxManual.Checked)
                {
                    dbFile = textBoxDB.Text;
                    dFile = textBoxD.Text;
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
                    dFile = dFile,
                    templateDirectory = templateDirectory,
                    outputPath = textBoxGameData.Text,
                    rawSourcePath = textBoxSource.Text,
                    manual = checkBoxManual.Checked,
                    modName = modName,
                    separateAnimations = checkBoxSeparateAnimations.Checked,
                    doCopy = checkBoxCopy.Checked
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
                if (Path.GetExtension(textBoxD.Text) != ".lsj")
                {
                    LoggingHelper.Write("Dialogs file must have a .lsj file extension.", 2);
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
            if (!Directory.Exists(textBoxData.Text))
            {
                LoggingHelper.Write("Failed to locate unpacked data directory.", 2);
                checkSuccess = false;
            }
            if (checkBoxCopy.Checked && !Directory.Exists(textBoxGameData.Text))
            {
                LoggingHelper.Write("Failed to locate game data directory.", 2);
                checkSuccess = false;
            }
            if (checkBoxCopy.Checked && listBoxMods.SelectedIndex == -1)
            {
                LoggingHelper.Write("No mod selected.", 2);
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
                if (!File.Exists(textBoxD.Text))
                {
                    LoggingHelper.Write("Failed to locate dialogs file.", 2);
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
            e.Result = GenerationDriver.DoGeneration(this, args.sourceName, args.dataDirectory, args.sourceFile, args.gdtFile, args.dbFile, args.dFile, args.templateDirectory, args.outputPath, args.rawSourcePath, args.modName, args.manual, args.separateAnimations, args.doCopy);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null || (int)e.Result > 0)
                LoggingHelper.Write("An error occurred during generation.", 3);
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
                    textBoxD.Text = cache.dPath;
                    textBoxData.Text = cache.dataPath;
                    textBoxTT.Text = cache.templatePath;
                    textBoxGameData.Text = cache.gameDataPath;
                    checkBoxManual.Checked = cache.manual;
                    checkBoxSeparateAnimations.Checked = cache.separateAnimations;
                    checkBoxCopy.Checked = cache.doCopy;
                    foreach (string mod in cache.mods)
                    {
                        listBoxMods.Items.Add(mod);
                    }
                    listBoxMods.SelectedIndex = cache.modIndex;
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            WriteSettingsToCache();
        }

        private void WriteSettingsToCache()
        {
            List<string> modList = [];
            foreach (object mod in listBoxMods.Items)
            {
                modList.Add(mod.ToString());
            }
            CacheHelper.WriteCache(new Cache(textBoxSource.Text, textBoxGDT.Text, textBoxDB.Text, textBoxD.Text, textBoxData.Text, textBoxTT.Text, textBoxGameData.Text, modList, listBoxMods.SelectedIndex, checkBoxManual.Checked, checkBoxSeparateAnimations.Checked, checkBoxCopy.Checked));
        }

        private void buttonModsAdd_Click(object sender, EventArgs e)
        {
            ModSelection selection = new();
            DialogResult diaRes = selection.ShowDialog();
            if (diaRes == DialogResult.OK && selection.modName != string.Empty)
            {
                if (!listBoxMods.Items.Contains(selection.modName))
                {
                    listBoxMods.Items.Add(selection.modName);
                }
                else
                {
                    LoggingHelper.Write("This mod already exists in the mod list.", 2);
                }
            }
            selection.Dispose();
        }

        private void buttonModsRemove_Click(object sender, EventArgs e)
        {
            string item = (string)listBoxMods.SelectedItem;
            if (item != null)
            {
                listBoxMods.Items.Remove(item);
            }
        }

        private void listBoxMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = (string)listBoxMods.SelectedItem;
            if (selected != null) modName = selected;
        }
    }
}
