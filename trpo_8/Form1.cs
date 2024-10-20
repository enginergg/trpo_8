using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trpo_8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnMusicPlayer_Click(object sender, EventArgs e)
        {
            MusicPlayerForm musicPlayer = new MusicPlayerForm();
            musicPlayer.Show();
        }

        private void btnTextEditor_Click(object sender, EventArgs e)
        {
            TextEditorForm textEditor = new TextEditorForm();
            textEditor.Show();
        }

        private void btnGraphicEditor_Click(object sender, EventArgs e)
        {
            GraphicEditorForm graphicEditor = new GraphicEditorForm();
            graphicEditor.Show();
        }

        private void btnOrganizer_Click(object sender, EventArgs e)
        {
            OrganizerForm organizer = new OrganizerForm();
            organizer.Show();
        }

        private void btnFileAttributesEditor_Click(object sender, EventArgs e)
        {
            FileAttributesEditorForm fileEditor = new FileAttributesEditorForm();
            fileEditor.Show();
        }
    }
}