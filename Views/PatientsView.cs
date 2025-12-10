using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    public class PatientsView : UserControl
    {
        private PatientDAO patientDAO = new PatientDAO();
        private User currentUser;
        
        // UI Controls
        private DataGridView dgvPatients;
        private Panel headerPanel;
        private Button btnAdd;

        // Edit Panel
        private Panel editPanel;
        private TextBox txtName, txtFirstname, txtAge, txtGender;
        private Button btnSave, btnCancel;
        private int? editingId = null;

        // Dragging Logic
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public PatientsView(User user)
        {
            currentUser = user;
            SetupUI();
            LoadPatients();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10F);

            // --- Header ---
            headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(20) };
            
            var titleLabel = new Label 
            { 
                Text = "Gestion des Patients", 
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var btnDelete = new Button
            {
                Text = "Supprimer",
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 35),
                Cursor = Cursors.Hand,
                Location = new Point(headerPanel.Width - 370, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDelete.Click += BtnDelete_Click;

            btnAdd = new Button
            {
                Text = "Nouveau Patient",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(180, 35),
                Cursor = Cursors.Hand,
                Location = new Point(headerPanel.Width - 200, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAdd.Click += (s, e) => ShowEditPanel(null);

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnDelete);
            headerPanel.Controls.Add(btnAdd);

            // --- Grid ---
            dgvPatients = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            dgvPatients.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvPatients.ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray;
            dgvPatients.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPatients.ColumnHeadersHeight = 40;
            dgvPatients.DefaultCellStyle.Padding = new Padding(10);
            dgvPatients.RowTemplate.Height = 40;

            // Enable Edit on Double Click
            dgvPatients.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                    ShowEditPanel((int)dgvPatients.Rows[e.RowIndex].Cells["Id"].Value);
            };

            // Context Menu
            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Modifier", null, (s, e) => {
                if(dgvPatients.SelectedRows.Count > 0) 
                    ShowEditPanel((int)dgvPatients.SelectedRows[0].Cells["Id"].Value);
            });
            ctxMenu.Items.Add("Supprimer", null, BtnDelete_Click);
            dgvPatients.ContextMenuStrip = ctxMenu;

            // --- Edit Panel ---
            editPanel = new Panel 
            { 
                Size = new Size(400, 450), 
                BackColor = Color.White, 
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Header for dragging and closing
            Panel editHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(240, 240, 240) };
            editHeader.MouseDown += (s, e) => { isDragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = editPanel.Location; };
            editHeader.MouseMove += (s, e) => { if (isDragging) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); editPanel.Location = Point.Add(dragFormPoint, new Size(dif)); } };
            editHeader.MouseUp += (s, e) => { isDragging = false; };

            Label lblEditTitle = new Label { Text = "Fiche Patient", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
            editHeader.Controls.Add(lblEditTitle);

            Button btnCloseX = new Button 
            { 
                Text = "✕", 
                Dock = DockStyle.Right, 
                Width = 50, 
                FlatStyle = FlatStyle.Flat, 
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnCloseX.Click += (s, e) => editPanel.Visible = false;
            editHeader.Controls.Add(btnCloseX);

            editPanel.Controls.Add(editHeader);
            
            txtName = CreateInput("Nom", 80);
            txtFirstname = CreateInput("Prénom", 140);
            txtAge = CreateInput("Âge", 200);
            txtGender = CreateInput("Genre (M/F)", 260);

            btnSave = new Button 
            { 
                Text = "Enregistrer", 
                BackColor = Color.FromArgb(40, 167, 69), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(20, 320), 
                Size = new Size(150, 35) 
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button 
            { 
                Text = "Annuler", 
                BackColor = Color.Gray, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(190, 320), 
                Size = new Size(150, 35) 
            };
            btnCancel.Click += (s, e) => editPanel.Visible = false;

            editPanel.Controls.AddRange(new Control[] { txtName, txtFirstname, txtAge, txtGender, btnSave, btnCancel });

            // Assembly
            this.Controls.Add(editPanel);
            this.Controls.Add(dgvPatients);
            this.Controls.Add(headerPanel);

            this.SizeChanged += (s, e) => CenterEditPanel();
        }

        private TextBox CreateInput(string placeholder, int y)
        {
            Label lbl = new Label { Text = placeholder, Location = new Point(20, y - 20), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9F) };
            TextBox txt = new TextBox { Location = new Point(20, y), Width = 340, Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            editPanel.Controls.Add(lbl);
            editPanel.Controls.Add(txt);
            return txt;
        }

        private void CenterEditPanel()
        {
            if (editPanel != null)
            {
                editPanel.Location = new Point(
                    (this.Width - editPanel.Width) / 2,
                    (this.Height - editPanel.Height) / 2
                );
            }
        }

        private void LoadPatients()
        {
            try
            {
                dgvPatients.DataSource = patientDAO.GetAllPatients();
                if(dgvPatients.Columns["Id"] != null) dgvPatients.Columns["Id"].Visible = false;
                if(dgvPatients.Columns["Id_user"] != null) dgvPatients.Columns["Id_user"].Visible = false;
            }
            catch(Exception ex) { MessageBox.Show("Erreur: " + ex.Message); }
        }

        private void ShowEditPanel(int? id)
        {
            editingId = id;
            if (id.HasValue)
            {
                var p = patientDAO.GetPatientById(id.Value);
                if (p != null)
                {
                    txtName.Text = p.Name;
                    txtFirstname.Text = p.Firstname;
                    txtAge.Text = p.Age.ToString();
                    txtGender.Text = p.Gender;
                }
            }
            else
            {
                txtName.Clear();
                txtFirstname.Clear();
                txtAge.Clear();
                txtGender.Clear();
            }
            editPanel.Visible = true;
            editPanel.BringToFront();
            CenterEditPanel();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtAge.Text, out int age)) { MessageBox.Show("Âge invalide"); return; }
            
            var p = new Patient(
                editingId ?? 0, 
                currentUser.Id, 
                txtName.Text, 
                age, 
                txtFirstname.Text, 
                txtGender.Text
            );

            if (editingId.HasValue)
            {
                patientDAO.UpdatePatient(p);
            }
            else
            {
                patientDAO.Insert(p);
            }
            
            editPanel.Visible = false;
            LoadPatients();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
             if(dgvPatients.SelectedRows.Count == 0) return;
             
             if(MessageBox.Show("Supprimer ce patient ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
             {
                 int id = (int)dgvPatients.SelectedRows[0].Cells["Id"].Value;
                 patientDAO.DeletePatient(id);
                 LoadPatients();
             }
        }
    }
}