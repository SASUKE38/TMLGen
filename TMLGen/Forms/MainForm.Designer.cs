namespace TMLGen
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            buttonGenerate = new System.Windows.Forms.Button();
            openFileDialogSource = new System.Windows.Forms.OpenFileDialog();
            buttonSourceBrowse = new System.Windows.Forms.Button();
            textBoxSource = new System.Windows.Forms.TextBox();
            labelSource = new System.Windows.Forms.Label();
            buttonGDTBrowse = new System.Windows.Forms.Button();
            textBoxGDT = new System.Windows.Forms.TextBox();
            labelGDT = new System.Windows.Forms.Label();
            buttonDBBrowse = new System.Windows.Forms.Button();
            textBoxDB = new System.Windows.Forms.TextBox();
            buttonTTBrowse = new System.Windows.Forms.Button();
            labelDB = new System.Windows.Forms.Label();
            textBoxTT = new System.Windows.Forms.TextBox();
            labelTT = new System.Windows.Forms.Label();
            checkBoxManual = new System.Windows.Forms.CheckBox();
            labelData = new System.Windows.Forms.Label();
            buttonDataBrowse = new System.Windows.Forms.Button();
            textBoxData = new System.Windows.Forms.TextBox();
            openFileDialogGDT = new System.Windows.Forms.OpenFileDialog();
            openFileDialogDB = new System.Windows.Forms.OpenFileDialog();
            folderBrowserDialogData = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialogTemplates = new System.Windows.Forms.FolderBrowserDialog();
            labelGameData = new System.Windows.Forms.Label();
            buttonGameDataBrowse = new System.Windows.Forms.Button();
            textBoxGameData = new System.Windows.Forms.TextBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            checkBoxSeparateAnimations = new System.Windows.Forms.CheckBox();
            checkBoxCopy = new System.Windows.Forms.CheckBox();
            textBoxD = new System.Windows.Forms.TextBox();
            textBoxBatch = new System.Windows.Forms.TextBox();
            formConsole = new System.Windows.Forms.RichTextBox();
            labelConsole = new System.Windows.Forms.Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            groupBox1 = new System.Windows.Forms.GroupBox();
            listBoxMods = new System.Windows.Forms.ListBox();
            buttonModsAdd = new System.Windows.Forms.Button();
            buttonModsRemove = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            folderBrowserDialogGameData = new System.Windows.Forms.FolderBrowserDialog();
            labelD = new System.Windows.Forms.Label();
            buttonDBrowse = new System.Windows.Forms.Button();
            openFileDialogD = new System.Windows.Forms.OpenFileDialog();
            tabControlMode = new System.Windows.Forms.TabControl();
            tabPageSingle = new System.Windows.Forms.TabPage();
            tabPageBatch = new System.Windows.Forms.TabPage();
            labelBatchCurrentName = new System.Windows.Forms.Label();
            labelBatchCurrent = new System.Windows.Forms.Label();
            labelBatchProgress = new System.Windows.Forms.Label();
            progressBarBatch = new System.Windows.Forms.ProgressBar();
            buttonBatch = new System.Windows.Forms.Button();
            labelBatch = new System.Windows.Forms.Label();
            folderBrowserDialogBatchSource = new System.Windows.Forms.FolderBrowserDialog();
            backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            groupBox1.SuspendLayout();
            tabControlMode.SuspendLayout();
            tabPageSingle.SuspendLayout();
            tabPageBatch.SuspendLayout();
            SuspendLayout();
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new System.Drawing.Point(330, 445);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new System.Drawing.Size(679, 40);
            buttonGenerate.TabIndex = 0;
            buttonGenerate.Text = "Generate";
            buttonGenerate.UseVisualStyleBackColor = true;
            buttonGenerate.Click += buttonGenerate_Click;
            // 
            // openFileDialogSource
            // 
            openFileDialogSource.DefaultExt = "lsf";
            openFileDialogSource.Filter = "Timeline Source Files|*.lsf";
            openFileDialogSource.Title = "Choose Source File";
            // 
            // buttonSourceBrowse
            // 
            buttonSourceBrowse.Location = new System.Drawing.Point(904, 21);
            buttonSourceBrowse.Name = "buttonSourceBrowse";
            buttonSourceBrowse.Size = new System.Drawing.Size(75, 23);
            buttonSourceBrowse.TabIndex = 0;
            buttonSourceBrowse.Text = "Browse";
            buttonSourceBrowse.UseVisualStyleBackColor = true;
            buttonSourceBrowse.Click += buttonSourceBrowse_Click;
            // 
            // textBoxSource
            // 
            textBoxSource.Location = new System.Drawing.Point(6, 21);
            textBoxSource.Name = "textBoxSource";
            textBoxSource.Size = new System.Drawing.Size(892, 23);
            textBoxSource.TabIndex = 1;
            toolTip.SetToolTip(textBoxSource, resources.GetString("textBoxSource.ToolTip"));
            // 
            // labelSource
            // 
            labelSource.AutoSize = true;
            labelSource.Location = new System.Drawing.Point(6, 3);
            labelSource.Name = "labelSource";
            labelSource.Size = new System.Drawing.Size(67, 15);
            labelSource.TabIndex = 2;
            labelSource.Text = "Source File:";
            // 
            // buttonGDTBrowse
            // 
            buttonGDTBrowse.Enabled = false;
            buttonGDTBrowse.Location = new System.Drawing.Point(904, 65);
            buttonGDTBrowse.Name = "buttonGDTBrowse";
            buttonGDTBrowse.Size = new System.Drawing.Size(75, 23);
            buttonGDTBrowse.TabIndex = 3;
            buttonGDTBrowse.Text = "Browse";
            buttonGDTBrowse.UseVisualStyleBackColor = true;
            buttonGDTBrowse.Click += buttonGDTBrowse_Click;
            // 
            // textBoxGDT
            // 
            textBoxGDT.Enabled = false;
            textBoxGDT.Location = new System.Drawing.Point(6, 65);
            textBoxGDT.Name = "textBoxGDT";
            textBoxGDT.Size = new System.Drawing.Size(892, 23);
            textBoxGDT.TabIndex = 4;
            toolTip.SetToolTip(textBoxGDT, resources.GetString("textBoxGDT.ToolTip"));
            // 
            // labelGDT
            // 
            labelGDT.AutoSize = true;
            labelGDT.Enabled = false;
            labelGDT.Location = new System.Drawing.Point(6, 47);
            labelGDT.Name = "labelGDT";
            labelGDT.Size = new System.Drawing.Size(175, 15);
            labelGDT.TabIndex = 5;
            labelGDT.Text = "Generated Dialog Timelines File:";
            // 
            // buttonDBBrowse
            // 
            buttonDBBrowse.Enabled = false;
            buttonDBBrowse.Location = new System.Drawing.Point(904, 110);
            buttonDBBrowse.Name = "buttonDBBrowse";
            buttonDBBrowse.Size = new System.Drawing.Size(75, 23);
            buttonDBBrowse.TabIndex = 6;
            buttonDBBrowse.Text = "Browse";
            buttonDBBrowse.UseVisualStyleBackColor = true;
            buttonDBBrowse.Click += buttonDBBrowse_Click;
            // 
            // textBoxDB
            // 
            textBoxDB.Enabled = false;
            textBoxDB.Location = new System.Drawing.Point(6, 110);
            textBoxDB.Name = "textBoxDB";
            textBoxDB.Size = new System.Drawing.Size(892, 23);
            textBoxDB.TabIndex = 7;
            toolTip.SetToolTip(textBoxDB, resources.GetString("textBoxDB.ToolTip"));
            // 
            // buttonTTBrowse
            // 
            buttonTTBrowse.Enabled = false;
            buttonTTBrowse.Location = new System.Drawing.Point(904, 198);
            buttonTTBrowse.Name = "buttonTTBrowse";
            buttonTTBrowse.Size = new System.Drawing.Size(75, 23);
            buttonTTBrowse.TabIndex = 6;
            buttonTTBrowse.Text = "Browse";
            buttonTTBrowse.UseVisualStyleBackColor = true;
            buttonTTBrowse.Click += buttonTTBrowse_Click;
            // 
            // labelDB
            // 
            labelDB.AutoSize = true;
            labelDB.Enabled = false;
            labelDB.Location = new System.Drawing.Point(6, 92);
            labelDB.Name = "labelDB";
            labelDB.Size = new System.Drawing.Size(106, 15);
            labelDB.TabIndex = 8;
            labelDB.Text = "Dialogs Binary File:";
            // 
            // textBoxTT
            // 
            textBoxTT.Enabled = false;
            textBoxTT.Location = new System.Drawing.Point(6, 198);
            textBoxTT.Name = "textBoxTT";
            textBoxTT.Size = new System.Drawing.Size(892, 23);
            textBoxTT.TabIndex = 7;
            toolTip.SetToolTip(textBoxTT, resources.GetString("textBoxTT.ToolTip"));
            // 
            // labelTT
            // 
            labelTT.AutoSize = true;
            labelTT.Enabled = false;
            labelTT.Location = new System.Drawing.Point(6, 180);
            labelTT.Name = "labelTT";
            labelTT.Size = new System.Drawing.Size(162, 15);
            labelTT.TabIndex = 8;
            labelTT.Text = "Timeline Templates Directory:";
            // 
            // checkBoxManual
            // 
            checkBoxManual.AutoSize = true;
            checkBoxManual.Location = new System.Drawing.Point(6, 22);
            checkBoxManual.Name = "checkBoxManual";
            checkBoxManual.Size = new System.Drawing.Size(204, 19);
            checkBoxManual.TabIndex = 9;
            checkBoxManual.Text = "Manually select timeline data files";
            toolTip.SetToolTip(checkBoxManual, "Select which timeline data files to use manually.");
            checkBoxManual.UseVisualStyleBackColor = true;
            checkBoxManual.CheckedChanged += checkBoxManual_CheckedChanged;
            // 
            // labelData
            // 
            labelData.AutoSize = true;
            labelData.Location = new System.Drawing.Point(12, 9);
            labelData.Name = "labelData";
            labelData.Size = new System.Drawing.Size(141, 15);
            labelData.TabIndex = 12;
            labelData.Text = "Unpacked Data Directory:";
            // 
            // buttonDataBrowse
            // 
            buttonDataBrowse.Location = new System.Drawing.Point(920, 27);
            buttonDataBrowse.Name = "buttonDataBrowse";
            buttonDataBrowse.Size = new System.Drawing.Size(75, 23);
            buttonDataBrowse.TabIndex = 10;
            buttonDataBrowse.Text = "Browse";
            buttonDataBrowse.UseVisualStyleBackColor = true;
            buttonDataBrowse.Click += buttonDataBrowse_Click;
            // 
            // textBoxData
            // 
            textBoxData.Location = new System.Drawing.Point(12, 27);
            textBoxData.Name = "textBoxData";
            textBoxData.Size = new System.Drawing.Size(902, 23);
            textBoxData.TabIndex = 11;
            toolTip.SetToolTip(textBoxData, "The path to the top level of your unpacked data, such as that extracted by the BG3 Modder's Multitool.\r\nExample: F:\\BG3Multitool\\UnpackedData");
            // 
            // openFileDialogGDT
            // 
            openFileDialogGDT.Filter = "Generated Dialog Timelines Files|*.lsf";
            openFileDialogGDT.Title = "Choose Generated Dialog Timelines File";
            // 
            // openFileDialogDB
            // 
            openFileDialogDB.Filter = "Dialogs Binary Files|*.lsf";
            openFileDialogDB.Title = "Choose Dialogs Binary File";
            // 
            // folderBrowserDialogData
            // 
            folderBrowserDialogData.Description = "Choose Unpacked Data Directory";
            folderBrowserDialogData.UseDescriptionForTitle = true;
            // 
            // folderBrowserDialogTemplates
            // 
            folderBrowserDialogTemplates.Description = "Choose Timeline Templates Folder";
            folderBrowserDialogTemplates.UseDescriptionForTitle = true;
            // 
            // labelGameData
            // 
            labelGameData.AutoSize = true;
            labelGameData.Enabled = false;
            labelGameData.Location = new System.Drawing.Point(12, 53);
            labelGameData.Name = "labelGameData";
            labelGameData.Size = new System.Drawing.Size(119, 15);
            labelGameData.TabIndex = 15;
            labelGameData.Text = "Game Data Directory:";
            // 
            // buttonGameDataBrowse
            // 
            buttonGameDataBrowse.Enabled = false;
            buttonGameDataBrowse.Location = new System.Drawing.Point(920, 71);
            buttonGameDataBrowse.Name = "buttonGameDataBrowse";
            buttonGameDataBrowse.Size = new System.Drawing.Size(75, 23);
            buttonGameDataBrowse.TabIndex = 13;
            buttonGameDataBrowse.Text = "Browse";
            buttonGameDataBrowse.UseVisualStyleBackColor = true;
            buttonGameDataBrowse.Click += buttonGameDataBrowse_Click;
            // 
            // textBoxGameData
            // 
            textBoxGameData.Enabled = false;
            textBoxGameData.Location = new System.Drawing.Point(12, 71);
            textBoxGameData.Name = "textBoxGameData";
            textBoxGameData.Size = new System.Drawing.Size(902, 23);
            textBoxGameData.TabIndex = 14;
            toolTip.SetToolTip(textBoxGameData, "The path to the directory that contains the game's .pak files. Likely ends with Baldurs Gate 3\\Data.\r\nExample: F:\\SteamLibrary\\steamapps\\common\\Baldurs Gate 3\\Data");
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 10;
            // 
            // checkBoxSeparateAnimations
            // 
            checkBoxSeparateAnimations.AutoSize = true;
            checkBoxSeparateAnimations.Checked = true;
            checkBoxSeparateAnimations.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxSeparateAnimations.Location = new System.Drawing.Point(368, 22);
            checkBoxSeparateAnimations.Name = "checkBoxSeparateAnimations";
            checkBoxSeparateAnimations.Size = new System.Drawing.Size(305, 19);
            checkBoxSeparateAnimations.TabIndex = 18;
            checkBoxSeparateAnimations.Text = "Separate overlapping animations into different tracks";
            toolTip.SetToolTip(checkBoxSeparateAnimations, "Animations will be moved to their own track if they overlap.\r\nThis reduces crashes in the editor but could also reduce the accuracy of the recreation.\r\nSee the README for more information.");
            checkBoxSeparateAnimations.UseVisualStyleBackColor = true;
            // 
            // checkBoxCopy
            // 
            checkBoxCopy.AutoSize = true;
            checkBoxCopy.Location = new System.Drawing.Point(6, 47);
            checkBoxCopy.Name = "checkBoxCopy";
            checkBoxCopy.Size = new System.Drawing.Size(204, 19);
            checkBoxCopy.TabIndex = 19;
            checkBoxCopy.Text = "Override timeline in selected mod";
            toolTip.SetToolTip(checkBoxCopy, "Will copy the timeline's data files to the selected mod instead of to the Timeline Data directory.");
            checkBoxCopy.UseVisualStyleBackColor = true;
            checkBoxCopy.CheckedChanged += checkBoxCopy_CheckedChanged;
            // 
            // textBoxD
            // 
            textBoxD.Enabled = false;
            textBoxD.Location = new System.Drawing.Point(6, 154);
            textBoxD.Name = "textBoxD";
            textBoxD.Size = new System.Drawing.Size(892, 23);
            textBoxD.TabIndex = 28;
            toolTip.SetToolTip(textBoxD, resources.GetString("textBoxD.ToolTip"));
            // 
            // textBoxBatch
            // 
            textBoxBatch.Location = new System.Drawing.Point(6, 21);
            textBoxBatch.Name = "textBoxBatch";
            textBoxBatch.Size = new System.Drawing.Size(892, 23);
            textBoxBatch.TabIndex = 4;
            toolTip.SetToolTip(textBoxBatch, resources.GetString("textBoxBatch.ToolTip"));
            // 
            // formConsole
            // 
            formConsole.BackColor = System.Drawing.SystemColors.InfoText;
            formConsole.Location = new System.Drawing.Point(330, 506);
            formConsole.Name = "formConsole";
            formConsole.ReadOnly = true;
            formConsole.Size = new System.Drawing.Size(679, 153);
            formConsole.TabIndex = 16;
            formConsole.Text = "";
            formConsole.TextChanged += formConsole_TextChanged;
            // 
            // labelConsole
            // 
            labelConsole.AutoSize = true;
            labelConsole.Location = new System.Drawing.Point(330, 488);
            labelConsole.Name = "labelConsole";
            labelConsole.Size = new System.Drawing.Size(30, 15);
            labelConsole.TabIndex = 17;
            labelConsole.Text = "Log:";
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBoxCopy);
            groupBox1.Controls.Add(checkBoxManual);
            groupBox1.Controls.Add(checkBoxSeparateAnimations);
            groupBox1.Location = new System.Drawing.Point(330, 367);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(679, 72);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // listBoxMods
            // 
            listBoxMods.FormattingEnabled = true;
            listBoxMods.ItemHeight = 15;
            listBoxMods.Location = new System.Drawing.Point(12, 382);
            listBoxMods.Name = "listBoxMods";
            listBoxMods.Size = new System.Drawing.Size(312, 244);
            listBoxMods.TabIndex = 20;
            listBoxMods.SelectedIndexChanged += listBoxMods_SelectedIndexChanged;
            // 
            // buttonModsAdd
            // 
            buttonModsAdd.Location = new System.Drawing.Point(12, 636);
            buttonModsAdd.Name = "buttonModsAdd";
            buttonModsAdd.Size = new System.Drawing.Size(117, 23);
            buttonModsAdd.TabIndex = 24;
            buttonModsAdd.Text = "Add Mod";
            buttonModsAdd.UseVisualStyleBackColor = true;
            buttonModsAdd.Click += buttonModsAdd_Click;
            // 
            // buttonModsRemove
            // 
            buttonModsRemove.Location = new System.Drawing.Point(207, 636);
            buttonModsRemove.Name = "buttonModsRemove";
            buttonModsRemove.Size = new System.Drawing.Size(117, 23);
            buttonModsRemove.TabIndex = 25;
            buttonModsRemove.Text = "Remove Mod";
            buttonModsRemove.UseVisualStyleBackColor = true;
            buttonModsRemove.Click += buttonModsRemove_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 364);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(40, 15);
            label1.TabIndex = 26;
            label1.Text = "Mods:";
            // 
            // folderBrowserDialogGameData
            // 
            folderBrowserDialogGameData.Description = "Select Game Data Directory";
            folderBrowserDialogGameData.UseDescriptionForTitle = true;
            // 
            // labelD
            // 
            labelD.AutoSize = true;
            labelD.Enabled = false;
            labelD.Location = new System.Drawing.Point(6, 136);
            labelD.Name = "labelD";
            labelD.Size = new System.Drawing.Size(70, 15);
            labelD.TabIndex = 29;
            labelD.Text = "Dialogs File:";
            // 
            // buttonDBrowse
            // 
            buttonDBrowse.Enabled = false;
            buttonDBrowse.Location = new System.Drawing.Point(904, 154);
            buttonDBrowse.Name = "buttonDBrowse";
            buttonDBrowse.Size = new System.Drawing.Size(75, 23);
            buttonDBrowse.TabIndex = 27;
            buttonDBrowse.Text = "Browse";
            buttonDBrowse.UseVisualStyleBackColor = true;
            buttonDBrowse.Click += buttonDBrowse_Click;
            // 
            // openFileDialogD
            // 
            openFileDialogD.Filter = "Dialogs Files|*.lsj";
            openFileDialogD.Title = "Choose Dialogs File";
            // 
            // tabControlMode
            // 
            tabControlMode.Controls.Add(tabPageSingle);
            tabControlMode.Controls.Add(tabPageBatch);
            tabControlMode.Location = new System.Drawing.Point(12, 100);
            tabControlMode.Name = "tabControlMode";
            tabControlMode.SelectedIndex = 0;
            tabControlMode.Size = new System.Drawing.Size(997, 261);
            tabControlMode.TabIndex = 30;
            tabControlMode.SelectedIndexChanged += tabControlMode_SelectedIndexChanged;
            // 
            // tabPageSingle
            // 
            tabPageSingle.Controls.Add(labelD);
            tabPageSingle.Controls.Add(textBoxGDT);
            tabPageSingle.Controls.Add(textBoxD);
            tabPageSingle.Controls.Add(labelGDT);
            tabPageSingle.Controls.Add(buttonDBrowse);
            tabPageSingle.Controls.Add(buttonGDTBrowse);
            tabPageSingle.Controls.Add(buttonDBBrowse);
            tabPageSingle.Controls.Add(textBoxSource);
            tabPageSingle.Controls.Add(textBoxDB);
            tabPageSingle.Controls.Add(buttonSourceBrowse);
            tabPageSingle.Controls.Add(buttonTTBrowse);
            tabPageSingle.Controls.Add(labelSource);
            tabPageSingle.Controls.Add(labelDB);
            tabPageSingle.Controls.Add(textBoxTT);
            tabPageSingle.Controls.Add(labelTT);
            tabPageSingle.Location = new System.Drawing.Point(4, 24);
            tabPageSingle.Name = "tabPageSingle";
            tabPageSingle.Padding = new System.Windows.Forms.Padding(3);
            tabPageSingle.Size = new System.Drawing.Size(989, 233);
            tabPageSingle.TabIndex = 0;
            tabPageSingle.Text = "Single";
            tabPageSingle.UseVisualStyleBackColor = true;
            // 
            // tabPageBatch
            // 
            tabPageBatch.Controls.Add(labelBatchCurrentName);
            tabPageBatch.Controls.Add(labelBatchCurrent);
            tabPageBatch.Controls.Add(labelBatchProgress);
            tabPageBatch.Controls.Add(progressBarBatch);
            tabPageBatch.Controls.Add(textBoxBatch);
            tabPageBatch.Controls.Add(buttonBatch);
            tabPageBatch.Controls.Add(labelBatch);
            tabPageBatch.Location = new System.Drawing.Point(4, 24);
            tabPageBatch.Name = "tabPageBatch";
            tabPageBatch.Padding = new System.Windows.Forms.Padding(3);
            tabPageBatch.Size = new System.Drawing.Size(989, 233);
            tabPageBatch.TabIndex = 1;
            tabPageBatch.Text = "Batch";
            tabPageBatch.UseVisualStyleBackColor = true;
            // 
            // labelBatchCurrentName
            // 
            labelBatchCurrentName.AutoSize = true;
            labelBatchCurrentName.Location = new System.Drawing.Point(75, 91);
            labelBatchCurrentName.Name = "labelBatchCurrentName";
            labelBatchCurrentName.Size = new System.Drawing.Size(36, 15);
            labelBatchCurrentName.TabIndex = 9;
            labelBatchCurrentName.Text = "None";
            labelBatchCurrentName.Visible = false;
            // 
            // labelBatchCurrent
            // 
            labelBatchCurrent.AutoSize = true;
            labelBatchCurrent.Location = new System.Drawing.Point(6, 91);
            labelBatchCurrent.Name = "labelBatchCurrent";
            labelBatchCurrent.Size = new System.Drawing.Size(71, 15);
            labelBatchCurrent.TabIndex = 8;
            labelBatchCurrent.Text = "Current File:";
            labelBatchCurrent.Visible = false;
            // 
            // labelBatchProgress
            // 
            labelBatchProgress.AutoSize = true;
            labelBatchProgress.Location = new System.Drawing.Point(6, 47);
            labelBatchProgress.Name = "labelBatchProgress";
            labelBatchProgress.Size = new System.Drawing.Size(55, 15);
            labelBatchProgress.TabIndex = 7;
            labelBatchProgress.Text = "Progress:";
            // 
            // progressBarBatch
            // 
            progressBarBatch.Location = new System.Drawing.Point(6, 65);
            progressBarBatch.Name = "progressBarBatch";
            progressBarBatch.Size = new System.Drawing.Size(973, 23);
            progressBarBatch.Step = 1;
            progressBarBatch.TabIndex = 6;
            // 
            // buttonBatch
            // 
            buttonBatch.Location = new System.Drawing.Point(904, 21);
            buttonBatch.Name = "buttonBatch";
            buttonBatch.Size = new System.Drawing.Size(75, 23);
            buttonBatch.TabIndex = 3;
            buttonBatch.Text = "Browse";
            buttonBatch.UseVisualStyleBackColor = true;
            buttonBatch.Click += buttonBatch_Click;
            // 
            // labelBatch
            // 
            labelBatch.AutoSize = true;
            labelBatch.Location = new System.Drawing.Point(6, 3);
            labelBatch.Name = "labelBatch";
            labelBatch.Size = new System.Drawing.Size(97, 15);
            labelBatch.TabIndex = 5;
            labelBatch.Text = "Source Directory:";
            // 
            // folderBrowserDialogBatchSource
            // 
            folderBrowserDialogBatchSource.Description = "Choose Batch Source Directory";
            folderBrowserDialogBatchSource.UseDescriptionForTitle = true;
            // 
            // backgroundWorker2
            // 
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.DoWork += backgroundWorker2_DoWork;
            backgroundWorker2.ProgressChanged += backgroundWorker2_ProgressChanged;
            backgroundWorker2.RunWorkerCompleted += backgroundWorker2_RunWorkerCompleted;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1020, 672);
            Controls.Add(labelData);
            Controls.Add(tabControlMode);
            Controls.Add(label1);
            Controls.Add(buttonModsRemove);
            Controls.Add(buttonModsAdd);
            Controls.Add(listBoxMods);
            Controls.Add(groupBox1);
            Controls.Add(labelConsole);
            Controls.Add(formConsole);
            Controls.Add(buttonGenerate);
            Controls.Add(buttonDataBrowse);
            Controls.Add(textBoxData);
            Controls.Add(labelGameData);
            Controls.Add(textBoxGameData);
            Controls.Add(buttonGameDataBrowse);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new System.Drawing.Size(720, 605);
            Name = "MainForm";
            Text = "TMLGen";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControlMode.ResumeLayout(false);
            tabPageSingle.ResumeLayout(false);
            tabPageSingle.PerformLayout();
            tabPageBatch.ResumeLayout(false);
            tabPageBatch.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.OpenFileDialog openFileDialogSource;
        private System.Windows.Forms.Button buttonSourceBrowse;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Button buttonGDTBrowse;
        private System.Windows.Forms.TextBox textBoxGDT;
        private System.Windows.Forms.Label labelGDT;
        private System.Windows.Forms.Button buttonDBBrowse;
        private System.Windows.Forms.TextBox textBoxDB;
        private System.Windows.Forms.Button buttonTTBrowse;
        private System.Windows.Forms.Label labelDB;
        private System.Windows.Forms.TextBox textBoxTT;
        private System.Windows.Forms.Label labelTT;
        private System.Windows.Forms.CheckBox checkBoxManual;
        private System.Windows.Forms.Label labelData;
        private System.Windows.Forms.Button buttonDataBrowse;
        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.OpenFileDialog openFileDialogGDT;
        private System.Windows.Forms.OpenFileDialog openFileDialogDB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogTemplates;
        private System.Windows.Forms.Label labelGameData;
        private System.Windows.Forms.Button buttonGameDataBrowse;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelConsole;
        public System.Windows.Forms.RichTextBox formConsole;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBoxSeparateAnimations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxCopy;
        private System.Windows.Forms.ListBox listBoxMods;
        private System.Windows.Forms.TextBox textBoxGameData;
        private System.Windows.Forms.Button buttonModsAdd;
        private System.Windows.Forms.Button buttonModsRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogGameData;
        private System.Windows.Forms.Label labelD;
        private System.Windows.Forms.TextBox textBoxD;
        private System.Windows.Forms.Button buttonDBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialogD;
        private System.Windows.Forms.TabControl tabControlMode;
        private System.Windows.Forms.TabPage tabPageSingle;
        private System.Windows.Forms.TabPage tabPageBatch;
        private System.Windows.Forms.TextBox textBoxBatch;
        private System.Windows.Forms.Button buttonBatch;
        private System.Windows.Forms.Label labelBatch;
        private System.Windows.Forms.ProgressBar progressBarBatch;
        private System.Windows.Forms.Label labelBatchProgress;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBatchSource;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Label labelBatchCurrent;
        private System.Windows.Forms.Label labelBatchCurrentName;
    }
}

