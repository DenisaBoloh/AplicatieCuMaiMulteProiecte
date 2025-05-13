using System;
using System.Drawing;
using System.Windows.Forms;
using AdministrareFisier;
using LibrarieModele;
using System.Drawing.Drawing2D;
using System.Linq;

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
        private readonly Color highPriorityColor = Color.FromArgb(255, 153, 153);
        private readonly Color mediumPriorityColor = Color.FromArgb(255, 255, 153);
        private readonly Color lowPriorityColor = Color.FromArgb(153, 255, 153);

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
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = lightBgColor;
            this.Font = new Font("Segoe UI", 9);

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
            tasksDataGridView.Columns.Add("Priority", "PRIORITY");
            tasksDataGridView.Columns.Add("Important", "IMPORTANT");
            tasksDataGridView.Columns.Add("Status", "STATUS");

            tasksDataGridView.Columns["Person"].Width = 150;
            tasksDataGridView.Columns["Priority"].Width = 100;
            tasksDataGridView.Columns["Important"].Width = 80;
            tasksDataGridView.Columns["Status"].Width = 100;

            tasksDataGridView.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tasksDataGridView.Columns["Important"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewButtonColumn completeButtonColumn = new DataGridViewButtonColumn();
            completeButtonColumn.Name = "Complete";
            completeButtonColumn.Text = "Complete";
            completeButtonColumn.UseColumnTextForButtonValue = true;
            completeButtonColumn.DefaultCellStyle.BackColor = successColor;
            completeButtonColumn.DefaultCellStyle.ForeColor = Color.White;
            completeButtonColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            tasksDataGridView.Columns.Add(completeButtonColumn);

            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "Edit";
            editButtonColumn.Text = "Edit";
            editButtonColumn.UseColumnTextForButtonValue = true;
            editButtonColumn.DefaultCellStyle.BackColor = Color.FromArgb(91, 155, 213);
            editButtonColumn.DefaultCellStyle.ForeColor = Color.White;
            editButtonColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            tasksDataGridView.Columns.Add(editButtonColumn);

            tasksDataGridView.CellContentClick += TasksDataGridView_CellContentClick;
            mainPanel.Controls.Add(tasksDataGridView);

            RefreshTasksGrid();
        }

        private void ShowAddTaskDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Add New Task";
            dialog.Size = new Size(400, 380);
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
            personComboBox.Width = 300;
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
            taskTextBox.Width = 300;
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

            Label priorityLabel = new Label();
            priorityLabel.Text = "Priority:";
            priorityLabel.Location = new Point(30, 150);
            priorityLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(priorityLabel);

            RadioButton rbHigh = new RadioButton();
            rbHigh.Text = "High";
            rbHigh.Location = new Point(30, 175);
            rbHigh.Font = new Font("Segoe UI", 9);
            rbHigh.AutoSize = true;
            dialog.Controls.Add(rbHigh);

            RadioButton rbMedium = new RadioButton();
            rbMedium.Text = "Medium";
            rbMedium.Location = new Point(rbHigh.Right + 20, 175);
            rbMedium.Font = new Font("Segoe UI", 9);
            rbMedium.AutoSize = true;
            rbMedium.Checked = true;
            dialog.Controls.Add(rbMedium);

            RadioButton rbLow = new RadioButton();
            rbLow.Text = "Low";
            rbLow.Location = new Point(rbMedium.Right + 20, 175);
            rbLow.Font = new Font("Segoe UI", 9);
            rbLow.AutoSize = true;
            dialog.Controls.Add(rbLow);

            CheckBox chkImportant = new CheckBox();
            chkImportant.Text = "Important Task";
            chkImportant.Location = new Point(30, 205);
            chkImportant.Font = new Font("Segoe UI", 9);
            chkImportant.AutoSize = true;
            dialog.Controls.Add(chkImportant);

            Button addButton = new Button();
            addButton.Text = "Add Task";
            addButton.Size = new Size(120, 35);
            addButton.Location = new Point(210, 260);
            StyleButton(addButton, secondaryColor);
            addButton.Click += (sender, e) =>
            {
                if (personComboBox.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(taskTextBox.Text) &&
                    taskTextBox.Text != "Enter task description...")
                {
                    var task = new Task(taskTextBox.Text.Trim())
                    {
                        Priority = rbHigh.Checked ? "High" : rbMedium.Checked ? "Medium" : "Low",
                        IsImportant = chkImportant.Checked
                    };

                    var persoana = adminClienti.CautaPersoana(personComboBox.SelectedItem.ToString());
                    persoana.AdaugaTask(task);
                    adminTaskuri.SalveazaTaskuri();
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

        private void ShowEditTaskDialog(string personName, string taskDescription)
        {
            var person = adminClienti.CautaPersoana(personName);
            var task = person.GetTaskuri().FirstOrDefault(t => t.Descriere == taskDescription);
            if (task == null) return;

            Form dialog = new Form();
            dialog.Text = "Edit Task";
            dialog.Size = new Size(400, 420);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = lightBgColor;
            dialog.Padding = new Padding(20);

            
            Label personLabel = new Label();
            personLabel.Text = "Person:";
            personLabel.Location = new Point(30, 20);
            personLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(personLabel);

            TextBox personTextBox = new TextBox();
            personTextBox.Text = personName;
            personTextBox.Width = 300;
            personTextBox.Location = new Point(30, 45);
            personTextBox.Font = new Font("Segoe UI", 10);
            personTextBox.ReadOnly = true;
            personTextBox.BackColor = Color.WhiteSmoke;
            dialog.Controls.Add(personTextBox);

            
            Label taskLabel = new Label();
            taskLabel.Text = "Task Description:";
            taskLabel.Location = new Point(30, 85);
            taskLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(taskLabel);

            TextBox taskTextBox = new TextBox();
            taskTextBox.Text = task.Descriere;
            taskTextBox.Width = 300;
            taskTextBox.Location = new Point(30, 110);
            taskTextBox.Font = new Font("Segoe UI", 10);
            taskTextBox.BorderStyle = BorderStyle.FixedSingle;
            dialog.Controls.Add(taskTextBox);

            
            Label priorityLabel = new Label();
            priorityLabel.Text = "Priority:";
            priorityLabel.Location = new Point(30, 150);
            priorityLabel.Font = new Font("Segoe UI", 10);
            dialog.Controls.Add(priorityLabel);

            RadioButton rbHigh = new RadioButton();
            rbHigh.Text = "High";
            rbHigh.Location = new Point(30, 175);
            rbHigh.Font = new Font("Segoe UI", 9);
            rbHigh.AutoSize = true;
            rbHigh.Checked = task.Priority == "High";
            dialog.Controls.Add(rbHigh);

            RadioButton rbMedium = new RadioButton();
            rbMedium.Text = "Medium";
            rbMedium.Location = new Point(rbHigh.Right + 20, 175);
            rbMedium.Font = new Font("Segoe UI", 9);
            rbMedium.AutoSize = true;
            rbMedium.Checked = task.Priority == "Medium";
            dialog.Controls.Add(rbMedium);

            RadioButton rbLow = new RadioButton();
            rbLow.Text = "Low";
            rbLow.Location = new Point(rbMedium.Right + 20, 175);
            rbLow.Font = new Font("Segoe UI", 9);
            rbLow.AutoSize = true;
            rbLow.Checked = task.Priority == "Low";
            dialog.Controls.Add(rbLow);

            
            CheckBox chkImportant = new CheckBox();
            chkImportant.Text = "Important Task";
            chkImportant.Location = new Point(30, 205);
            chkImportant.Font = new Font("Segoe UI", 9);
            chkImportant.AutoSize = true;
            chkImportant.Checked = task.IsImportant;
            dialog.Controls.Add(chkImportant);

            
            CheckBox chkCompleted = new CheckBox();
            chkCompleted.Text = "Completed";
            chkCompleted.Location = new Point(30, 235);
            chkCompleted.Font = new Font("Segoe UI", 9);
            chkCompleted.AutoSize = true;
            chkCompleted.Checked = task.EsteFinalizat;
            dialog.Controls.Add(chkCompleted);

            
            Button updateButton = new Button();
            updateButton.Text = "Update Task";
            updateButton.Size = new Size(120, 35);
            updateButton.Location = new Point(210, 280);
            StyleButton(updateButton, secondaryColor);
            updateButton.Click += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(taskTextBox.Text))
                {
                    
                    task.Descriere = taskTextBox.Text.Trim();
                    task.Priority = rbHigh.Checked ? "High" : rbMedium.Checked ? "Medium" : "Low";
                    task.IsImportant = chkImportant.Checked;
                    task.EsteFinalizat = chkCompleted.Checked;

                    adminTaskuri.SalveazaTaskuri();
                    dialog.Close();
                    RefreshTasksGrid();
                }
                else
                {
                    MessageBox.Show("Please enter a task description!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            dialog.Controls.Add(updateButton);

            
            Button deleteButton = new Button();
            deleteButton.Text = "Delete Task";
            deleteButton.Size = new Size(120, 35);
            deleteButton.Location = new Point(80, 280);
            StyleButton(deleteButton, Color.FromArgb(217, 83, 79));
            deleteButton.Click += (sender, e) =>
            {
                if (MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    person.StergeTask(task.Descriere); 
                    dialog.Close();
                    RefreshTasksGrid();
                }
            };

            dialog.Controls.Add(deleteButton);

            dialog.ShowDialog(this);
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
                        task.Priority,
                        task.IsImportant ? "★" : "",
                        task.EsteFinalizat ? "Completed" : "Pending"
                    );

                    DataGridViewRow row = tasksDataGridView.Rows[tasksDataGridView.Rows.Count - 1];
                    if (task.Priority == "High")
                        row.DefaultCellStyle.BackColor = highPriorityColor;
                    else if (task.Priority == "Medium")
                        row.DefaultCellStyle.BackColor = mediumPriorityColor;
                    else
                        row.DefaultCellStyle.BackColor = lowPriorityColor;
                }
            }
            HighlightCompletedTasks();
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
                            task.Priority,
                            task.IsImportant ? "★" : "",
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

        private void TasksDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == tasksDataGridView.Columns["Complete"].Index)
            {
                string personName = tasksDataGridView.Rows[e.RowIndex].Cells["Person"].Value.ToString();
                string taskDesc = tasksDataGridView.Rows[e.RowIndex].Cells["Task"].Value.ToString();
                adminTaskuri.MarcheazaTaskCaFinalizat(personName, taskDesc);
                RefreshTasksGrid();
            }
            else if (e.ColumnIndex == tasksDataGridView.Columns["Edit"].Index)
            {
                string personName = tasksDataGridView.Rows[e.RowIndex].Cells["Person"].Value.ToString();
                string taskDesc = tasksDataGridView.Rows[e.RowIndex].Cells["Task"].Value.ToString();
                ShowEditTaskDialog(personName, taskDesc);
            }
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
                    row.Cells["Edit"].Style.BackColor = Color.FromArgb(191, 191, 191);
                }
                else
                {
                    row.Cells["Complete"].Style.BackColor = primaryColor;
                    row.Cells["Complete"].Style.ForeColor = Color.White;
                    row.Cells["Edit"].Style.BackColor = Color.FromArgb(91, 155, 213);
                }
            }
        }
    }
}