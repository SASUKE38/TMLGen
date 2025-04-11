namespace TMLGen.Forms
{
    partial class ModSelection
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
            textBoxName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            buttonCancel = new System.Windows.Forms.Button();
            buttonAdd = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // textBoxName
            // 
            textBoxName.Location = new System.Drawing.Point(12, 27);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new System.Drawing.Size(360, 23);
            textBoxName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(189, 15);
            label1.TabIndex = 1;
            label1.Text = "Enter the name of the mod to add.";
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(216, 56);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            buttonAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonAdd.Location = new System.Drawing.Point(297, 56);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new System.Drawing.Size(75, 23);
            buttonAdd.TabIndex = 3;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // ModSelection
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(383, 93);
            ControlBox = false;
            Controls.Add(buttonAdd);
            Controls.Add(buttonCancel);
            Controls.Add(label1);
            Controls.Add(textBoxName);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(399, 132);
            MinimumSize = new System.Drawing.Size(399, 132);
            Name = "ModSelection";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Mod Selection";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAdd;
    }
}