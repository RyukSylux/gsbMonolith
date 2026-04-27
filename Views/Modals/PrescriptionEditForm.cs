using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using gsbMonolith.Models;
using gsbMonolith.DAO;

namespace gsbMonolith.Views.Modals
{
    public class PrescriptionEditForm : Form
    {
        public int SelectedPatientId => (int)cmbPatients.SelectedValue;
        public string ValidityDate => dtpValidity.Value.ToString("yyyy-MM-dd");
        public List<(int Id, int Quantity)> SelectedMeds => _currentMeds.Select(m => (m.Id, m.Quantity)).ToList();

        private ComboBox cmbPatients;
        private DateTimePicker dtpValidity;
        private ComboBox cmbMedicines;
        private NumericUpDown numQuantity;
        private Button btnAddMed, btnSave, btnCancel;
        private DataGridView dgvSelectedMeds;
        private List<MedicineSelection> _currentMeds = new List<MedicineSelection>();
        
        private PatientDAO patientDAO = new PatientDAO();
        private MedicineDAO medicineDAO = new MedicineDAO();
        private Prescription _prescription;

        private class MedicineSelection
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }

        public PrescriptionEditForm(Prescription prescription = null, List<(int Id, int Quantity)> existingMeds = null)
        {
            _prescription = prescription;
            if (existingMeds != null)
            {
                foreach (var item in existingMeds)
                {
                    var m = medicineDAO.GetById(item.Id);
                    if (m != null)
                        _currentMeds.Add(new MedicineSelection { Id = m.Id_medicine, Name = m.Name, Quantity = item.Quantity });
                }
            }
            
            SetupUI();
            LoadCombos();

            if (_prescription != null)
            {
                this.Text = "Modifier Prescription";
                cmbPatients.SelectedValue = _prescription.Id_patient;
                dtpValidity.Value = DateTime.Parse(_prescription.Date);
                btnSave.Text = "Enregistrer";
            }
            else
            {
                this.Text = "Nouvelle Prescription";
                dtpValidity.Value = DateTime.Now;
                btnSave.Text = "Créer";
            }
            RefreshMedsGrid();
        }

        private void SetupUI()
        {
            this.Size = new Size(600, 650);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            Label lblPatient = new Label { Text = "Patient", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.Gray };
            cmbPatients = new ComboBox { Location = new Point(20, 40), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            
            Label lblDate = new Label { Text = "Date de validité", Location = new Point(300, 20), AutoSize = true, ForeColor = Color.Gray };
            dtpValidity = new DateTimePicker { Location = new Point(300, 40), Width = 250, Format = DateTimePickerFormat.Short };

            GroupBox grpMeds = new GroupBox { Text = "Ajouter des médicaments", Location = new Point(20, 90), Size = new Size(540, 80), ForeColor = Color.Gray };
            cmbMedicines = new ComboBox { Location = new Point(20, 30), Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };
            numQuantity = new NumericUpDown { Location = new Point(260, 30), Width = 60, Minimum = 1, Maximum = 100, Value = 1 };
            btnAddMed = new Button { Text = "Ajouter", Location = new Point(330, 25), Size = new Size(200, 40), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, UseCompatibleTextRendering = true };
            btnAddMed.Click += BtnAddMed_Click;
            grpMeds.Controls.AddRange(new Control[] { cmbMedicines, numQuantity, btnAddMed });

            dgvSelectedMeds = new DataGridView 
            { 
                Location = new Point(20, 190), 
                Size = new Size(540, 300),
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 45,
                EnableHeadersVisualStyles = false
            };
            dgvSelectedMeds.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvSelectedMeds.DataBindingComplete += (s, e) => dgvSelectedMeds.ClearSelection();

            dgvSelectedMeds.CellDoubleClick += (s, e) => {
                if(e.RowIndex >= 0) {
                    _currentMeds.RemoveAt(e.RowIndex);
                    RefreshMedsGrid();
                }
            };

            btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 530), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(200, 530), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblPatient, cmbPatients, lblDate, dtpValidity, grpMeds, dgvSelectedMeds, btnSave, btnCancel });
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void LoadCombos()
        {
            cmbPatients.DataSource = patientDAO.GetPatientsForComboBox();
            cmbPatients.DisplayMember = "FullName";
            cmbPatients.ValueMember = "Id_patient";
            
            cmbMedicines.DataSource = medicineDAO.GetAllMedicines();
            cmbMedicines.DisplayMember = "Name";
            cmbMedicines.ValueMember = "Id_medicine";
        }

        private void RefreshMedsGrid()
        {
            dgvSelectedMeds.DataSource = null;
            dgvSelectedMeds.DataSource = _currentMeds;
            
            if (dgvSelectedMeds.Columns["Id"] != null) dgvSelectedMeds.Columns["Id"].Visible = false;
            
            if (dgvSelectedMeds.Columns["Name"] != null) 
            {
                dgvSelectedMeds.Columns["Name"].HeaderText = "Médicament";
                dgvSelectedMeds.Columns["Name"].FillWeight = 70;
            }
            
            if (dgvSelectedMeds.Columns["Quantity"] != null) 
            {
                dgvSelectedMeds.Columns["Quantity"].HeaderText = "Quantité";
                dgvSelectedMeds.Columns["Quantity"].FillWeight = 30;
            }
        }

        private void BtnAddMed_Click(object sender, EventArgs e)
        {
            if (cmbMedicines.SelectedValue == null) return;
            int id = (int)cmbMedicines.SelectedValue;
            var medObj = cmbMedicines.SelectedItem as Medicine;
            string name = medObj?.Name ?? "Inconnu";
            int qty = (int)numQuantity.Value;

            var existing = _currentMeds.FirstOrDefault(m => m.Id == id);
            if(existing != null) existing.Quantity += qty;
            else _currentMeds.Add(new MedicineSelection { Id = id, Name = name, Quantity = qty });
            RefreshMedsGrid();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbPatients.SelectedValue == null) { MessageBox.Show("Sélectionnez un patient."); return; }
            if (_currentMeds.Count == 0) { MessageBox.Show("Ajoutez au moins un médicament."); return; }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
