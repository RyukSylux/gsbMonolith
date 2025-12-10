using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    public class MedicinesView : UserControl
    {
        private MedicineDAO medicineDAO = new MedicineDAO();
        private User currentUser;
        
        // UI Controls
        private DataGridView dgvMedicines;
        private Button btnAdd;
        private Panel headerPanel;
        
        // Edit/Add Panel
        private Panel editPanel;
        private TextBox txtName, txtDosage, txtDescription, txtMolecule;
        private Button btnSave, btnCancel;
        private int? editingId = null;

        // Dragging Logic
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public MedicinesView(User user)
        {
            currentUser = user;
            SetupUI();
            LoadMedicines();
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
                Text = "Gestion des Médicaments", 
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

            btnAdd = CreateButton("Nouveau Médicament", Color.FromArgb(0, 122, 204));
            btnAdd.Location = new Point(headerPanel.Width - 200, 15);
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Ancrage à droite important
            btnAdd.Click += (s, e) => ShowEditPanel(null);

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnDelete);
            headerPanel.Controls.Add(btnAdd);

            // --- Grid ---
            dgvMedicines = new DataGridView
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
            
            // Grid Styling
            dgvMedicines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvMedicines.ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray;
            dgvMedicines.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvMedicines.ColumnHeadersHeight = 40;
            dgvMedicines.DefaultCellStyle.Padding = new Padding(10);
            dgvMedicines.RowTemplate.Height = 40;

            // Enable Edit on Double Click
            dgvMedicines.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                    ShowEditPanel((int)dgvMedicines.Rows[e.RowIndex].Cells["Id_medicine"].Value);
            };

            // Context Menu
            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Modifier", null, (s, e) => {
                if(dgvMedicines.SelectedRows.Count > 0) 
                    ShowEditPanel((int)dgvMedicines.SelectedRows[0].Cells["Id_medicine"].Value);
            });
            ctxMenu.Items.Add("Supprimer", null, BtnDelete_Click);
            dgvMedicines.ContextMenuStrip = ctxMenu;


            // --- Edit Panel (Overlay) ---
            editPanel = new Panel 
            { 
                Size = new Size(400, 500), 
                BackColor = Color.White, 
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Header for dragging and closing
            Panel editHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(240, 240, 240) };
            editHeader.MouseDown += (s, e) => { isDragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = editPanel.Location; };
            editHeader.MouseMove += (s, e) => { if (isDragging) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); editPanel.Location = Point.Add(dragFormPoint, new Size(dif)); } };
            editHeader.MouseUp += (s, e) => { isDragging = false; };

            Label lblEditTitle = new Label { Text = "Détails Médicament", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
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
            
            txtName = CreateInput("Nom du médicament", 80);
            txtMolecule = CreateInput("Molécule", 140);
            txtDosage = CreateInput("Dosage", 200);
            txtDescription = CreateInput("Description", 260);

            btnSave = CreateButton("Enregistrer", Color.FromArgb(40, 167, 69));
            btnSave.Location = new Point(20, 340);
            btnSave.Width = 150;
            btnSave.Click += BtnSave_Click;

            btnCancel = CreateButton("Annuler", Color.Gray);
            btnCancel.Location = new Point(190, 340);
            btnCancel.Width = 150;
            btnCancel.Click += (s, e) => editPanel.Visible = false;

            editPanel.Controls.AddRange(new Control[] { txtName, txtMolecule, txtDosage, txtDescription, btnSave, btnCancel });

            // --- Assembly ---
            this.Controls.Add(editPanel); 
            this.Controls.Add(dgvMedicines);
            this.Controls.Add(headerPanel);

            // Responsive Edit Panel
            this.SizeChanged += (s, e) => CenterEditPanel();
        }

        private TextBox CreateInput(string placeholder, int y)
        {
            Label lbl = new Label { Text = placeholder, Location = new Point(20, y - 20), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9F) };
            TextBox txt = new TextBox { Location = new Point(20, y), Width = 340, Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            editPanel.Controls.Add(lbl);
            return txt;
        }

        private Button CreateButton(string text, Color color)
        {
            return new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(180, 35),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
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

        private void ShowEditPanel(int? id)
        {
            editingId = id;
            if (id.HasValue)
            {
                var med = medicineDAO.GetById(id.Value);
                if (med != null)
                {
                    txtName.Text = med.Name;
                    txtMolecule.Text = med.Molecule;
                    txtDosage.Text = med.Dosage;
                    txtDescription.Text = med.Description;
                }
            }
            else
            {
                txtName.Clear();
                txtMolecule.Clear();
                txtDosage.Clear();
                txtDescription.Clear();
            }
            editPanel.Visible = true;
            editPanel.BringToFront();
            CenterEditPanel();
        }

        private void LoadMedicines()
        {
            try
            {
                dgvMedicines.DataSource = medicineDAO.GetAll();
                if(dgvMedicines.Columns["Id_user"] != null) dgvMedicines.Columns["Id_user"].Visible = false;
                if(dgvMedicines.Columns["Id_medicine"] != null) dgvMedicines.Columns["Id_medicine"].Visible = false;
                if(dgvMedicines.Columns["User"] != null) dgvMedicines.Columns["User"].Visible = false;
            }
            catch(Exception ex) { MessageBox.Show("Erreur chargement: " + ex.Message); }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var med = new Medicine
            {
                Id_user = currentUser.Id,
                Name = txtName.Text,
                Molecule = txtMolecule.Text,
                Dosage = txtDosage.Text,
                Description = txtDescription.Text
            };

            if (editingId.HasValue)
            {
                med.Id_medicine = editingId.Value;
                medicineDAO.Update(med);
            }
            else
            {
                medicineDAO.Insert(med);
            }
            
            editPanel.Visible = false;
            LoadMedicines();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
             if(dgvMedicines.SelectedRows.Count == 0) return;
             
             if(MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes)
             {
                 int id = (int)dgvMedicines.SelectedRows[0].Cells["Id_medicine"].Value;
                 medicineDAO.Delete(id);
                 LoadMedicines();
             }
        }
    }
}