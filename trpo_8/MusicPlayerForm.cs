using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace trpo_8
{
    public partial class MusicPlayerForm : Form
    {
        private AudioFileReader audioFileReader; 
        private WaveOutEvent outputDevice; 

        public MusicPlayerForm()
        {
            InitializeComponent();

            // Установите минимальное и максимальное значение для trackBarVolume
            trackBarVolume.Minimum = 0; // Минимальная громкость
            trackBarVolume.Maximum = 100; // Максимальная громкость

            // Установите начальное значение trackBarVolume на 100 (максимальная громкость)
            trackBarVolume.Value = 100;

            // Примените громкость
            trackBarVolume_Scroll(this, EventArgs.Empty);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.aiff";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                lblFileName.Text = $"Playing: {System.IO.Path.GetFileName(fileName)}"; 

                outputDevice?.Stop();
                outputDevice?.Dispose();
                audioFileReader?.Dispose();

                audioFileReader = new AudioFileReader(fileName);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFileReader);
            }
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                if (audioFileReader.Position == audioFileReader.Length)
                {
                    audioFileReader.Position = 0;
                }
                outputDevice.Play();
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            outputDevice?.Stop();
        }
        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            if (audioFileReader != null)
            {
                audioFileReader.Volume = trackBarVolume.Value / 100f; 
            }
        }
    }
}
