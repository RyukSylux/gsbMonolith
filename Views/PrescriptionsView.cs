using gsbMonolith.DAO;
using gsbMonolith.Models;
using gsbMonolith.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    public class PrescriptionsView : UserControl
    {
        private PrescriptionDAO prescriptionDAO = new PrescriptionDAO();
        private PatientDAO patientDAO = new PatientDAO();
        private MedicineDAO medicineDAO = new MedicineDAO();
        private UserDAO userDAO = new UserDAO();
        private User currentUser;
        
        // Main UI
        private DataGridView dgvPrescriptions;
        private Panel headerPanel;
        private Button btnExport, btnAdd;

        // Edit Panel UI
        private Panel editPanel;
        private ComboBox cmbPatients;
        private DateTimePicker dtpValidity;
        private ComboBox cmbMedicines;
        private NumericUpDown numQuantity;
        private Button btnAddMed, btnSave, btnCancel;
        private DataGridView dgvSelectedMeds;
        private int? editingPrescriptionId = null;

        // Temporary storage for meds in edit mode
        private List<MedicineSelection> currentMeds = new List<MedicineSelection>();

        private class MedicineSelection
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }

        public PrescriptionsView(User user)
        {
            currentUser = user;
            SetupUI();
            LoadPrescriptions();
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
                Text = "Prescriptions", 
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
                Location = new Point(headerPanel.Width - 550, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnDelete.Click += BtnDelete_Click;

            btnAdd = new Button
            {
                Text = "Nouvelle Prescription",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(180, 35),
                Location = new Point(headerPanel.Width - 380, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => ShowEditPanel(null);

            btnExport = new Button
            {
                Text = "Exporter PDF",
                BackColor = Color.SeaGreen, // Changed to Green for Export distinction
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 35),
                Location = new Point(headerPanel.Width - 170, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnExport.Click += BtnExport_Click;

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnDelete);
            headerPanel.Controls.Add(btnAdd);
            headerPanel.Controls.Add(btnExport);

            // --- Grid ---
            dgvPrescriptions = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPrescriptions.ColumnHeadersHeight = 40;
            dgvPrescriptions.DefaultCellStyle.Padding = new Padding(10);
            dgvPrescriptions.RowTemplate.Height = 40;

            dgvPrescriptions.CellDoubleClick += (s, e) => {
                if(e.RowIndex >= 0) ShowEditPanel((int)dgvPrescriptions.Rows[e.RowIndex].Cells["Id"].Value);
            };

            // Context Menu
            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Modifier", null, (s, e) => {
                 if(dgvPrescriptions.SelectedRows.Count > 0) 
                    ShowEditPanel((int)dgvPrescriptions.SelectedRows[0].Cells["Id"].Value);
            });
            ctx.Items.Add("Supprimer", null, BtnDelete_Click);
            dgvPrescriptions.ContextMenuStrip = ctx;


            // --- Edit Panel (Overlay) ---
            editPanel = new Panel 
            { 
                Size = new Size(600, 600), 
                BackColor = Color.White, 
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle 
            };
            
            Label lblEditTitle = new Label { Text = "Détails Prescription", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            
            // Patient & Date
            Label lblPatient = new Label { Text = "Patient", Location = new Point(20, 60), AutoSize = true, ForeColor = Color.Gray };
            cmbPatients = new ComboBox { Location = new Point(20, 80), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            
            Label lblDate = new Label { Text = "Date de validité", Location = new Point(300, 60), AutoSize = true, ForeColor = Color.Gray };
            dtpValidity = new DateTimePicker { Location = new Point(300, 80), Width = 250, Format = DateTimePickerFormat.Short };

            // Add Medicine Section
            GroupBox grpMeds = new GroupBox { Text = "Ajouter des médicaments", Location = new Point(20, 130), Size = new Size(540, 80), ForeColor = Color.Gray };
            
            cmbMedicines = new ComboBox { Location = new Point(20, 30), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            numQuantity = new NumericUpDown { Location = new Point(290, 30), Width = 80, Minimum = 1, Maximum = 100, Value = 1 };
            btnAddMed = new Button { Text = "Ajouter", Location = new Point(390, 29), Width = 100, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAddMed.Click += BtnAddMed_Click;

            grpMeds.Controls.Add(cmbMedicines);
            grpMeds.Controls.Add(numQuantity);
            grpMeds.Controls.Add(btnAddMed);

            // Meds List Grid
            dgvSelectedMeds = new DataGridView 
            { 
                Location = new Point(20, 230), 
                Size = new Size(540, 250),
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            // Remove med on double click
            dgvSelectedMeds.CellDoubleClick += (s, e) => {
                if(e.RowIndex >= 0) {
                    currentMeds.RemoveAt(e.RowIndex);
                    RefreshMedsGrid();
                }
            };

            // Actions
            btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 520), Size = new Size(150, 40) };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(190, 520), Size = new Size(150, 40) };
            btnCancel.Click += (s, e) => editPanel.Visible = false;

            editPanel.Controls.AddRange(new Control[] { lblEditTitle, lblPatient, cmbPatients, lblDate, dtpValidity, grpMeds, dgvSelectedMeds, btnSave, btnCancel });

            // Assembly
            this.Controls.Add(editPanel);
            this.Controls.Add(dgvPrescriptions);
            this.Controls.Add(headerPanel);

            this.SizeChanged += (s, e) => CenterEditPanel();
        }

        private void CenterEditPanel()
        {
            if (editPanel != null)
                editPanel.Location = new Point((this.Width - editPanel.Width) / 2, (this.Height - editPanel.Height) / 2);
        }

        private void LoadPrescriptions()
        {
            try
            {
                dgvPrescriptions.DataSource = prescriptionDAO.GetAllPrescriptions();
                if (dgvPrescriptions.Columns["Id"] != null) dgvPrescriptions.Columns["Id"].Visible = false;
            }
            catch(Exception ex) { MessageBox.Show("Erreur: " + ex.Message); }
        }

        private void LoadCombos()
        {
            // Patients
            cmbPatients.DataSource = patientDAO.GetPatientsForComboBox();
            cmbPatients.DisplayMember = "FullName";
            cmbPatients.ValueMember = "Id_patient";
            
            // Medicines
            cmbMedicines.DataSource = medicineDAO.GetAll(); // Assumes returns List<Medicine>
            cmbMedicines.DisplayMember = "Name";
            cmbMedicines.ValueMember = "Id_medicine";
        }

        private void ShowEditPanel(int? id)
        {
            LoadCombos();
            currentMeds.Clear();
            editingPrescriptionId = id;

            if (id.HasValue)
            {
                var p = prescriptionDAO.GetPrescriptionById(id.Value);
                if (p != null)
                {
                    cmbPatients.SelectedValue = p.Id_patient;
                    dtpValidity.Value = DateTime.Parse(p.Date);
                    
                    var details = prescriptionDAO.GetMedicinesWithQuantities(id.Value);
                    foreach(var d in details)
                    {
                        var m = medicineDAO.GetById(d.Id_medicine);
                        if(m != null)
                            currentMeds.Add(new MedicineSelection { Id = m.Id_medicine, Name = m.Name, Quantity = d.Quantity });
                    }
                }
            }
            else
            {
                dtpValidity.Value = DateTime.Now;
                if(cmbPatients.Items.Count > 0) cmbPatients.SelectedIndex = 0;
            }

            RefreshMedsGrid();
            editPanel.Visible = true;
            editPanel.BringToFront();
            CenterEditPanel();
        }

        private void RefreshMedsGrid()
        {
            dgvSelectedMeds.DataSource = null;
            dgvSelectedMeds.DataSource = currentMeds;
            dgvSelectedMeds.Columns["Id"].Visible = false;
            dgvSelectedMeds.Columns["Name"].HeaderText = "Médicament";
            dgvSelectedMeds.Columns["Quantity"].HeaderText = "Quantité";
        }

        private void BtnAddMed_Click(object sender, EventArgs e)
        {
            if (cmbMedicines.SelectedValue == null) return;
            
            int id = (int)cmbMedicines.SelectedValue;
            var medObj = cmbMedicines.SelectedItem as Medicine;
            string name = medObj?.Name ?? "Inconnu";
            int qty = (int)numQuantity.Value;

            var existing = currentMeds.FirstOrDefault(m => m.Id == id);
            if(existing != null)
            {
                existing.Quantity += qty;
            }
            else
            {
                currentMeds.Add(new MedicineSelection { Id = id, Name = name, Quantity = qty });
            }
            RefreshMedsGrid();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbPatients.SelectedValue == null) { MessageBox.Show("Sélectionnez un patient."); return; }
            if (currentMeds.Count == 0) { MessageBox.Show("Ajoutez au moins un médicament."); return; }

            int patientId = (int)cmbPatients.SelectedValue;
            string date = dtpValidity.Value.ToString("yyyy-MM-dd");
            
            var medList = currentMeds.Select(m => (m.Id, m.Quantity)).ToList();

            if (editingPrescriptionId.HasValue)
            {
                prescriptionDAO.UpdatePrescription(editingPrescriptionId.Value, date, medList);
            }
            else
            {
                var p = new Prescription(0, currentUser.Id, patientId, date);
                prescriptionDAO.CreatePrescriptionWithMedicines(p, medList);
            }

            editPanel.Visible = false;
            LoadPrescriptions();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPrescriptions.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Supprimer ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = (int)dgvPrescriptions.SelectedRows[0].Cells["Id"].Value;
                prescriptionDAO.DeletePrescription(id);
                LoadPrescriptions();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvPrescriptions.SelectedRows.Count == 0) { MessageBox.Show("Sélectionnez une ligne."); return; }

            try 
            {
                int idPresc = Convert.ToInt32(dgvPrescriptions.SelectedRows[0].Cells["Id"].Value);
                var presc = prescriptionDAO.GetPrescriptionById(idPresc);
                var patient = patientDAO.GetPatientById(presc.Id_patient);
                
                var meds = prescriptionDAO.GetMedicinesWithQuantities(idPresc)
                                          .Select(m => (Med: medicineDAO.GetById(m.Id_medicine), m.Quantity))
                                          .Where(x => x.Med != null)
                                          .Select(x => (x.Med!, x.Quantity))
                                          .ToList();

                var doctor = userDAO.GetUserById(presc.Id_user);

                PdfExporter.ExportPrescription(presc, patient, doctor, meds);
            }
            catch(Exception ex) { MessageBox.Show("Erreur PDF: " + ex.Message); }
        }
    }
}
