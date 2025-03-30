namespace TMLGen.Forms
{
    partial class LocationSelection
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
            buttonSkip = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            listBoxSelection = new System.Windows.Forms.ListBox();
            buttonOK = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            buttonCopy = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // buttonSkip
            // 
            buttonSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            buttonSkip.Location = new System.Drawing.Point(311, 277);
            buttonSkip.Name = "buttonSkip";
            buttonSkip.Size = new System.Drawing.Size(75, 23);
            buttonSkip.TabIndex = 10;
            buttonSkip.Text = "Skip";
            buttonSkip.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(450, 30);
            label1.TabIndex = 9;
            label1.Text = "This timeline has multiple starting locations. You can choose one to use from the list\r\nbelow or skip this step and add it later in the editor.";
            // 
            // listBoxSelection
            // 
            listBoxSelection.FormattingEnabled = true;
            listBoxSelection.ItemHeight = 15;
            listBoxSelection.Location = new System.Drawing.Point(12, 102);
            listBoxSelection.Name = "listBoxSelection";
            listBoxSelection.Size = new System.Drawing.Size(455, 169);
            listBoxSelection.TabIndex = 8;
            listBoxSelection.SelectedValueChanged += listBoxSelection_SelectedValueChanged;
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Enabled = false;
            buttonOK.Location = new System.Drawing.Point(392, 277);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.TabIndex = 7;
            buttonOK.Text = "Select";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += buttonOK_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 54);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(445, 45);
            label2.TabIndex = 11;
            label2.Text = "You can find the name of these timeline scene triggers either by searching for them\r\nin the level they appear in in the editor or by using the index search feature of the\r\nBG3 Modder's Multitool.";
            // 
            // buttonCopy
            // 
            buttonCopy.Enabled = false;
            buttonCopy.Location = new System.Drawing.Point(12, 277);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new System.Drawing.Size(75, 23);
            buttonCopy.TabIndex = 12;
            buttonCopy.Text = "Copy";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // LocationSelection
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(480, 306);
            Controls.Add(buttonCopy);
            Controls.Add(label2);
            Controls.Add(buttonSkip);
            Controls.Add(label1);
            Controls.Add(listBoxSelection);
            Controls.Add(buttonOK);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "LocationSelection";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Location Selection";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxSelection;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCopy;
    }
}