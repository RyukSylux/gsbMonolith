using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    /// <summary>
    /// Form used to display, add, refresh, and delete patients.
    /// Loads all patients from the database and associates new ones to the connected user.
    /// </summary>
    public partial class PatientsForm : Form
    {
        private readonly PatientDAO patientDAO = new PatientDAO();
        private readonly User currentUser;

        /// <summary>
        /// Initializes the patient management window and loads the existing patients.
        /// </summary>
        /// <param name="user">The currently logged user, linked to newly added patients.</param>
        public PatientsForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadPatients();
            dgvPatients.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }

        /// <summary>
        /// Loads all patients from the database and binds them to the DataGridView.
        /// Configures the column display and readability.
        /// </summary>
        private void LoadPatients()
        {
            try
            {
                var patients = patientDAO.GetAllPatients();
                dgvPatients.DataSource = patients;

                dgvPatients.Columns["Id"].HeaderText = "ID";
                dgvPatients.Columns["Firstname"].HeaderText = "Prénom";
                dgvPatients.Columns["Name"].HeaderText = "Nom";
                dgvPatients.Columns["Age"].HeaderText = "Âge";
                dgvPatients.Columns["Gender"].HeaderText = "Genre";
                dgvPatients.Columns["Doctor"].HeaderText = "Médecin référent";

                // Moves the “Doctor” column next to the ID
                dgvPatients.Columns["Doctor"].DisplayIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des patients : " + ex.Message);
            }
        }

        /// <summary>
        /// Refreshes the list of patients by reloading them from the database.
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatients();
        }

        /// <summary>
        /// Shows or hides the input panel used to add a new patient.
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            groupBoxAdd.Visible = !groupBoxAdd.Visible;
        }

        /// <summary>
        /// Creates a new patient using the values entered in the form and saves it to the database.
        /// If successful, refreshes the list and clears the fields.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Patient patient = new Patient
                {
                    Id_user = currentUser.Id,
                    Firstname = txtFirstname.Text,
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    Gender = cmbGender.SelectedItem?.ToString() ?? "Inconnu"
                };

                bool result = patientDAO.Insert(patient);

                if (result)
                {
                    MessageBox.Show("✅ Patient ajouté avec succès !");
                    LoadPatients();
                    groupBoxAdd.Visible = false;
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("❌ Erreur lors de l’ajout du patient.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes the selected patient after a confirmation message.
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un patient à supprimer.");
                return;
            }

            int id = (int)dgvPatients.SelectedRows[0].Cells["Id"].Value;

            if (MessageBox.Show("Voulez-vous vraiment supprimer ce patient ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                bool result = patientDAO.DeletePatient(id);
                if (result)
                {
                    MessageBox.Show("🗑️ Patient supprimé avec succès !");
                    LoadPatients();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.");
                }
            }
        }

        /// <summary>
        /// Clears the input fields used to create a new patient.
        /// </summary>
        private void ClearFields()
        {
            txtFirstname.Clear();
            txtName.Clear();
            txtAge.Clear();
            cmbGender.SelectedIndex = -1;
        }
    }
}
