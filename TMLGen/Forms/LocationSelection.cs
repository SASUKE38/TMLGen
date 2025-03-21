using System;
using System.Collections.Generic;
using System.Windows.Forms;

#pragma warning disable CA1416
namespace TMLGen.Forms
{
    public partial class LocationSelection : Form
    {
        public Guid selected;
        private List<Guid> candidates;

        public LocationSelection(List<Guid> candidates)
        {
            InitializeComponent();
            foreach (Guid id in candidates)
            {
                listBoxSelection.Items.Add(id);
            }
            this.candidates = candidates;
        }

        private void listBoxSelection_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;
            buttonCopy.Enabled = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            selected = candidates[listBoxSelection.SelectedIndex];
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(((Guid)listBoxSelection.SelectedItem).ToString());
        }
    }
}
