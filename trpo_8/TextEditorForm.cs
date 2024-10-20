using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace trpo_8
{
    public partial class TextEditorForm : Form
    {
        private string currentFilePath = null; // Stores the current file path

        public TextEditorForm()
        {
            InitializeComponent();

            // Fill the font size combo box
            comboBoxFontSize.Items.AddRange(new object[]
            {
                "8", "10", "12", "14", "16", "18", "20",
                "22", "24", "26", "28", "30", "32", "34", "36"
            });
            comboBoxFontSize.SelectedIndex = 2; // Default font size
            comboBoxFontSize.SelectedIndexChanged += ComboBoxFontSize_SelectedIndexChanged;

            // Fill the font name combo box
            var fontNames = FontFamily.Families.Select(f => f.Name).ToArray();
            comboBoxFontName.Items.AddRange(fontNames);
            comboBoxFontName.SelectedIndexChanged += ComboBoxFontName_SelectedIndexChanged; // Handle font change
        }

        private void ComboBoxFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFontName.SelectedItem is string selectedFont)
            {
                try
                {
                    if (richTextBox1.SelectionLength > 0)
                    {
                        SetSelectedTextFontName(selectedFont);
                    }
                    else
                    {
                        SetDefaultFontName(selectedFont);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при изменении шрифта: {ex.Message}");
                }
            }
        }

        private void SetSelectedTextFontName(string fontName)
        {
            var selectionFont = richTextBox1.SelectionFont;
            if (selectionFont != null)
            {
                richTextBox1.SelectionFont = new Font(fontName, selectionFont.Size, selectionFont.Style);
            }
            else
            {
                richTextBox1.SelectionFont = new Font(fontName, richTextBox1.Font.Size);
            }
        }

        private void SetDefaultFontName(string fontName)
        {
            richTextBox1.Font = new Font(fontName, richTextBox1.Font.Size);
        }

        private void ComboBoxFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFontSize.SelectedItem is string fontSizeString &&
                float.TryParse(fontSizeString, out float fontSize))
            {
                try
                {
                    if (richTextBox1.SelectionLength > 0)
                    {
                        SetSelectedTextFontSize(fontSize);
                    }
                    else
                    {
                        SetDefaultFontSize(fontSize);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при изменении размера шрифта: {ex.Message}");
                }
            }
        }

        private void SetSelectedTextFontSize(float fontSize)
        {
            var selectionFont = richTextBox1.SelectionFont;
            richTextBox1.SelectionFont = selectionFont != null
                ? new Font(selectionFont.FontFamily, fontSize, selectionFont.Style)
                : new Font(richTextBox1.Font.FontFamily, fontSize);
        }

        private void SetDefaultFontSize(float fontSize)
        {
            richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, fontSize);
        }

        private void btnOpenFile_Click(object sender, EventArgs e) => OpenFile();

        private void btnSaveFile_Click(object sender, EventArgs e) => SaveFile();

        private void btnSaveAs_Click(object sender, EventArgs e) => SaveFileAs();

        private void OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|RTF Files (*.rtf)|*.rtf|Word Files (*.docx)|*.docx|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(openFileDialog.FileName);
                }
            }
        }

        private void LoadFile(string filePath)
        {
            try
            {
                currentFilePath = filePath; // Store the file path
                string fileExtension = Path.GetExtension(filePath).ToLower();

                if (fileExtension == ".txt")
                {
                    richTextBox1.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
                else if (fileExtension == ".rtf")
                {
                    richTextBox1.LoadFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else if (fileExtension == ".docx")
                {
                    using (var document = DocX.Load(currentFilePath))
                    {
                        richTextBox1.Text = document.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
            }
        }

        private void SaveFile()
        {
            if (currentFilePath != null) // If a file was opened
            {
                SaveCurrentFile(currentFilePath);
            }
            else
            {
                SaveFileAs(); // Save as
            }
        }

        private void SaveFileAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|RTF Files (*.rtf)|*.rtf|Word Files (*.docx)|*.docx|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName; // Update current file path
                    SaveCurrentFile(currentFilePath);
                }
            }
        }

        private void SaveCurrentFile(string filePath)
        {
            try
            {
                string fileExtension = Path.GetExtension(filePath).ToLower();
                if (fileExtension == ".txt")
                {
                    richTextBox1.SaveFile(filePath, RichTextBoxStreamType.PlainText);
                }
                else if (fileExtension == ".rtf")
                {
                    richTextBox1.SaveFile(filePath, RichTextBoxStreamType.RichText);
                }
                else if (fileExtension == ".docx")
                {
                    using (var document = DocX.Create(filePath))
                    {
                        document.InsertParagraph(richTextBox1.Text);
                        document.Save();
                    }
                }
                MessageBox.Show("Файл успешно сохранен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
            }
        }
        private void btnChangeFont_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                // Установите текущий шрифт в диалоговое окно
                fontDialog.Font = richTextBox1.Font;

                // Отобразите диалоговое окно
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    // Установите выбранный шрифт в RichTextBox
                    richTextBox1.SelectionFont = fontDialog.Font;
                }
            }
        }

    }
}
