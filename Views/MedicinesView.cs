using gsbMonolith.DAO;
using gsbMonolith.Models;
using gsbMonolith.Views.Modals;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    /// <summary>
    /// Represents the view for managing medicines, allowing users to view, add, edit, and delete medicine entries.
    /// </summary>
    public class MedicinesView : UserControl
    {
        private MedicineDAO medicineDAO = new MedicineDAO();
        private User currentUser;
        
        // UI Controls
        private DataGridView dgvMedicines;
        private Button btnAdd;
        private Panel headerPanel;

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
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.Click += (s, e) => OpenMedicineModal(null);

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
            dgvMedicines.DataBindingComplete += (s, e) => dgvMedicines.ClearSelection();
            
            dgvMedicines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvMedicines.ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray;
            dgvMedicines.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvMedicines.ColumnHeadersHeight = 40;
            dgvMedicines.DefaultCellStyle.Padding = new Padding(10);
            dgvMedicines.RowTemplate.Height = 40;

            dgvMedicines.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                {
                    int id = (int)dgvMedicines.Rows[e.RowIndex].Cells["Id_medicine"].Value;
                    OpenMedicineModal(medicineDAO.GetById(id));
                }
            };

            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Modifier", null, (s, e) => {
                if(dgvMedicines.SelectedRows.Count > 0) 
                {
                    int id = (int)dgvMedicines.SelectedRows[0].Cells["Id_medicine"].Value;
                    OpenMedicineModal(medicineDAO.GetById(id));
                }
            });
            ctxMenu.Items.Add("Supprimer", null, BtnDelete_Click);
            dgvMedicines.ContextMenuStrip = ctxMenu;

            // Assembly
            this.Controls.Add(dgvMedicines);
            this.Controls.Add(headerPanel);
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

        private void LoadMedicines()
        {
            try
            {
                dgvMedicines.DataSource = medicineDAO.GetAll();
                if(dgvMedicines.Columns["Id_user"] != null) dgvMedicines.Columns["Id_user"].Visible = false;
                if(dgvMedicines.Columns["Id_medicine"] != null) dgvMedicines.Columns["Id_medicine"].Visible = false;
                if(dgvMedicines.Columns["User"] != null) dgvMedicines.Columns["User"].Visible = false;
                
                if (dgvMedicines.Columns["Name"] != null) dgvMedicines.Columns["Name"].HeaderText = "Nom";
                if (dgvMedicines.Columns["Molecule"] != null) dgvMedicines.Columns["Molecule"].HeaderText = "Molécule";
                if (dgvMedicines.Columns["Dosage"] != null) dgvMedicines.Columns["Dosage"].HeaderText = "Dosage";
                if (dgvMedicines.Columns["Description"] != null) dgvMedicines.Columns["Description"].HeaderText = "Description";
            }
            catch(Exception ex) { MessageBox.Show("Erreur chargement: " + ex.Message); }
        }

        private void OpenMedicineModal(Medicine medicine)
        {
            using (var modal = new MedicineEditForm(medicine))
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var med = new Medicine
                        {
                            Id_user = currentUser.Id,
                            Name = modal.MedName,
                            Molecule = modal.MedMolecule,
                            Dosage = modal.MedDosage,
                            Description = modal.MedDescription
                        };

                        if (medicine != null)
                        {
                            med.Id_medicine = medicine.Id_medicine;
                            medicineDAO.Update(med);
                        }
                        else
                        {
                            medicineDAO.Insert(med);
                        }
                        
                        LoadMedicines();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement: " + ex.Message);
                    }
                }
            }
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