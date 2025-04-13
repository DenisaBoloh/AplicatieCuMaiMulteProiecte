using System;
using System.Drawing;
using System.Windows.Forms;
using AdministrareFisier;
using LibrarieModele;

namespace TaskManagerUI
{
    public partial class Form1 : Form
    {
        private AdministrareClienti adminClienti;
        private AdministrareTaskuri adminTaskuri;
        private DataGridView tasksDataGridView;

        public Form1()
        {
            InitializeComponent();
            adminClienti = new AdministrareClienti();
            adminTaskuri = new AdministrareTaskuri(adminClienti);
            InitializeDashboardUI();
        }

        private void InitializeDashboardUI()
        {
            this.Text = "Task Manager - Dashboard View";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Main Panel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.WhiteSmoke;
            mainPanel.Padding = new Padding(20);
            this.Controls.Add(mainPanel);

            // Header
            Label headerLabel = new Label();
            headerLabel.Text = "Task Manager Dashboard";
            headerLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            headerLabel.AutoSize = true;
            mainPanel.Controls.Add(headerLabel);

            // Quick Actions Panel
            Panel quickActionsPanel = new Panel();
            quickActionsPanel.Dock = DockStyle.Top;
            quickActionsPanel.Height = 80;
            quickActionsPanel.BackColor = Color.LightGray;
            quickActionsPanel.Padding = new Padding(10);
            mainPanel.Controls.Add(quickActionsPanel);

            // Add Person Button
            Button addPersonButton = new Button();
            addPersonButton.Text = "Add Person";
            addPersonButton.Size = new Size(120, 40);
            addPersonButton.Location = new Point(10, 20);
            addPersonButton.Click += (sender, e) => ShowAddPersonDialog();
            quickActionsPanel.Controls.Add(addPersonButton);

            // Add Task Button
            Button addTaskButton = new Button();
            addTaskButton.Text = "Add Task";
            addTaskButton.Size = new Size(120, 40);
            addTaskButton.Location = new Point(140, 20);
            addTaskButton.Click += (sender, e) => ShowAddTaskDialog();
            quickActionsPanel.Controls.Add(addTaskButton);

            // Tasks DataGridView
            tasksDataGridView = new DataGridView();
            tasksDataGridView.Dock = DockStyle.Fill;
            tasksDataGridView.Margin = new Padding(0, 90, 0, 0);
            tasksDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tasksDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tasksDataGridView.RowHeadersVisible = false;
            tasksDataGridView.AllowUserToAddRows = false;
            tasksDataGridView.BackgroundColor = Color.White;

            // Add columns
            tasksDataGridView.Columns.Add("Person", "Person");
            tasksDataGridView.Columns.Add("Task", "Task Description");
            tasksDataGridView.Columns.Add("Status", "Status");

            // Status column styling
            tasksDataGridView.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tasksDataGridView.Columns["Status"].Width = 100;

            // Mark complete button column
            DataGridViewButtonColumn completeButtonColumn = new DataGridViewButtonColumn();
            completeButtonColumn.Name = "Complete";
            completeButtonColumn.Text = "Mark Complete";
            completeButtonColumn.UseColumnTextForButtonValue = true;
            tasksDataGridView.Columns.Add(completeButtonColumn);

            tasksDataGridView.CellContentClick += TasksDataGridView_CellContentClick;
            mainPanel.Controls.Add(tasksDataGridView);

            // Load data
            RefreshTasksGrid();
        }

        private void ShowAddPersonDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Add New Person";
            dialog.Size = new Size(300, 150);
            dialog.StartPosition = FormStartPosition.CenterParent;

            TextBox nameTextBox = new TextBox();
            nameTextBox.Width = 200;
            nameTextBox.Location = new Point(50, 30);
            nameTextBox.Text = "Enter person name..."; // Placeholder-like behavior
            nameTextBox.ForeColor = Color.Gray;

            // Add placeholder behavior
            nameTextBox.GotFocus += (sender, e) =>
            {
                if (nameTextBox.Text == "Enter person name...")
                {
                    nameTextBox.Text = "";
                    nameTextBox.ForeColor = Color.Black;
                }
            };

            nameTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                {
                    nameTextBox.Text = "Enter person name...";
                    nameTextBox.ForeColor = Color.Gray;
                }
            };

            dialog.Controls.Add(nameTextBox);

            Button addButton = new Button();
            addButton.Text = "Add";
            addButton.Location = new Point(100, 70);
            addButton.Click += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text) && nameTextBox.Text != "Enter person name...")
                {
                    adminClienti.AdaugaPersoana(nameTextBox.Text.Trim());
                    dialog.Close();
                    RefreshTasksGrid();
                }
            };
            dialog.Controls.Add(addButton);

            dialog.ShowDialog(this);
        }

        private void ShowAddTaskDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Add New Task";
            dialog.Size = new Size(300, 200);
            dialog.StartPosition = FormStartPosition.CenterParent;

            // Person ComboBox
            ComboBox personComboBox = new ComboBox();
            personComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            personComboBox.Width = 200;
            personComboBox.Location = new Point(50, 30);
            foreach (var person in adminClienti.GetPersoane())
            {
                personComboBox.Items.Add(person.Nume);
            }
            if (personComboBox.Items.Count > 0)
            {
                personComboBox.SelectedIndex = 0;
            }
            dialog.Controls.Add(personComboBox);

            // Task Description
            TextBox taskTextBox = new TextBox();
            taskTextBox.Text = "Enter task description..."; // Placeholder-like behavior
            taskTextBox.ForeColor = Color.Gray;

            // Add placeholder behavior:
            taskTextBox.GotFocus += (sender, e) =>
            {
                if (taskTextBox.Text == "Enter task description...")
                {
                    taskTextBox.Text = "";
                    taskTextBox.ForeColor = Color.Black;
                }
            };

            taskTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(taskTextBox.Text))
                {
                    taskTextBox.Text = "Enter task description...";
                    taskTextBox.ForeColor = Color.Gray;
                }
            };


            // With this workaround:
            taskTextBox.Text = "Enter task description..."; // Placeholder-like behavior
            taskTextBox.ForeColor = Color.Gray;

            // Add placeholder behavior:
            taskTextBox.GotFocus += (sender, e) =>
            {
                if (taskTextBox.Text == "Enter task description...")
                {
                    taskTextBox.Text = "";
                    taskTextBox.ForeColor = Color.Black;
                }
            };

            taskTextBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(taskTextBox.Text))
                {
                    taskTextBox.Text = "Enter task description...";
                    taskTextBox.ForeColor = Color.Gray;
                }
            };
            taskTextBox.Width = 200;
            taskTextBox.Location = new Point(50, 70);
            dialog.Controls.Add(taskTextBox);

            Button addButton = new Button();
            addButton.Text = "Add";
            addButton.Location = new Point(100, 110);
            addButton.Click += (sender, e) =>
            {
                if (personComboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(taskTextBox.Text))
                {
                    adminTaskuri.AdaugaTask(personComboBox.SelectedItem.ToString(), taskTextBox.Text.Trim());
                    dialog.Close();
                    RefreshTasksGrid();
                }
            };
            dialog.Controls.Add(addButton);

            dialog.ShowDialog(this);
        }

        private void TasksDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == tasksDataGridView.Columns["Complete"].Index && e.RowIndex >= 0)
            {
                string personName = tasksDataGridView.Rows[e.RowIndex].Cells["Person"].Value.ToString();
                string taskDesc = tasksDataGridView.Rows[e.RowIndex].Cells["Task"].Value.ToString();
                adminTaskuri.MarcheazaTaskCaFinalizat(personName, taskDesc);
                RefreshTasksGrid();
            }
        }

        private void RefreshTasksGrid()
        {
            tasksDataGridView.Rows.Clear();
            foreach (var person in adminClienti.GetPersoane())
            {
                foreach (var task in person.GetTaskuri())
                {
                    tasksDataGridView.Rows.Add(
                        person.Nume,
                        task.Descriere,
                        task.EsteFinalizat ? "Completed" : "Pending"
                    );
                }
            }

            // Update complete button visibility
            foreach (DataGridViewRow row in tasksDataGridView.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "Completed")
                {
                    row.Cells["Complete"].Value = "Done";
                    row.Cells["Complete"].ReadOnly = true;
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }
    }
}