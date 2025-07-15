using System;
using System.Collections.Generic;
using System.Windows.Forms;

#pragma warning disable CA1416
namespace TMLGen.Forms
{
    public partial class SlotMaterialSelection : Form
    {
        public Guid selected;
        private Dictionary<string, Guid> candidates;
        private Guid materialId;
        private Guid resourceId;

        public SlotMaterialSelection(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId, string sourceNameExtensionless)
        {
            InitializeComponent();
            foreach ((string name, _) in candidates)
            {
                listBoxSelection.Items.Add(name);
            }
            this.candidates = candidates;
            this.materialId = materialId;
            this.resourceId = resourceId;
            labelMaterialId.Text += $" {materialId}";
            labelVisualId.Text += $" {resourceId}";
            Text += $" - {sourceNameExtensionless}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected = candidates.GetValueOrDefault((string)listBoxSelection.SelectedItem);
        }

        private void listBoxSelection_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
        }

        private void buttonCopyVisual_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(resourceId.ToString());
        }

        private void buttonCopyMaterial_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(materialId.ToString());
        }
    }
}
