namespace TMLGen.Forms
{
    partial class DataConfiguration
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
            labelData = new System.Windows.Forms.Label();
            buttonDataBrowse = new System.Windows.Forms.Button();
            textBoxData = new System.Windows.Forms.TextBox();
            labelGameData = new System.Windows.Forms.Label();
            textBoxGameData = new System.Windows.Forms.TextBox();
            buttonGameDataBrowse = new System.Windows.Forms.Button();
            buttonOK = new System.Windows.Forms.Button();
            folderBrowserDialogData = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialogGameData = new System.Windows.Forms.FolderBrowserDialog();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // labelData
            // 
            labelData.AutoSize = true;
            labelData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelData.Location = new System.Drawing.Point(12, 9);
            labelData.Name = "labelData";
            labelData.Size = new System.Drawing.Size(141, 15);
            labelData.TabIndex = 18;
            labelData.Text = "Unpacked Data Directory:";
            // 
            // buttonDataBrowse
            // 
            buttonDataBrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonDataBrowse.Location = new System.Drawing.Point(508, 27);
            buttonDataBrowse.Name = "buttonDataBrowse";
            buttonDataBrowse.Size = new System.Drawing.Size(75, 23);
            buttonDataBrowse.TabIndex = 16;
            buttonDataBrowse.Text = "Browse";
            buttonDataBrowse.UseVisualStyleBackColor = true;
            buttonDataBrowse.Click += buttonDataBrowse_Click;
            // 
            // textBoxData
            // 
            textBoxData.Location = new System.Drawing.Point(12, 27);
            textBoxData.Name = "textBoxData";
            textBoxData.Size = new System.Drawing.Size(490, 23);
            textBoxData.TabIndex = 17;
            toolTip1.SetToolTip(textBoxData, "The path to the top level of your unpacked data, such as that extracted by the BG3 Modder's Multitool.");
            // 
            // labelGameData
            // 
            labelGameData.AutoSize = true;
            labelGameData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelGameData.Location = new System.Drawing.Point(12, 53);
            labelGameData.Name = "labelGameData";
            labelGameData.Size = new System.Drawing.Size(119, 15);
            labelGameData.TabIndex = 21;
            labelGameData.Text = "Game Data Directory:";
            // 
            // textBoxGameData
            // 
            textBoxGameData.Location = new System.Drawing.Point(12, 71);
            textBoxGameData.Name = "textBoxGameData";
            textBoxGameData.Size = new System.Drawing.Size(490, 23);
            textBoxGameData.TabIndex = 20;
            toolTip1.SetToolTip(textBoxGameData, "The path to the directory that contains the game's .pak files. Likely ends with Baldurs Gate 3\\Data.");
            // 
            // buttonGameDataBrowse
            // 
            buttonGameDataBrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGameDataBrowse.Location = new System.Drawing.Point(508, 70);
            buttonGameDataBrowse.Name = "buttonGameDataBrowse";
            buttonGameDataBrowse.Size = new System.Drawing.Size(75, 23);
            buttonGameDataBrowse.TabIndex = 19;
            buttonGameDataBrowse.Text = "Browse";
            buttonGameDataBrowse.UseVisualStyleBackColor = true;
            buttonGameDataBrowse.Click += buttonGameDataBrowse_Click;
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Location = new System.Drawing.Point(508, 127);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.TabIndex = 22;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // folderBrowserDialogData
            // 
            folderBrowserDialogData.Description = "Choose Unpacked Data Directory";
            folderBrowserDialogData.UseDescriptionForTitle = true;
            // 
            // folderBrowserDialogGameData
            // 
            folderBrowserDialogGameData.Description = "Select Game Data Directory";
            folderBrowserDialogGameData.UseDescriptionForTitle = true;
            // 
            // DataConfiguration
            // 
            AcceptButton = buttonOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(593, 161);
            Controls.Add(buttonOK);
            Controls.Add(labelData);
            Controls.Add(buttonDataBrowse);
            Controls.Add(textBoxData);
            Controls.Add(labelGameData);
            Controls.Add(textBoxGameData);
            Controls.Add(buttonGameDataBrowse);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DataConfiguration";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Data Path Configuration";
            TopMost = true;
            FormClosing += DataConfiguration_FormClosing;
            Load += DataConfiguration_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelData;
        private System.Windows.Forms.Button buttonDataBrowse;
        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.Label labelGameData;
        private System.Windows.Forms.TextBox textBoxGameData;
        private System.Windows.Forms.Button buttonGameDataBrowse;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogGameData;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}