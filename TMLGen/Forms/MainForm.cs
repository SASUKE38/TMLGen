using System;
using System.Collections.Generic;
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
        public delegate Guid ShowMaterialSelection(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId, string sourceNameExtensionless);
        public static ShowMaterialSelection materialSelectionDelegate;
        public delegate Guid ShowLocationSelection(HashSet<Guid> candidates, string sourceNameExtensionless);
        public static ShowLocationSelection locationSelectionDelegate;

        private static readonly uint logMax = 200u;

        public MainForm()
        {
            InitializeComponent();

            Text += " v" + Application.ProductVersion;

            LoggingHelper.Set(new(logMax), formConsole, this);

            logDelegate = new UpdateLog(UpdateLogMethod);
            materialSelectionDelegate = new ShowMaterialSelection(MaterialSelectionMethod);
            locationSelectionDelegate = new ShowLocationSelection(LocationSelectionMethod);
        }

        public void UpdateLogMethod()
        {
            formConsole.Rtf = LoggingHelper.GetOutput();
        }

        public Guid MaterialSelectionMethod(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId, string sourceNameExtensionless)
        {
            Guid selectionRes = Guid.Empty;

            if (Settings.Default.SkipSelectionPrompt)
            {
                return selectionRes;
            }

            SlotMaterialSelection selection = new(candidates, materialId, resourceId, sourceNameExtensionless);
            DialogResult diaRes = selection.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                selectionRes = selection.selected;
            }
            selection.Dispose();
            return selectionRes;
        }

        public Guid LocationSelectionMethod(HashSet<Guid> candidates, string sourceNameExtensionless)
        {
            Guid selectionRes = Guid.Empty;

            if (Settings.Default.SkipSelectionPrompt)
            {
                return selectionRes;
            }

            LocationSelection selection = new(candidates, sourceNameExtensionless);
            DialogResult diaRes = selection.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                selectionRes = selection.selected;
            }
            selection.Dispose();
            return selectionRes;
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
                buttonGenerate.Enabled = false;
                WriteSettings();
                backgroundWorker1.RunWorkerAsync();
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
                    buttonGenerate.Enabled = false;
                    buttonCancel.Enabled = true;
                    WriteSettings();
                    backgroundWorker2.RunWorkerAsync();
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
            if (!Directory.Exists(PathConfigurationSettings.Default.UnpackedDataDirectory))
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
                if (!Directory.Exists(PathConfigurationSettings.Default.GameDataDirectory))
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
            e.Result = GenerationDriver.DoGeneration(this);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FinishGeneration(e);
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GenerationDriver.DoBatchGeneration(sender as BackgroundWorker, e, this);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarBatch.Value = 0;
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
                LoggingHelper.Write(Resources.GenerationErrorNoException, 3);
            }
            else
                LoggingHelper.Write(Resources.GenerationFinished, 1);
            buttonGenerate.Enabled = true;
            buttonCancel.Enabled = false;
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
            checkBoxNoLocationSelection.Checked = Settings.Default.SkipSelectionPrompt;
            checkBoxSkipShowArmor.Checked = Settings.Default.SkipShowArmor;
            foreach (string mod in Settings.Default.Mods) listBoxMods.Items.Add(mod);
            listBoxMods.SelectedIndex = Settings.Default.ModIndex;
            tabControlMode.SelectedIndex = Settings.Default.ModeIndex;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!PathConfigurationSettings.Default.DidFirstLoad)
            {
                ShowConfiguration();
                PathConfigurationSettings.Default.DidFirstLoad = true;
                PathConfigurationSettings.Default.Save();
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
            Settings.Default.SkipSelectionPrompt = checkBoxNoLocationSelection.Checked;
            Settings.Default.SkipShowArmor = checkBoxSkipShowArmor.Checked;
            Settings.Default.ModIndex = listBoxMods.SelectedIndex;
            Settings.Default.SelectedMod = (string)listBoxMods.SelectedItem;
            Settings.Default.ModeIndex = tabControlMode.SelectedIndex;
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
                    Settings.Default.Mods.Add(selection.modName);
                    WriteSettings();
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
                Settings.Default.Mods.Remove(item);
                WriteSettings();
            }
        }

        private void listBoxMods_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void checkBoxNoLocationSelection_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.SkipSelectionPrompt = checkBoxNoLocationSelection.Checked;
            Settings.Default.Save();
        }

        private void openUnpackedDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHelper.OpenPath(PathConfigurationSettings.Default.UnpackedDataDirectory);
        }

        private void openGameDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHelper.OpenPath(PathConfigurationSettings.Default.GameDataDirectory);
        }

        private void openTimelineDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(CopyHelper.copiedDataDirectoryName))
            {
                Directory.CreateDirectory(CopyHelper.copiedDataDirectoryName);
            }
            FileHelper.OpenPath(CopyHelper.copiedDataDirectoryName);
        }
    }
}
