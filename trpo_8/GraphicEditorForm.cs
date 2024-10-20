using System;
using System.Drawing;
using System.Windows.Forms;

namespace trpo_8
{
    public partial class GraphicEditorForm : Form
    {
        private Bitmap canvas;
        private Color currentColor;
        private int brushSize; // Размер кисти
        private bool isDrawing;
        private Point startPoint;
        private Image loadedImage; // Храним загруженное изображение
        private bool isEraserActive = false; // Флаг для режима ластика

        public GraphicEditorForm()
        {
            InitializeComponent();
            InitializeCanvas();
            currentColor = Color.Black; // Установите цвет по умолчанию
            brushSize = 5; // Установите размер кисти по умолчанию
            trackBarBrushSize.Minimum = 2; // Минимальный размер кисти
            trackBarBrushSize.Maximum = 50; // Максимальный размер кисти
            trackBarBrushSize.Value = brushSize; // Установите начальное значение ползунка

            // Включите двойную буферизацию для панели
            panelCanvas.DoubleBuffered(true);
        }

        private void InitializeCanvas()
        {
            canvas = new Bitmap(panelCanvas.Width, panelCanvas.Height);
            panelCanvas.BackgroundImage = canvas;
            panelCanvas.BackgroundImageLayout = ImageLayout.None;
        }

        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                startPoint = e.Location;
            }
        }

        private void panelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                using (Graphics g = Graphics.FromImage(canvas))
                {
                    Color colorToUse = isEraserActive ? Color.White : currentColor; // Если ластик активен, рисуем белым цветом

                    using (Brush brush = new SolidBrush(colorToUse)) // Используем кисть с выбранным цветом
                    {
                        // Вычисляем шаги для рисования между предыдущей и текущей точками
                        int steps = Math.Max(Math.Abs(e.X - startPoint.X), Math.Abs(e.Y - startPoint.Y));

                        if (steps == 0) // Проверка на случай, если шаги равны 0, чтобы избежать деления на ноль
                        {
                            g.FillEllipse(brush, e.X - brushSize / 2, e.Y - brushSize / 2, brushSize, brushSize);
                        }
                        else
                        {
                            for (int i = 0; i <= steps; i++)
                            {
                                float x = startPoint.X + (e.X - startPoint.X) * i / steps;
                                float y = startPoint.Y + (e.Y - startPoint.Y) * i / steps;
                                g.FillEllipse(brush, x - brushSize / 2, y - brushSize / 2, brushSize, brushSize);
                            }
                        }
                    }
                }

                // Обновляем startPoint для следующего движения
                startPoint = e.Location;
                panelCanvas.Invalidate(); // Перерисовать панель
            }
        }

        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            InitializeCanvas(); // Очистить холст
            panelCanvas.Invalidate();
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    currentColor = colorDialog.Color; // Установить выбранный цвет
                    isEraserActive = false; // Деактивировать ластик при выборе нового цвета
                    buttonEraser.Text = "Включить ластик"; // Сбросить текст кнопки ластика
                }
            }
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Фильтр для изображений
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    loadedImage = Image.FromFile(openFileDialog.FileName); // Загрузка изображения
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        g.DrawImage(loadedImage, new Point(0, 0)); // Отрисовка изображения на холсте
                    }
                    panelCanvas.Invalidate();
                }
            }
        }

        private void trackBarBrushSize_Scroll(object sender, EventArgs e)
        {
            brushSize = trackBarBrushSize.Value; // Обновление размера кисти
        }

        private void buttonEraser_Click(object sender, EventArgs e)
        {
            isEraserActive = !isEraserActive; // Переключение режима ластика
            if (isEraserActive)
            {
                buttonEraser.Text = "Отключить ластик"; // Изменение текста кнопки
            }
            else
            {
                buttonEraser.Text = "Включить ластик"; // Возвращаем текст кнопки
            }
        }
    }

    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            System.Reflection.PropertyInfo propertyInfo = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            propertyInfo.SetValue(control, enable, null);
        }
    }
}
