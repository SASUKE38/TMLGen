using System;
using System.Collections.Generic;
using System.Windows.Forms;

#pragma warning disable CA1416
namespace TMLGen.Forms
{
    public partial class LocationSelection : Form
    {
        public Guid selected;

        public LocationSelection(HashSet<Guid> candidates, string sourceNameExtensionless)
        {
            InitializeComponent();
            foreach (Guid id in candidates)
            {
                listBoxSelection.Items.Add(id);
            }
            Text += $" - {sourceNameExtensionless}";
        }

        private void listBoxSelection_SelectedValueChanged(object sender, EventArgs e)
        {
            bool isSelected = listBoxSelection.SelectedIndex != -1;
            buttonOK.Enabled = isSelected;
            buttonCopy.Enabled = isSelected;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            selected = (Guid)listBoxSelection.SelectedItem;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(((Guid)listBoxSelection.SelectedItem).ToString());
        }
    }
}
