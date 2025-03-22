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
            saveFileDialogOutput = new System.Windows.Forms.SaveFileDialog();
            label6 = new System.Windows.Forms.Label();
            buttonOutputBrowse = new System.Windows.Forms.Button();
            textBoxOutput = new System.Windows.Forms.TextBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            checkBoxSeparateAnimations = new System.Windows.Forms.CheckBox();
            formConsole = new System.Windows.Forms.RichTextBox();
            labelConsole = new System.Windows.Forms.Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            SuspendLayout();
            // 
            // buttonGenerate
            // 
            buttonGenerate.Location = new System.Drawing.Point(12, 341);
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
            // 
            // buttonSourceBrowse
            // 
            buttonSourceBrowse.Location = new System.Drawing.Point(616, 71);
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
            textBoxSource.Size = new System.Drawing.Size(598, 23);
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
            buttonGDTBrowse.Location = new System.Drawing.Point(616, 118);
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
            textBoxGDT.Location = new System.Drawing.Point(12, 118);
            textBoxGDT.Name = "textBoxGDT";
            textBoxGDT.Size = new System.Drawing.Size(598, 23);
            textBoxGDT.TabIndex = 4;
            toolTip.SetToolTip(textBoxGDT, resources.GetString("textBoxGDT.ToolTip"));
            // 
            // labelGDT
            // 
            labelGDT.AutoSize = true;
            labelGDT.Enabled = false;
            labelGDT.Location = new System.Drawing.Point(12, 100);
            labelGDT.Name = "labelGDT";
            labelGDT.Size = new System.Drawing.Size(175, 15);
            labelGDT.TabIndex = 5;
            labelGDT.Text = "Generated Dialog Timelines File:";
            // 
            // buttonDBBrowse
            // 
            buttonDBBrowse.Enabled = false;
            buttonDBBrowse.Location = new System.Drawing.Point(616, 163);
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
            textBoxDB.Location = new System.Drawing.Point(12, 163);
            textBoxDB.Name = "textBoxDB";
            textBoxDB.Size = new System.Drawing.Size(598, 23);
            textBoxDB.TabIndex = 7;
            toolTip.SetToolTip(textBoxDB, resources.GetString("textBoxDB.ToolTip"));
            // 
            // buttonTTBrowse
            // 
            buttonTTBrowse.Enabled = false;
            buttonTTBrowse.Location = new System.Drawing.Point(616, 207);
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
            labelDB.Location = new System.Drawing.Point(12, 145);
            labelDB.Name = "labelDB";
            labelDB.Size = new System.Drawing.Size(106, 15);
            labelDB.TabIndex = 8;
            labelDB.Text = "Dialogs Binary File:";
            // 
            // textBoxTT
            // 
            textBoxTT.Enabled = false;
            textBoxTT.Location = new System.Drawing.Point(12, 207);
            textBoxTT.Name = "textBoxTT";
            textBoxTT.Size = new System.Drawing.Size(598, 23);
            textBoxTT.TabIndex = 7;
            toolTip.SetToolTip(textBoxTT, resources.GetString("textBoxTT.ToolTip"));
            // 
            // labelTT
            // 
            labelTT.AutoSize = true;
            labelTT.Enabled = false;
            labelTT.Location = new System.Drawing.Point(12, 189);
            labelTT.Name = "labelTT";
            labelTT.Size = new System.Drawing.Size(162, 15);
            labelTT.TabIndex = 8;
            labelTT.Text = "Timeline Templates Directory:";
            // 
            // checkBoxManual
            // 
            checkBoxManual.AutoSize = true;
            checkBoxManual.Location = new System.Drawing.Point(12, 297);
            checkBoxManual.Name = "checkBoxManual";
            checkBoxManual.Size = new System.Drawing.Size(306, 19);
            checkBoxManual.TabIndex = 9;
            checkBoxManual.Text = "Manually select GDT file, DB file, and templates folder";
            toolTip.SetToolTip(checkBoxManual, "Select which timeline data files to use manually.");
            checkBoxManual.UseVisualStyleBackColor = true;
            checkBoxManual.CheckedChanged += checkBox1_CheckedChanged;
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
            buttonDataBrowse.Location = new System.Drawing.Point(616, 27);
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
            textBoxData.Size = new System.Drawing.Size(598, 23);
            textBoxData.TabIndex = 11;
            toolTip.SetToolTip(textBoxData, "The path to the top level of your unpacked data, such as that extracted by the BG3 Modder's Multitool.\r\nExample: F:\\BG3Multitool\\UnpackedData");
            // 
            // openFileDialogGDT
            // 
            openFileDialogGDT.Filter = "Generated Dialog Timelines Files|*.lsf";
            // 
            // openFileDialogDB
            // 
            openFileDialogDB.Filter = "Dialogs Binary Files|*.lsf";
            // 
            // saveFileDialogOutput
            // 
            saveFileDialogOutput.Filter = "Timeline Editor Source Files|*.tml";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(12, 233);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(69, 15);
            label6.TabIndex = 15;
            label6.Text = "Output File:";
            // 
            // buttonOutputBrowse
            // 
            buttonOutputBrowse.Location = new System.Drawing.Point(616, 251);
            buttonOutputBrowse.Name = "buttonOutputBrowse";
            buttonOutputBrowse.Size = new System.Drawing.Size(75, 23);
            buttonOutputBrowse.TabIndex = 13;
            buttonOutputBrowse.Text = "Browse";
            buttonOutputBrowse.UseVisualStyleBackColor = true;
            buttonOutputBrowse.Click += buttonOutputBrowse_Click;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Location = new System.Drawing.Point(12, 251);
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.Size = new System.Drawing.Size(598, 23);
            textBoxOutput.TabIndex = 14;
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
            checkBoxSeparateAnimations.Location = new System.Drawing.Point(387, 297);
            checkBoxSeparateAnimations.Name = "checkBoxSeparateAnimations";
            checkBoxSeparateAnimations.Size = new System.Drawing.Size(305, 19);
            checkBoxSeparateAnimations.TabIndex = 18;
            checkBoxSeparateAnimations.Text = "Separate overlapping animations into different tracks";
            toolTip.SetToolTip(checkBoxSeparateAnimations, "Animations will be moved to their own track if they overlap.\r\nThis can reduce crashes in the editor.");
            checkBoxSeparateAnimations.UseVisualStyleBackColor = true;
            // 
            // formConsole
            // 
            formConsole.BackColor = System.Drawing.SystemColors.InfoText;
            formConsole.Location = new System.Drawing.Point(12, 402);
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
            labelConsole.Location = new System.Drawing.Point(12, 384);
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
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(704, 566);
            Controls.Add(checkBoxSeparateAnimations);
            Controls.Add(labelConsole);
            Controls.Add(formConsole);
            Controls.Add(label6);
            Controls.Add(buttonOutputBrowse);
            Controls.Add(textBoxOutput);
            Controls.Add(labelData);
            Controls.Add(buttonDataBrowse);
            Controls.Add(textBoxData);
            Controls.Add(checkBoxManual);
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
            MaximumSize = new System.Drawing.Size(720, 605);
            MinimumSize = new System.Drawing.Size(720, 605);
            Name = "MainForm";
            Text = "TMLGen";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
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
        private System.Windows.Forms.SaveFileDialog saveFileDialogOutput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonOutputBrowse;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelConsole;
        public System.Windows.Forms.RichTextBox formConsole;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBoxSeparateAnimations;
    }
}

