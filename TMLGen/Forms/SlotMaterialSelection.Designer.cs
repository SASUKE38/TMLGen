namespace TMLGen.Forms
{
    partial class SlotMaterialSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlotMaterialSelection));
            buttonOK = new System.Windows.Forms.Button();
            listBoxSelection = new System.Windows.Forms.ListBox();
            label1 = new System.Windows.Forms.Label();
            buttonSkip = new System.Windows.Forms.Button();
            labelMaterialId = new System.Windows.Forms.Label();
            labelVisualId = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Enabled = false;
            buttonOK.Location = new System.Drawing.Point(392, 306);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new System.Drawing.Size(75, 23);
            buttonOK.TabIndex = 1;
            buttonOK.Text = "Select";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += button1_Click;
            // 
            // listBoxSelection
            // 
            listBoxSelection.FormattingEnabled = true;
            listBoxSelection.ItemHeight = 15;
            listBoxSelection.Location = new System.Drawing.Point(12, 131);
            listBoxSelection.Name = "listBoxSelection";
            listBoxSelection.Size = new System.Drawing.Size(455, 169);
            listBoxSelection.TabIndex = 2;
            listBoxSelection.SelectedValueChanged += listBoxSelection_SelectedValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(458, 75);
            label1.TabIndex = 3;
            label1.Text = resources.GetString("label1.Text");
            // 
            // buttonSkip
            // 
            buttonSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            buttonSkip.Location = new System.Drawing.Point(311, 306);
            buttonSkip.Name = "buttonSkip";
            buttonSkip.Size = new System.Drawing.Size(75, 23);
            buttonSkip.TabIndex = 4;
            buttonSkip.Text = "Skip";
            buttonSkip.UseVisualStyleBackColor = true;
            // 
            // labelMaterialId
            // 
            labelMaterialId.AutoSize = true;
            labelMaterialId.Location = new System.Drawing.Point(12, 98);
            labelMaterialId.Name = "labelMaterialId";
            labelMaterialId.Size = new System.Drawing.Size(118, 15);
            labelMaterialId.TabIndex = 5;
            labelMaterialId.Text = "Material Resource ID:";
            // 
            // labelVisualId
            // 
            labelVisualId.AutoSize = true;
            labelVisualId.Location = new System.Drawing.Point(12, 113);
            labelVisualId.Name = "labelVisualId";
            labelVisualId.Size = new System.Drawing.Size(106, 15);
            labelVisualId.TabIndex = 6;
            labelVisualId.Text = "Visual Resource ID:";
            // 
            // SlotMaterialSelection
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(481, 339);
            Controls.Add(labelVisualId);
            Controls.Add(labelMaterialId);
            Controls.Add(buttonSkip);
            Controls.Add(label1);
            Controls.Add(listBoxSelection);
            Controls.Add(buttonOK);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SlotMaterialSelection";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Slot Material Character Selection";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListBox listBoxSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.Label labelMaterialId;
        private System.Windows.Forms.Label labelVisualId;
    }
}