using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AdministrareFisier;

namespace TaskManagerUI
{
    public partial class Form1 : Form
    {
        private DataGridView dgvTasks;
        private Button btnRefresh;
        private AdministrareClienti admin;
        private AdministrareTaskuri adminTaskuri;

        // Color palette
        private readonly Color LightPink = Color.FromArgb(255, 230, 230); 
        private readonly Color SoftPink = Color.FromArgb(255, 200, 200); 
        private readonly Color AccentPink = Color.FromArgb(255, 150, 150); 
        private readonly Color TextColor = Color.FromArgb(80, 60, 60); 

        public Form1()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            
            this.Text = "Task Manager";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = LightPink;
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);

            admin = new AdministrareClienti();
            adminTaskuri = new AdministrareTaskuri(admin);

            CreateDataGridView();
            CreateRefreshButton();
            LoadTaskData();
        }

        private void CreateDataGridView()
        {
            dgvTasks = new DataGridView
            {
                Name = "dgvTasks",
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackgroundColor = LightPink,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false
            };

            
            dgvTasks.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = AccentPink,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            
            dgvTasks.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = TextColor,
                SelectionBackColor = SoftPink,
                SelectionForeColor = TextColor,
                Padding = new Padding(3)
            };

            
            dgvTasks.AlternatingRowsDefaultCellStyle = null;

            
            dgvTasks.Columns.Add("Persoana", "Nume");
            dgvTasks.Columns.Add("Task", "Task");
            dgvTasks.Columns.Add("Stare", "Status");

            this.Controls.Add(dgvTasks);
        }

        private void CreateRefreshButton()
        {
            btnRefresh = new Button
            {
                Text = "Refresh Data",
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = AccentPink,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = SoftPink;
            btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 120, 120);

            btnRefresh.Click += (sender, e) => LoadTaskData();
            this.Controls.Add(btnRefresh);
        }

        private void LoadTaskData()
        {
            dgvTasks.Rows.Clear();

            try
            {
                foreach (var persoana in admin.GetPersoane())
                {
                    foreach (var task in persoana.GetTaskuri())
                    {
                        int rowIndex = dgvTasks.Rows.Add(
                            persoana.Nume,
                            task.Descriere,
                            task.EsteFinalizat ? "terminat" : "în așteptare"
                        );

                        
                        var statusCell = dgvTasks.Rows[rowIndex].Cells["Stare"];
                        statusCell.Style.BackColor = Color.White; 
                        statusCell.Style.ForeColor = TextColor;
                    }
                }

                if (dgvTasks.Rows.Count == 0)
                {
                    MessageBox.Show("Nu s-au găsit date. Verificați fișierele în directorul aplicației.",
                                    "Informație",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}",
                              "Eroare",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}