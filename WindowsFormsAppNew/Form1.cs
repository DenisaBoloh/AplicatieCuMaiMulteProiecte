using System;
using System.Drawing;
using System.Windows.Forms;
using AdministrareFisier;
using LibrarieModele;
using System.Drawing.Drawing2D;

namespace TaskManagerUI
{
    public partial class Form1 : Form
    {
        private AdministrareClienti adminClienti;
        private AdministrareTaskuri adminTaskuri;
        private DataGridView tasksDataGridView;
        private TextBox txtSearch;
        private Button btnReset;

        
        private readonly Color primaryColor = Color.FromArgb(68, 114, 196);
        private readonly Color secondaryColor = Color.FromArgb(237, 125, 49);
        private readonly Color lightBgColor = Color.FromArgb(240, 243, 248);
        private readonly Color darkTextColor = Color.FromArgb(51, 51, 51);
        private readonly Color successColor = Color.FromArgb(92, 184, 92);

        public Form1()
        {
            InitializeComponent();
            adminClienti = new AdministrareClienti();
            adminTaskuri = new AdministrareTaskuri(adminClienti);
            InitializeDashboardUI();
        }

        private void InitializeDashboardUI()
        {
            
            this.Text = "Task Manager";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = lightBgColor;
            this.Font = new Font("Segoe UI", 9);

            // Main panel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = lightBgColor;
            mainPanel.Padding = new Padding(25);
            this.Controls.Add(mainPanel);

            
            Label headerLabel = new Label();
            headerLabel.Text = "TASK MANAGER";
            headerLabel.Font = new Font("Segoe UI Semibold", 18, FontStyle.Bold);
            headerLabel.AutoSize = true;
            headerLabel.Location = new Point(25, 25);
            headerLabel.ForeColor = primaryColor;
            mainPanel.Controls.Add(headerLabel);

            
            Panel quickActionsPanel = new Panel();
            quickActionsPanel.BackColor = Color.White;
            quickActionsPanel.Padding = new Padding(10);
            quickActionsPanel.Location = new Point(25, 70);
            quickActionsPanel.Size = new Size(mainPanel.Width - 50, 70);
            quickActionsPanel.BorderStyle = BorderStyle.None;
            quickActionsPanel.BackColor = Color.Transparent;
            mainPanel.Controls.Add(quickActionsPanel);

            
            Button addPersonButton = new Button();
            addPersonButton.Text = "Add Person";
            addPersonButton.Size = new Size(140, 40);
            addPersonButton.Location = new Point(10, 10);
            StyleButton(addPersonButton, primaryColor);
            addPersonButton.Click += (sender, e) => ShowAddPersonDialog();
            quickActionsPanel.Controls.Add(addPersonButton);

            
            Button addTaskButton = new Button();
            addTaskButton.Text = "Add Task";
            addTaskButton.Size = new Size(140, 40);
            addTaskButton.Location = new Point(160, 10);
            StyleButton(addTaskButton, secondaryColor);
            addTaskButton.Click += (sender, e) => ShowAddTaskDialog();
            quickActionsPanel.Controls.Add(addTaskButton);

            
            Panel searchPanel = new Panel();
            searchPanel.BackColor = Color.White;
            searchPanel.Location = new Point(25, 150);
            searchPanel.Size = new Size(mainPanel.Width - 50, 50);
            searchPanel.Padding = new Padding(5);
            searchPanel.BorderStyle = BorderStyle.None;
            mainPanel.Controls.Add(searchPanel);

            
            txtSearch = new TextBox();
            txtSearch.Location = new Point(10, 10);
            txtSearch.Width = 250;
            txtSearch.Height = 30;
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "Search by person name...";
            txtSearch.GotFocus += (sender, e) =>
            {
                if (txtSearch.Text == "Search by person name...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = darkTextColor;
                }
            };
            txtSearch.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Search by person name...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            searchPanel.Controls.Add(txtSearch);

            
            Button btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Size = new Size(100, 35);  
            btnSearch.Location = new Point(270, 8);  
            StyleButton(btnSearch, primaryColor);
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);

           
            btnReset = new Button();
            btnReset.Text = "Reset";
            btnReset.Size = new Size(100, 35);  
            btnReset.Location = new Point(380, 8);  
            StyleButton(btnReset, Color.Gray);
            btnReset.Click += BtnReset_Click;
            searchPanel.Controls.Add(btnReset);

            // Tasks DataGridView
            tasksDataGridView = new DataGridView();
            tasksDataGridView.Location = new Point(25, 210);
            tasksDataGridView.Size = new Size(mainPanel.Width - 50, mainPanel.Height - 240);
            tasksDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tasksDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tasksDataGridView.RowHeadersVisible = false;
            tasksDataGridView.AllowUserToAddRows = false;
            tasksDataGridView.BackgroundColor = Color.White;
            tasksDataGridView.BorderStyle = BorderStyle.None;
            tasksDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            tasksDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10);
            tasksDataGridView.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            tasksDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tasksDataGridView.ColumnHeadersHeight = 35;
            tasksDataGridView.RowTemplate.Height = 30;
            tasksDataGridView.EnableHeadersVisualStyles = false;
            tasksDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            tasksDataGridView.GridColor = Color.FromArgb(224, 224, 224);

            
            tasksDataGridView.Columns.Add("Person", "PERSON");
            tasksDataGridView.Columns.Add("Task", "TASK DESCRIPTION");
            tasksDataGridView.Columns.Add("Status", "STATUS");

            tasksDataGridView.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tasksDataGridView.Columns["Status"].Width = 120;

            DataGridViewButtonColumn completeButtonColumn = new DataGridViewButtonColumn();
            completeButtonColumn.Name = "Complete";
            completeButtonColumn.Text = "Complete";
            completeButtonColumn.UseColumnTextForButtonValue = true;
            completeButtonColumn.DefaultCellStyle.BackColor = successColor;
            completeButtonColumn.DefaultCellStyle.ForeColor = Color.White;
            completeButtonColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            tasksDataGridView.Columns.Add(completeButtonColumn);

            tasksDataGridView.CellContentClick += TasksDataGridView_CellContentClick;
            mainPanel.Controls.Add(tasksDataGridView);

            RefreshTasksGrid();
        }

        private void StyleButton(Button button, Color bgColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = bgColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 9.5f);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(5);

            
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, 10, 10, 180, 90);
            path.AddArc(button.Width - 10, 0, 10, 10, 270, 90);
            path.AddArc(button.Width - 10, button.Height - 10, 10, 10, 0, 90);
            path.AddArc(0, button.Height - 10, 10, 10, 90, 90);
            path.CloseFigure();
            button.Region = new Region(path);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) || txtSearch.Text == "Search by person name...")
            {
                RefreshTasksGrid();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText) || searchText == "Search by person name...")
            {
                MessageBox.Show("Please enter a name to search!", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            tasksDataGridView.Rows.Clear();
            foreach (var person in adminClienti.GetPersoane())
            {
                if (person.Nume.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
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
            }
            HighlightCompletedTasks();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Search by person name...";
            txtSearch.ForeColor = Color.Gray;
            RefreshTasksGrid();
        }

        private void ShowAddPersonDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Add New Person";
            dialog.Size = new Size(350, 180);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = lightBgColor;
            dialog.Padding = new Padding(20);

            Label nameLabel = new Label();
            nameLabel.Text = "Person Name:";
            nameLabel.Location = new Point(30, 30);
            nameLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(nameLabel);

            TextBox nameTextBox = new TextBox();
            nameTextBox.Width = 250;
            nameTextBox.Location = new Point(30, 55);
            nameTextBox.Font = new Font("Segoe UI", 10);
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.ForeColor = Color.Gray;
            nameTextBox.Text = "Enter person name...";
            nameTextBox.GotFocus += (sender, e) =>
            {
                if (nameTextBox.Text == "Enter person name...")
                {
                    nameTextBox.Text = "";
                    nameTextBox.ForeColor = darkTextColor;
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
            addButton.Text = "Add Person";
            addButton.Size = new Size(120, 35);
            addButton.Location = new Point(160, 100);
            StyleButton(addButton, primaryColor);
            addButton.Click += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text) && nameTextBox.Text != "Enter person name...")
                {
                    adminClienti.AdaugaPersoana(nameTextBox.Text.Trim());
                    dialog.Close();
                    RefreshTasksGrid();
                }
                else
                {
                    MessageBox.Show("Please enter a valid name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            dialog.Controls.Add(addButton);

            dialog.ShowDialog(this);
        }

        private void ShowAddTaskDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Add New Task";
            dialog.Size = new Size(350, 230);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = lightBgColor;
            dialog.Padding = new Padding(20);

            Label personLabel = new Label();
            personLabel.Text = "Select Person:";
            personLabel.Location = new Point(30, 20);
            personLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(personLabel);

            ComboBox personComboBox = new ComboBox();
            personComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            personComboBox.Width = 250;
            personComboBox.Location = new Point(30, 45);
            personComboBox.Font = new Font("Segoe UI", 10);
            foreach (var person in adminClienti.GetPersoane())
            {
                personComboBox.Items.Add(person.Nume);
            }
            if (personComboBox.Items.Count > 0)
            {
                personComboBox.SelectedIndex = 0;
            }
            dialog.Controls.Add(personComboBox);

            Label taskLabel = new Label();
            taskLabel.Text = "Task Description:";
            taskLabel.Location = new Point(30, 85);
            taskLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(taskLabel);

            TextBox taskTextBox = new TextBox();
            taskTextBox.Width = 250;
            taskTextBox.Location = new Point(30, 110);
            taskTextBox.Font = new Font("Segoe UI", 10);
            taskTextBox.BorderStyle = BorderStyle.FixedSingle;
            taskTextBox.ForeColor = Color.Gray;
            taskTextBox.Text = "Enter task description...";
            taskTextBox.GotFocus += (sender, e) =>
            {
                if (taskTextBox.Text == "Enter task description...")
                {
                    taskTextBox.Text = "";
                    taskTextBox.ForeColor = darkTextColor;
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
            dialog.Controls.Add(taskTextBox);

            Button addButton = new Button();
            addButton.Text = "Add Task";
            addButton.Size = new Size(120, 35);
            addButton.Location = new Point(160, 160);
            StyleButton(addButton, secondaryColor);
            addButton.Click += (sender, e) =>
            {
                if (personComboBox.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(taskTextBox.Text) &&
                    taskTextBox.Text != "Enter task description...")
                {
                    adminTaskuri.AdaugaTask(personComboBox.SelectedItem.ToString(), taskTextBox.Text.Trim());
                    dialog.Close();
                    RefreshTasksGrid();
                }
                else
                {
                    MessageBox.Show("Please select a person and enter a task description!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            HighlightCompletedTasks();
        }

        private void HighlightCompletedTasks()
        {
            foreach (DataGridViewRow row in tasksDataGridView.Rows)
            {
                if (row.Cells["Status"].Value?.ToString() == "Completed")
                {
                    row.Cells["Complete"].Value = "Completed";
                    row.Cells["Complete"].ReadOnly = true;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 223);
                    row.Cells["Complete"].Style.BackColor = successColor;
                    row.Cells["Complete"].Style.ForeColor = Color.White;
                }
                else
                {
                    row.Cells["Complete"].Style.BackColor = primaryColor;
                    row.Cells["Complete"].Style.ForeColor = Color.White;
                }
            }
        }
    }
}