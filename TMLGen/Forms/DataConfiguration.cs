using System;
using System.Windows.Forms;

#pragma warning disable CA1416
namespace TMLGen.Forms
{
    public partial class DataConfiguration : Form
    {
        public DataConfiguration()
        {
            InitializeComponent();
        }

        private void buttonDataBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogData.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxData.Text = folderBrowserDialogData.SelectedPath;
            }
        }

        private void buttonGameDataBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialogGameData.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxGameData.Text = folderBrowserDialogGameData.SelectedPath;
            }
        }

        private void DataConfiguration_Load(object sender, EventArgs e)
        {
            textBoxData.Text = Properties.Settings.Default.UnpackedDataDirectory;
            textBoxGameData.Text = Properties.Settings.Default.GameDataDirectory;
        }

        private void DataConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.UnpackedDataDirectory = textBoxData.Text;
            Properties.Settings.Default.GameDataDirectory = textBoxGameData.Text;
            Properties.Settings.Default.Save();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Save();
        }
    }
}
