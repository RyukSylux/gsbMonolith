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

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

#if DEBUG
            AllocConsole();
#endif

            Form? formToRun = null;

#if DEBUG
            var userDao = new UserDAO();
            var connectedUser = userDao.Login("thomas.robert@clinic.fr", "password");
            if (connectedUser == null)
            {
                MessageBox.Show("Échec de la connexion. Veuillez vérifier vos identifiants.");
            }
            else
            {
                formToRun = new UserForm(connectedUser);
            }
#else
            formToRun = new MainForm();
#endif

            if (formToRun != null)
            {
                formToRun.FormClosed += (s, e) =>
                {
                    if (Application.OpenForms.Count == 0)
                        Application.ExitThread();
                };

                Application.Run(formToRun);
            }

#if DEBUG
            FreeConsole();
#endif
        }
    }
}