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
            this.Padding = new Padding(0);
            this.AutoScaleMode = AutoScaleMode.Dpi; // Better scaling
            SetupModernUI();
        }

        private void SetupModernUI()
        {
            // Clear existing designer controls to avoid conflict
            this.Controls.Clear();

            // Window Settings
            this.FormBorderStyle = FormBorderStyle.Sizable; // Revert to standard sizable border
            this.Size = new Size(900, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.MaximizeBox = true;

            // --- Left Panel (Branding) ---
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.FromArgb(33, 43, 54) // Dark Sidebar color
            };
            
            Label lblLogo = new Label
            {
                Text = "GSB",
                Font = new Font("Segoe UI", 48F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(40, 180) // Approximate center
            };
            
            Label lblTagline = new Label
            {
                Text = "Gestion des Services\nBancaires & Médicaux",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(45, 270)
            };

            leftPanel.Controls.Add(lblLogo);
            leftPanel.Controls.Add(lblTagline);


            // --- Right Panel (Login Form) ---
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };

            // Main Layout Container (Centered)
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                BackColor = Color.White
            };
            
            // Columns: flexible spacer, fixed content (450px), flexible spacer
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 450F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Rows: flexible spacer, auto-sized content, flexible spacer
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // Content Panel (Holds the actual form controls)
            Panel contentPanel = new Panel
            {
                Size = new Size(450, 450), // Initial size, height will adjust
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top, // Fill the center cell width
                Padding = new Padding(10)
            };

            // Login Header
            Label lblLoginTitle = new Label
            {
                Text = "Connexion",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 43, 54),
                AutoSize = true,
                Location = new Point(10, 0)
            };

            // Input Fields (re-adjusted Y positions relative to 0)
            Panel pnlEmail = CreateModernInput("Email", txtEmail = new TextBox(), 80);
            Panel pnlPass = CreateModernInput("Mot de passe", txtPassword = new TextBox { PasswordChar = '•' }, 160);

            // Login Button
            btnLogin = new Button
            {
                Text = "SE CONNECTER",
                BackColor = Color.FromArgb(0, 122, 204), // GSB Blue
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(430, 45),
                Location = new Point(10, 260),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += login_Click;

            // Register Link (Only in Debug)
#if DEBUG
            Label lblNoAccount = new Label
            {
                Text = "Pas encore de compte ?",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(10, 325) // Adjusted Y position
            };

            btnRegister = new Button
            {
                Text = "Créer un compte",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Location = new Point(150, 320) // Adjusted Y position
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnRegister.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnRegister.Click += btnRegister_Click;
#endif

            // Add controls to content panel
            contentPanel.Controls.Add(lblLoginTitle);
            contentPanel.Controls.Add(pnlEmail);
            contentPanel.Controls.Add(pnlPass);
            contentPanel.Controls.Add(btnLogin);
#if DEBUG
            contentPanel.Controls.Add(lblNoAccount);
            contentPanel.Controls.Add(btnRegister);
#endif
            // Add content panel to the center cell of the table layout (Col 1, Row 1)
            mainLayout.Controls.Add(contentPanel, 1, 1);

            rightPanel.Controls.Add(mainLayout);

            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);
        }

        private Panel CreateModernInput(string labelText, TextBox txtBox, int yPos)
        {
            Panel container = new Panel { Location = new Point(10, yPos), Size = new Size(430, 60) }; // x=10 for alignment
            
            Label lbl = new Label 
            { 
                Text = labelText, 
                Font = new Font("Segoe UI", 9F, FontStyle.Bold), 
                ForeColor = Color.Gray, 
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Panel line = new Panel 
            { 
                Height = 2, 
                BackColor = Color.LightGray, 
                Dock = DockStyle.Bottom 
            };

            txtBox.BorderStyle = BorderStyle.None;
            txtBox.Font = new Font("Segoe UI", 11F);
            txtBox.Width = 430;
            txtBox.Location = new Point(0, 25);
            txtBox.BackColor = Color.White;

            // Focus Effect
            txtBox.Enter += (s, e) => { line.BackColor = Color.FromArgb(0, 122, 204); lbl.ForeColor = Color.FromArgb(0, 122, 204); };
            txtBox.Leave += (s, e) => { line.BackColor = Color.LightGray; lbl.ForeColor = Color.Gray; };

            container.Controls.Add(lbl);
            container.Controls.Add(txtBox);
            container.Controls.Add(line);
            
            return container;
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
#if DEBUG
            var reg = new RegisterForm();
            this.Hide(); // Hide MainForm
            reg.ShowDialog(); // Show RegisterForm as a dialog, blocking MainForm
            this.Show(); // Show MainForm again when RegisterForm is closed
#else
            MessageBox.Show("Registration is disabled in Release mode.", "Feature Disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
        }
    }
}