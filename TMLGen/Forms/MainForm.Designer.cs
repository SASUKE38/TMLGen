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
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new System.Drawing.Point(330, 402);
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
            buttonSourceBrowse.Location = new System.Drawing.Point(934, 71);
            buttonSourceBrowse.Name = "buttonSourceBrowse";
            buttonSourceBrowse.Size = new System.Drawing.Size(75, 23);
            buttonSourceBrowse.TabIndex = 0;
            buttonSourceBrowse.Text = "Browse";
            buttonSourceBrowse.UseVisualStyleBackColor = true;
            buttonSourceBrowse.Click += buttonSourceBrowse_Click;
            // 
            // textBoxSource
            // 
            textBoxSource.Location = new System.Drawing.Point(12, 71);
            textBoxSource.Name = "textBoxSource";
            textBoxSource.Size = new System.Drawing.Size(916, 23);
            textBoxSource.TabIndex = 1;
            toolTip.SetToolTip(textBoxSource, resources.GetString("textBoxSource.ToolTip"));
            // 
            // labelSource
            // 
            labelSource.AutoSize = true;
            labelSource.Location = new System.Drawing.Point(12, 53);
            labelSource.Name = "labelSource";
            labelSource.Size = new System.Drawing.Size(67, 15);
            labelSource.TabIndex = 2;
            labelSource.Text = "Source File:";
            // 
            // buttonGDTBrowse
            // 
            buttonGDTBrowse.Enabled = false;
            buttonGDTBrowse.Location = new System.Drawing.Point(934, 162);
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
            textBoxGDT.Location = new System.Drawing.Point(12, 162);
            textBoxGDT.Name = "textBoxGDT";
            textBoxGDT.Size = new System.Drawing.Size(916, 23);
            textBoxGDT.TabIndex = 4;
            toolTip.SetToolTip(textBoxGDT, resources.GetString("textBoxGDT.ToolTip"));
            // 
            // labelGDT
            // 
            labelGDT.AutoSize = true;
            labelGDT.Enabled = false;
            labelGDT.Location = new System.Drawing.Point(12, 144);
            labelGDT.Name = "labelGDT";
            labelGDT.Size = new System.Drawing.Size(175, 15);
            labelGDT.TabIndex = 5;
            labelGDT.Text = "Generated Dialog Timelines File:";
            // 
            // buttonDBBrowse
            // 
            buttonDBBrowse.Enabled = false;
            buttonDBBrowse.Location = new System.Drawing.Point(934, 207);
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
            textBoxDB.Location = new System.Drawing.Point(12, 207);
            textBoxDB.Name = "textBoxDB";
            textBoxDB.Size = new System.Drawing.Size(916, 23);
            textBoxDB.TabIndex = 7;
            toolTip.SetToolTip(textBoxDB, resources.GetString("textBoxDB.ToolTip"));
            // 
            // buttonTTBrowse
            // 
            buttonTTBrowse.Enabled = false;
            buttonTTBrowse.Location = new System.Drawing.Point(934, 295);
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
            labelDB.Location = new System.Drawing.Point(12, 189);
            labelDB.Name = "labelDB";
            labelDB.Size = new System.Drawing.Size(106, 15);
            labelDB.TabIndex = 8;
            labelDB.Text = "Dialogs Binary File:";
            // 
            // textBoxTT
            // 
            textBoxTT.Enabled = false;
            textBoxTT.Location = new System.Drawing.Point(12, 295);
            textBoxTT.Name = "textBoxTT";
            textBoxTT.Size = new System.Drawing.Size(916, 23);
            textBoxTT.TabIndex = 7;
            toolTip.SetToolTip(textBoxTT, resources.GetString("textBoxTT.ToolTip"));
            // 
            // labelTT
            // 
            labelTT.AutoSize = true;
            labelTT.Enabled = false;
            labelTT.Location = new System.Drawing.Point(12, 277);
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
            buttonDataBrowse.Location = new System.Drawing.Point(934, 27);
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
            textBoxData.Size = new System.Drawing.Size(916, 23);
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
            labelGameData.Location = new System.Drawing.Point(12, 97);
            labelGameData.Name = "labelGameData";
            labelGameData.Size = new System.Drawing.Size(119, 15);
            labelGameData.TabIndex = 15;
            labelGameData.Text = "Game Data Directory:";
            // 
            // buttonGameDataBrowse
            // 
            buttonGameDataBrowse.Enabled = false;
            buttonGameDataBrowse.Location = new System.Drawing.Point(934, 115);
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
            textBoxGameData.Location = new System.Drawing.Point(12, 115);
            textBoxGameData.Name = "textBoxGameData";
            textBoxGameData.Size = new System.Drawing.Size(916, 23);
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
            toolTip.SetToolTip(checkBoxSeparateAnimations, "Animations will be moved to their own track if they overlap.\r\nThis can reduce crashes in the editor.");
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
            textBoxD.Location = new System.Drawing.Point(12, 251);
            textBoxD.Name = "textBoxD";
            textBoxD.Size = new System.Drawing.Size(916, 23);
            textBoxD.TabIndex = 28;
            toolTip.SetToolTip(textBoxD, resources.GetString("textBoxD.ToolTip"));
            // 
            // formConsole
            // 
            formConsole.BackColor = System.Drawing.SystemColors.InfoText;
            formConsole.Location = new System.Drawing.Point(330, 463);
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
            labelConsole.Location = new System.Drawing.Point(330, 445);
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
            groupBox1.Location = new System.Drawing.Point(330, 324);
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
            listBoxMods.Location = new System.Drawing.Point(12, 339);
            listBoxMods.Name = "listBoxMods";
            listBoxMods.Size = new System.Drawing.Size(312, 244);
            listBoxMods.TabIndex = 20;
            listBoxMods.SelectedIndexChanged += listBoxMods_SelectedIndexChanged;
            // 
            // buttonModsAdd
            // 
            buttonModsAdd.Location = new System.Drawing.Point(207, 593);
            buttonModsAdd.Name = "buttonModsAdd";
            buttonModsAdd.Size = new System.Drawing.Size(117, 23);
            buttonModsAdd.TabIndex = 24;
            buttonModsAdd.Text = "Add Mod";
            buttonModsAdd.UseVisualStyleBackColor = true;
            buttonModsAdd.Click += buttonModsAdd_Click;
            // 
            // buttonModsRemove
            // 
            buttonModsRemove.Location = new System.Drawing.Point(12, 593);
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
            label1.Location = new System.Drawing.Point(12, 321);
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
            labelD.Location = new System.Drawing.Point(12, 233);
            labelD.Name = "labelD";
            labelD.Size = new System.Drawing.Size(70, 15);
            labelD.TabIndex = 29;
            labelD.Text = "Dialogs File:";
            // 
            // buttonDBrowse
            // 
            buttonDBrowse.Enabled = false;
            buttonDBrowse.Location = new System.Drawing.Point(934, 251);
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
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1022, 629);
            Controls.Add(labelD);
            Controls.Add(textBoxD);
            Controls.Add(buttonDBrowse);
            Controls.Add(label1);
            Controls.Add(buttonModsRemove);
            Controls.Add(buttonModsAdd);
            Controls.Add(listBoxMods);
            Controls.Add(groupBox1);
            Controls.Add(labelConsole);
            Controls.Add(formConsole);
            Controls.Add(labelGameData);
            Controls.Add(buttonGameDataBrowse);
            Controls.Add(textBoxGameData);
            Controls.Add(labelData);
            Controls.Add(buttonDataBrowse);
            Controls.Add(textBoxData);
            Controls.Add(labelTT);
            Controls.Add(textBoxTT);
            Controls.Add(buttonGenerate);
            Controls.Add(labelDB);
            Controls.Add(labelSource);
            Controls.Add(buttonTTBrowse);
            Controls.Add(buttonSourceBrowse);
            Controls.Add(textBoxDB);
            Controls.Add(textBoxSource);
            Controls.Add(buttonDBBrowse);
            Controls.Add(buttonGDTBrowse);
            Controls.Add(labelGDT);
            Controls.Add(textBoxGDT);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new System.Drawing.Size(720, 605);
            Name = "MainForm";
            Text = "TMLGen";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
    }
}

