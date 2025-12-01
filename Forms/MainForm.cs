using gsbMonolith.DAO;
using System;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void login_Click(object sender, EventArgs e)
        {
            var UserDao = new UserDAO();
            var connectedUser = UserDao.Login(txtEmail.Text, txtPassword.Text);

            if (connectedUser != null)
            {
                var userForm = new UserForm(connectedUser);
                userForm.FormClosed += (s2, e2) => this.Close();
                userForm.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show("Échec de la connexion. Veuillez vérifier vos identifiants.");
            }

            Console.WriteLine(connectedUser?.FirstName.ToString());
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            var reg = new RegisterForm();
            reg.FormClosed += (s2, e2) => this.Close();
            reg.Show();
            this.Hide();
            new RegisterForm().Show();
        }
    }
}