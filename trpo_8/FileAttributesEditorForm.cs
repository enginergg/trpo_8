using System;
using System.IO;
using System.Windows.Forms;

namespace trpo_8
{
    public partial class FileAttributesEditorForm : Form
    {
        public FileAttributesEditorForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                    LoadFileAttributes(openFileDialog.FileName);
                }
            }
        }

        private void LoadFileAttributes(string filePath)
        {
            FileAttributes attributes = File.GetAttributes(filePath);
            chkReadOnly.Checked = (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            chkHidden.Checked = (attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            chkSystem.Checked = (attributes & FileAttributes.System) == FileAttributes.System;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;
            FileAttributes attributes = File.GetAttributes(filePath);

            if (chkReadOnly.Checked)
                attributes |= FileAttributes.ReadOnly;
            else
                attributes &= ~FileAttributes.ReadOnly;

            if (chkHidden.Checked)
                attributes |= FileAttributes.Hidden;
            else
                attributes &= ~FileAttributes.Hidden;

            if (chkSystem.Checked)
                attributes |= FileAttributes.System;
            else
                attributes &= ~FileAttributes.System;

            File.SetAttributes(filePath, attributes);
            MessageBox.Show("Атрибуты файла успешно изменены.");
        }
    }
}
