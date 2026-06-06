using System;
using System.Drawing;
using System.Windows.Forms;
using gsbMonolith.Models;

namespace gsbMonolith.Views.Modals
{
    /// <summary>
    /// Form allowing system administrators to create or edit system users (Doctors/Administrators).
    /// Provides inputs for user details, password setup/change, and role assignment.
    /// </summary>
    public class UserEditForm : Form
    {
        /// <summary>
        /// Gets the entered last name of the user.
        /// </summary>
        public string UserName => txtName.Text;

        /// <summary>
        /// Gets the entered first name of the user.
        /// </summary>
        public string UserFirstName => txtFirstname.Text;

        /// <summary>
        /// Gets the entered email of the user.
        /// </summary>
        public string UserEmail => txtEmail.Text;

        /// <summary>
        /// Gets the entered password of the user.
        /// </summary>
        public string UserPassword => txtPassword.Text;

        /// <summary>
        /// Gets a value indicating whether the user is designated as an Administrator (true) or a standard user (false).
        /// </summary>
        public bool UserRole => chkRole.Checked;

        private TextBox txtName, txtFirstname, txtEmail, txtPassword;
        private CheckBox chkRole;
        private Button btnSave, btnCancel;
        private User _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditForm"/> class.
        /// </summary>
        /// <param name="user">The user to edit, or null to create a new user.</param>
        public UserEditForm(User user = null)
        {
            _user = user;
            SetupUI();
            if (_user != null)
            {
                this.Text = "Modifier Utilisateur";
                txtName.Text = _user.Name;
                txtFirstname.Text = _user.FirstName;
                txtEmail.Text = _user.Email;
                chkRole.Checked = _user.Role;
                btnSave.Text = "Enregistrer";
                txtPassword.PlaceholderText = "Laisser vide pour ne pas changer";
            }
            else
            {
                this.Text = "Nouvel Utilisateur";
                btnSave.Text = "Créer";
            }
        }

        private void SetupUI()
        {
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            Label lblHeader = new Label
            {
                Text = _user == null ? "Nouvel Utilisateur" : "Modifier l'Utilisateur",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblHeader);

            txtName = CreateInput("Nom", 80);
            txtFirstname = CreateInput("Prénom", 140);
            txtEmail = CreateInput("Email", 200);
            txtPassword = CreateInput("Mot de passe", 260);
            txtPassword.PasswordChar = '•';

            chkRole = new CheckBox { Text = "Administrateur", Location = new Point(20, 310), AutoSize = true };
            this.Controls.Add(chkRole);

            btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 380), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(200, 380), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { btnSave, btnCancel });
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private TextBox CreateInput(string placeholder, int y)
        {
            Label lbl = new Label { Text = placeholder, Location = new Point(20, y - 20), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9F) };
            TextBox txt = new TextBox { Location = new Point(20, y), Width = 340, Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            return txt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Veuillez remplir les champs obligatoires (Nom et Email).", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_user == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Le mot de passe est obligatoire pour un nouvel utilisateur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
