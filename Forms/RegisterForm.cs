using gsbMonolith.DAO;
using System;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly UserDAO userDAO = new UserDAO();

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string firstname = txtFirstname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(firstname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = userDAO.Register(email, password, name, firstname);

            if (success)
            {
                MessageBox.Show("Compte créé avec succès ! Vous pouvez maintenant vous connecter.",
                    "Inscription réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Retour à la page de connexion
                this.Hide();
                new MainForm().Show();
            }
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainForm().Show();
        }
    }
}
