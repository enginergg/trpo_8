using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace trpo_8
{
    public partial class OrganizerForm : Form
    {
        private List<string> tasks; // Список задач

        public OrganizerForm()
        {
            InitializeComponent();
            tasks = new List<string>();
        }

        // Метод для добавления задачи
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string task = txtTask.Text.Trim();
            if (!string.IsNullOrEmpty(task))
            {
                tasks.Add(task);
                UpdateTaskList();
                txtTask.Clear();
            }
            else
            {
                MessageBox.Show("Введите задачу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для удаления выбранной задачи
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex != -1)
            {
                tasks.RemoveAt(lstTasks.SelectedIndex);
                UpdateTaskList();
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для обновления списка задач
        private void UpdateTaskList()
        {
            lstTasks.Items.Clear();
            foreach (var task in tasks)
            {
                lstTasks.Items.Add(task);
            }
        }
    }
}
