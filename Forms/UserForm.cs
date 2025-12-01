using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    public partial class UserForm : Form
    {
        private readonly User connectedUser;
        private int? selectedUserId = null;
        public UserForm(User user)
        {
            InitializeComponent();
            connectedUser = user;
            LoadUserData();
            dvgUsersLoadContent();
        }
        private void LoadUserData()
        {
            Firstname_label.Text = $"Bienvenue {connectedUser.FirstName} {connectedUser.Name} 👋";

            if (!connectedUser.Role)
                Role_label.Text = "Rôle : Médecin / Prescripteur";
            else
                Role_label.Text = "Rôle : Administrateur";

            btnDeleteUser.Visible = connectedUser.Role;
            Email_label.Text = $"Email : {connectedUser.Email}";
        }
        private void dvgUsersLoadContent()
        {
            if (connectedUser.Role)
            {
                var Users = new UserDAO();
                dgvUsers.Visible = true;
                dgvUsers.AutoGenerateColumns = true;
                dgvUsers.DataSource = Users.GetAllUsers();

                if (dgvUsers.Columns["Id"] != null)
                    dgvUsers.Columns["Id"].Visible = false;

                if (dgvUsers.Columns["Password"] != null)
                    dgvUsers.Columns["Password"].Visible = false;

                dgvUsers.Columns["Name"].HeaderText = "Nom";
                dgvUsers.Columns["FirstName"].HeaderText = "Prénom";
                dgvUsers.Columns["Email"].HeaderText = "E-mail";
                dgvUsers.Columns["Role"].HeaderText = "Administrateur ?";
            }
        }
        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                var row = dgvUsers.SelectedRows[0];
                selectedUserId = Convert.ToInt32(row.Cells["Id"].Value);
                txtNom.Text = row.Cells["Name"].Value.ToString();
                txtPrenom.Text = row.Cells["FirstName"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                chkRole.Checked = Convert.ToBoolean(row.Cells["Role"].Value);
                txtPassword.Text = "";
            }
        }
        private void BtnNewUser_Click(object sender, EventArgs e)
        {
            selectedUserId = null;
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            chkRole.Checked = false;
            dgvUsers.ClearSelection();
        }
        private void BtnSaveUser_Click(object sender, EventArgs e)
        {
            var dao = new UserDAO();

            string email = txtEmail.Text.Trim();
            string name = txtNom.Text.Trim();
            string firstname = txtPrenom.Text.Trim();
            string password = txtPassword.Text.Trim();
            bool role = chkRole.Checked;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(firstname))
            {
                MessageBox.Show("Veuillez remplir tous les champs requis (nom, prénom, email).", "Champs manquants", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success;
            if (selectedUserId == null)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Le mot de passe est requis pour créer un utilisateur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                success = dao.CreateUser(email, password, name, firstname, role);
            }
            else
            {
                var updatedUser = new User((int)selectedUserId, name, firstname, role, email);
                success = dao.UpdateUser(updatedUser);
            }

            if (success)
            {
                MessageBox.Show("✅ Enregistrement effectué avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dvgUsersLoadContent();
                BtnNewUser_Click(null, null);
            }
        }
        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à supprimer.", "Aucun utilisateur sélectionné", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Voulez-vous vraiment supprimer cet utilisateur ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var dao = new UserDAO();
                bool success = dao.DeleteUser((int)selectedUserId);

                if (success)
                {
                    MessageBox.Show("✅ Utilisateur supprimé avec succès.", "Suppression réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dvgUsersLoadContent();
                    BtnNewUser_Click(null, null);
                }
            }
        }
        private void Logout_button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Voulez-vous vous déconnecter ?", "Déconnexion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
#if DEBUG
                this.Hide();
#else
                this.Close();
#endif
                MainForm loginForm = new MainForm();
                loginForm.Show();
            }
        }
        private void BtnPatients_Click(object sender, EventArgs e)
        {
            PatientsForm f = new PatientsForm(connectedUser);
            f.Show();
        }
        private void BtnPrescriptions_Click(object sender, EventArgs e)
        {
            PrescriptionsForm f = new PrescriptionsForm(connectedUser);
            f.Show();
        }
        private void BtnMedicines_Click(object sender, EventArgs e)
        {
            MedicinesForm f = new MedicinesForm(connectedUser);
            f.Show();
        }
        private void DgvUsers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int totalHeight = dgvUsers.ColumnHeadersHeight;
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                totalHeight += row.Height;
            }
            dgvUsers.Height = totalHeight + 2;
        }
    }
}
