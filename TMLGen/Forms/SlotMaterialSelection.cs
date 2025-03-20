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

        public SlotMaterialSelection(Dictionary<string, Guid> candidates, Guid materialId, Guid resourceId)
        {
            InitializeComponent();
            foreach ((string name, _) in candidates)
            {
                listBoxSelection.Items.Add(name);
            }
            this.candidates = candidates;
            labelMaterialId.Text += " " + materialId;
            labelVisualId.Text += " " + resourceId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected = candidates.GetValueOrDefault((string)listBoxSelection.SelectedItem);
        }

        private void listBoxSelection_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
        }
    }
}
