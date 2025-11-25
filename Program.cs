using gsbMonolith.DAO;
using gsbMonolith.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace gsbMonolith
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();
#if DEBUG
            AllocConsole();
            var UserDao = new UserDAO();
            var connectedUser = UserDao.Login("thomas.robert@clinic.fr", "password");
            if (connectedUser == null)
            {
                MessageBox.Show("Échec de la connexion. Veuillez vérifier vos identifiants.");
            }
            else
            {
                Application.Run(new Forms.UserForm(connectedUser));
            }
#else
            Application.Run(new Forms.MainForm());
#endif
        }
    }
}