using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TMLGen.Forms;
using TMLGen.Forms.Logging;
using TMLGen.Generation;
using TMLGen.Generation.Helpers;
using TMLGen.Properties;

#pragma warning disable CA1416
namespace TMLGen
{
    public partial class MainForm : Form
    {
        public delegate void UpdateLog();
        public static UpdateLog logDelegate;
        public delegate void UpdateCurrentBatchFile(string fileName);
        public static UpdateCurrentBatchFile currentBatchFileDelegate;
        public delegate Guid ShowMaterialSelection(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId);
        public static ShowMaterialSelection materialSelectionDelegate;
        public delegate Guid ShowLocationSelection(HashSet<Guid> candidates);
        public static ShowLocationSelection locationSelectionDelegate;
        private string modName = string.Empty;

        private static readonly uint logMax = 100u;

        public MainForm()
        {
            InitializeComponent();

            Text += " v" + Application.ProductVersion;

            LoggingHelper.Set(new(logMax), formConsole, this);

            logDelegate = new UpdateLog(UpdateLogMethod);
            currentBatchFileDelegate = new UpdateCurrentBatchFile(UpdateCurrentBatchFileMethod);
            materialSelectionDelegate = new ShowMaterialSelection(MaterialSelectionMethod);
            locationSelectionDelegate = new ShowLocationSelection(LocationSelectionMethod);
        }

        public void UpdateLogMethod()
        {
            formConsole.Rtf = LoggingHelper.GetOutput();
        }

        public void UpdateCurrentBatchFileMethod(string fileName)
        {
            labelBatchCurrentName.Text = fileName;
            labelBatchCurrent.Visible = true;
            labelBatchCurrentName.Visible = true;
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

        public Guid LocationSelectionMethod(HashSet<Guid> candidates)
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

        private struct BatchGenerationArgs
        {
            public string inputName;
            public string dataDirectory;
            public string outputPath;
            public string modName;
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            LoggingHelper.Write(Resources.GenerationCanceling, 2);
            backgroundWorker2.CancelAsync();
            buttonCancel.Enabled = false;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (tabControlMode.SelectedIndex == 0)
            {
                SingleGeneration();
            }
            else
            {
                BatchGeneration();
            }
        }

        private void buttonBatch_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogBatchSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxBatch.Text = folderBrowserDialogBatchSource.SelectedPath;
            }
        }

        private void SingleGeneration()
        {
            if (CheckFiles())
            {
                string sourceName = Path.GetFileName(textBoxSource.Text);
                string sourceFile = PreparationHelper.SaveToLsxFile(textBoxSource.Text);
                if (sourceFile == null)
                {
                    LoggingHelper.Write(Resources.SourceFilePreparationFailure, 2);
                    return;
                }
                string gdtFile = null;
                string dbFile = null;
                string dFile = null;
                string templateDirectory = null;
                string dataDirectory = Settings.Default.UnpackedDataDirectory;
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
                    outputPath = Settings.Default.GameDataDirectory,
                    rawSourcePath = textBoxSource.Text,
                    manual = checkBoxManual.Checked,
                    modName = modName,
                    separateAnimations = checkBoxSeparateAnimations.Checked,
                    doCopy = checkBoxCopy.Checked
                };
                buttonGenerate.Enabled = false;
                WriteSettings();
                backgroundWorker1.RunWorkerAsync(args);
            }
            else
            {
                LoggingHelper.Write(Resources.GenerationFailed, 3);
            }
        }

        private void BatchGeneration()
        {
            if (CheckExistenceBatch())
            {
                if (Directory.Exists(textBoxBatch.Text) && CheckExistenceBatch())
                {
                    BatchGenerationArgs args = new()
                    {
                        inputName = textBoxBatch.Text,
                        dataDirectory = Settings.Default.UnpackedDataDirectory,
                        outputPath = Settings.Default.GameDataDirectory,
                        modName = modName,
                        separateAnimations = checkBoxSeparateAnimations.Checked,
                        doCopy = checkBoxCopy.Checked
                    };

                    buttonGenerate.Enabled = false;
                    buttonCancel.Enabled = true;
                    WriteSettings();
                    backgroundWorker2.RunWorkerAsync(args);
                }
                else
                {
                    LoggingHelper.Write(Resources.GenerationFailed, 3);
                }
            }
            else
            {
                LoggingHelper.Write(Resources.GenerationFailed, 3);
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
                LoggingHelper.Write(Resources.WrongSourceExtension, 2);
                checkSuccess = false;
            }
            if (checkBoxManual.Checked)
            {
                if (Path.GetExtension(textBoxGDT.Text) != ".lsf")
                {
                    LoggingHelper.Write(Resources.WrongGDTExtension, 2);
                    checkSuccess = false;
                }
                if (Path.GetExtension(textBoxDB.Text) != ".lsf")
                {
                    LoggingHelper.Write(Resources.WrongDBExtension, 2);
                    checkSuccess = false;
                }
                if (Path.GetExtension(textBoxD.Text) != ".lsj")
                {
                    LoggingHelper.Write(Resources.WrongDExtension, 2);
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
                LoggingHelper.Write(Resources.SourceFileDoesNotExist, 2);
                checkSuccess = false;
            }
            return checkSuccess && CheckExistenceUnpackedData() && CheckExistenceCopy() && CheckExistenceManual();
        }

        private bool CheckExistenceBatch()
        {
            bool checkSuccess = true;
            if (!Directory.Exists(textBoxBatch.Text))
            {
                LoggingHelper.Write(Resources.BatchInputDoesNotExist, 2);
                checkSuccess = false;
            }
            return checkSuccess && CheckExistenceUnpackedData() && CheckExistenceCopy();
        }

        private bool CheckExistenceUnpackedData()
        {
            bool checkSuccess = true;
            if (!Directory.Exists(Settings.Default.UnpackedDataDirectory))
            {
                LoggingHelper.Write(Resources.UnpackedDataInputDoesNotExist, 2);
                checkSuccess = false;
            }
            return checkSuccess;
        }

        private bool CheckExistenceManual()
        {
            bool checkSuccess = true;
            if (checkBoxManual.Checked)
            {
                if (!File.Exists(textBoxGDT.Text))
                {
                    LoggingHelper.Write(Resources.GDTFileDoesNotExist, 2);
                    checkSuccess = false;
                }
                if (!File.Exists(textBoxDB.Text))
                {
                    LoggingHelper.Write(Resources.DBFileDoesNotExist, 2);
                    checkSuccess = false;
                }
                if (!File.Exists(textBoxD.Text))
                {
                    LoggingHelper.Write(Resources.DFileDoesNotExist, 2);
                    checkSuccess = false;
                }
                if (!Directory.Exists(textBoxTT.Text))
                {
                    LoggingHelper.Write(Resources.TTDirectoryDoesNotExist, 2);
                }
            }
            return checkSuccess;
        }

        private bool CheckExistenceCopy()
        {
            bool checkSuccess = true;
            if (checkBoxCopy.Checked)
            {
                if (!Directory.Exists(Settings.Default.GameDataDirectory))
                {
                    LoggingHelper.Write(Resources.GameDataInputDoesNotExist, 2);
                    checkSuccess = false;
                }
                if (listBoxMods.SelectedIndex == -1)
                {
                    LoggingHelper.Write(Resources.NoModSelected, 2);
                    checkSuccess = false;
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
            FinishGeneration(e);
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BatchGenerationArgs args = (BatchGenerationArgs)e.Argument;
            e.Result = GenerationDriver.DoBatchGeneration(sender as BackgroundWorker, e, this, args.inputName, args.dataDirectory, args.outputPath, args.modName, args.separateAnimations, args.doCopy);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarBatch.Value = 0;
            labelBatchCurrentName.Visible = false;
            labelBatchCurrent.Visible = false;
            FinishGeneration(e);
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarBatch.Value = e.ProgressPercentage;
        }

        private void FinishGeneration(RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LoggingHelper.Write(Resources.GenerationError, 3);
                CleanupHelper.WriteException(e.Error);
            }
            else if (e.Cancelled)
            {
                LoggingHelper.Write(Resources.GenerationCanceled, 3);
            }
            else if ((int)e.Result > 0)
            {
                LoggingHelper.Write(Resources.GenerationError, 3);
            }
            else
                LoggingHelper.Write(Resources.GenerationFinished, 1);
            buttonGenerate.Enabled = true;
            buttonCancel.Enabled = false;
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
            textBoxSource.Text = Settings.Default.SourceFile;
            textBoxGDT.Text = Settings.Default.GeneratedDialogTimelinesFile;
            textBoxDB.Text = Settings.Default.DialogsBinaryFile;
            textBoxD.Text = Settings.Default.DialogsFile;
            textBoxTT.Text = Settings.Default.TimelineTemplatesDirectory;
            textBoxBatch.Text = Settings.Default.BatchSourceDirectory;
            checkBoxManual.Checked = Settings.Default.Manual;
            checkBoxCopy.Checked = Settings.Default.DoCopy;
            checkBoxSeparateAnimations.Checked = Settings.Default.SeparateAnimations;
            if (Settings.Default.Mods != null)
            {
                foreach (string mod in Settings.Default.Mods)
                {
                    listBoxMods.Items.Add(mod);
                }
            }
            listBoxMods.SelectedIndex = Settings.Default.ModIndex;
            tabControlMode.SelectedIndex = Settings.Default.ModeIndex;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!Settings.Default.DidFirstLoad)
            {
                ShowConfiguration();
                Settings.Default.DidFirstLoad = true;
                Settings.Default.Save();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteSettings();
        }

        private void WriteSettings()
        {
            Settings.Default.SourceFile = textBoxSource.Text;
            Settings.Default.GeneratedDialogTimelinesFile = textBoxGDT.Text;
            Settings.Default.DialogsBinaryFile = textBoxDB.Text;
            Settings.Default.DialogsFile = textBoxD.Text;
            Settings.Default.TimelineTemplatesDirectory = textBoxTT.Text;
            Settings.Default.BatchSourceDirectory = textBoxBatch.Text;
            Settings.Default.Manual = checkBoxManual.Checked;
            Settings.Default.DoCopy = checkBoxCopy.Checked;
            Settings.Default.SeparateAnimations = checkBoxSeparateAnimations.Checked;
            Settings.Default.ModIndex = listBoxMods.SelectedIndex;
            Settings.Default.ModeIndex = tabControlMode.SelectedIndex;
            StringCollection modList = [];
            foreach (object mod in listBoxMods.Items)
            {
                modList.Add(mod.ToString());
            }
            Settings.Default.Mods = modList;
            Settings.Default.Save();
        }

        private void buttonModsAdd_Click(object sender, EventArgs e)
        {
            ModSelection selection = new();
            DialogResult diaRes = selection.ShowDialog();
            if (diaRes == DialogResult.OK && selection.modName != string.Empty && !string.IsNullOrWhiteSpace(selection.modName))
            {
                if (!listBoxMods.Items.Contains(selection.modName))
                {
                    listBoxMods.Items.Add(selection.modName);
                }
                else
                {
                    LoggingHelper.Write(Resources.DuplicateMod, 2);
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

        private void tabControlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBoxManual.Enabled = !checkBoxManual.Enabled;
        }

        private void dataPathConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfiguration();
        }

        private void ShowConfiguration()
        {
            DataConfiguration config = new();
            config.ShowDialog();
            config.Dispose();
        }
    }
}
