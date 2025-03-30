using System;
using System.Windows.Forms;

namespace TMLGen.Forms
{
    public partial class ModSelection : Form
    {
        public string modName = string.Empty;

        public ModSelection()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            modName = textBoxName.Text;
        }
    }
}
