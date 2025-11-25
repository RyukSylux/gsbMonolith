using gsbMonolith.DAO;
using System;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    /// <summary>
    /// Represents the main login window of the application.
    /// Allows the user to authenticate and navigate to the user dashboard
    /// or to the registration form.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for the login button.
        /// Attempts to authenticate the user using the provided email and password.
        /// If successful, opens the user dashboard; otherwise shows an error message.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
        private void login_Click(object sender, EventArgs e)
        {
            var UserDao = new UserDAO();
            var connectedUser = UserDao.Login(txtEmail.Text, txtPassword.Text);

            if (connectedUser != null)
            {
                var userForm = new UserForm(connectedUser);
                userForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Échec de la connexion. Veuillez vérifier vos identifiants.");
            }

            Console.WriteLine(connectedUser?.FirstName.ToString());
        }

        /// <summary>
        /// Opens the registration form and hides the current login window.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            new RegisterForm().Show();
        }
    }
}
