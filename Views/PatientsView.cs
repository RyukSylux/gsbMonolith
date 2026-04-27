using gsbMonolith.DAO;
using gsbMonolith.Models;
using gsbMonolith.Views.Modals;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    /// <summary>
    /// Represents the view for managing patients, allowing users to view, add, edit, and delete patient records.
    /// </summary>
    public class PatientsView : UserControl
    {
        private PatientDAO patientDAO = new PatientDAO();
        private User currentUser;
        
        // UI Controls
        private DataGridView dgvPatients;
        private Panel headerPanel;
        private Button btnAdd;

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
            btnAdd.Click += (s, e) => OpenPatientModal(null);

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
            dgvPatients.DataBindingComplete += (s, e) => dgvPatients.ClearSelection();
            
            dgvPatients.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvPatients.ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray;
            dgvPatients.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPatients.ColumnHeadersHeight = 40;
            dgvPatients.DefaultCellStyle.Padding = new Padding(10);
            dgvPatients.RowTemplate.Height = 40;

            dgvPatients.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                {
                    int id = (int)dgvPatients.Rows[e.RowIndex].Cells["Id"].Value;
                    OpenPatientModal(patientDAO.GetPatientById(id));
                }
            };

            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Modifier", null, (s, e) => {
                if(dgvPatients.SelectedRows.Count > 0) 
                {
                    int id = (int)dgvPatients.SelectedRows[0].Cells["Id"].Value;
                    OpenPatientModal(patientDAO.GetPatientById(id));
                }
            });
            ctxMenu.Items.Add("Supprimer", null, BtnDelete_Click);
            dgvPatients.ContextMenuStrip = ctxMenu;

            // Assembly
            this.Controls.Add(dgvPatients);
            this.Controls.Add(headerPanel);
        }

        private void LoadPatients()
        {
            try
            {
                dgvPatients.DataSource = patientDAO.GetAllPatients();
                if(dgvPatients.Columns["Id"] != null) dgvPatients.Columns["Id"].Visible = false;
                if(dgvPatients.Columns["Id_user"] != null) dgvPatients.Columns["Id_user"].Visible = false;
                
                if (dgvPatients.Columns["Name"] != null) dgvPatients.Columns["Name"].HeaderText = "Nom";
                if (dgvPatients.Columns["Firstname"] != null) dgvPatients.Columns["Firstname"].HeaderText = "Prénom";
                if (dgvPatients.Columns["Age"] != null) dgvPatients.Columns["Age"].HeaderText = "Âge";
                if (dgvPatients.Columns["Gender"] != null) dgvPatients.Columns["Gender"].HeaderText = "Genre";
                if (dgvPatients.Columns["Doctor"] != null) dgvPatients.Columns["Doctor"].HeaderText = "Médecin";
            }
            catch(Exception ex) { MessageBox.Show("Erreur: " + ex.Message); }
        }

        private void OpenPatientModal(Patient patient)
        {
            using (var modal = new PatientEditForm(patient))
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var p = new Patient(
                            patient?.Id_patient ?? 0, 
                            currentUser.Id, 
                            modal.PatientName, 
                            modal.PatientAge, 
                            modal.PatientFirstName, 
                            modal.PatientGender
                        );

                        if (patient != null)
                            patientDAO.UpdatePatient(p);
                        else
                            patientDAO.Insert(p);
                        
                        LoadPatients();
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