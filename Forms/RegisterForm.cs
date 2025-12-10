using gsbMonolith.DAO;
using System;
using System.Windows.Forms;

#if DEBUG
namespace gsbMonolith.Forms
{
        public partial class RegisterForm : Form
        {
            private readonly UserDAO userDAO = new UserDAO();
    
            public RegisterForm()
            {
                InitializeComponent();
                this.Padding = new Padding(0);
                this.AutoScaleMode = AutoScaleMode.Dpi;
                SetupModernUI();
            }
    
            private void SetupModernUI()
            {
                            this.Controls.Clear();
                            this.FormBorderStyle = FormBorderStyle.Sizable; // Standard OS borders
                            this.Size = new Size(1000, 600);
                            this.StartPosition = FormStartPosition.CenterScreen;
                            this.Text = "GSB - Inscription";
                            this.BackColor = Color.White;
                            this.MaximizeBox = true;    
                // --- Left Panel (Branding) ---
                Panel leftPanel = new Panel
                {
                    Dock = DockStyle.Left,
                    Width = 350,
                    BackColor = Color.FromArgb(33, 43, 54)
                };
    
                Label lblLogo = new Label
                {
                    Text = "GSB",
                    Font = new Font("Segoe UI", 48F, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(40, 200)
                };
    
                Label lblTagline = new Label
                {
                    Text = "Rejoignez le réseau\nGSB Medical",
                    Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                    ForeColor = Color.LightGray,
                    AutoSize = true,
                    Location = new Point(45, 290)
                };
    
                leftPanel.Controls.Add(lblLogo);
                leftPanel.Controls.Add(lblTagline);
    
                // --- Right Panel (Form) ---
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
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 450F));
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    
                // Content Panel
                Panel contentPanel = new Panel
                {
                    Size = new Size(450, 550),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Dock = DockStyle.Top,
                    Padding = new Padding(10)
                };
    
                // Title
                Label lblTitle = new Label
                {
                    Text = "Créer un compte",
                    Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(33, 43, 54),
                    AutoSize = true,
                    Location = new Point(10, 0)
                };
    
                // Inputs
                Panel pnlName = CreateModernInput("Nom", txtName = new TextBox(), 70);
                Panel pnlFirst = CreateModernInput("Prénom", txtFirstname = new TextBox(), 140);
                Panel pnlEmail = CreateModernInput("Email", txtEmail = new TextBox(), 210);
                Panel pnlPass = CreateModernInput("Mot de passe", txtPassword = new TextBox { PasswordChar = '•' }, 280);
    
                // Buttons
                btnRegister = new Button
                {
                    Text = "S'INSCRIRE",
                    BackColor = Color.FromArgb(40, 167, 69), // Green for Register
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Size = new Size(430, 45),
                    Location = new Point(10, 370),
                    Cursor = Cursors.Hand
                };
                btnRegister.FlatAppearance.BorderSize = 0;
                btnRegister.Click += BtnRegister_Click;
    
                btnBackToLogin = new Button
                {
                    Text = "Retour à la connexion",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(430, 35),
                    Location = new Point(10, 430),
                    Cursor = Cursors.Hand
                };
                btnBackToLogin.FlatAppearance.BorderSize = 0;
                btnBackToLogin.Click += BtnBackToLogin_Click;
    
                contentPanel.Controls.AddRange(new Control[] { lblTitle, pnlName, pnlFirst, pnlEmail, pnlPass, btnRegister, btnBackToLogin });
                
                mainLayout.Controls.Add(contentPanel, 1, 1);
                rightPanel.Controls.Add(mainLayout);
    
                this.Controls.Add(rightPanel);
                this.Controls.Add(leftPanel);
            }
    
            private Panel CreateModernInput(string labelText, TextBox txtBox, int yPos)
            {
                Panel container = new Panel { Location = new Point(10, yPos), Size = new Size(430, 60) };
                
                Label lbl = new Label 
                { 
                    Text = labelText, 
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold), 
                    ForeColor = Color.Gray, 
                    AutoSize = true,
                    Location = new Point(0, 0)
                };
    
                Panel line = new Panel { Height = 2, BackColor = Color.LightGray, Dock = DockStyle.Bottom };
    
                txtBox.BorderStyle = BorderStyle.None;
                txtBox.Font = new Font("Segoe UI", 11F);
                txtBox.Width = 430;
                txtBox.Location = new Point(0, 25);
                txtBox.BackColor = Color.White;
    
                txtBox.Enter += (s, e) => { line.BackColor = Color.FromArgb(40, 167, 69); lbl.ForeColor = Color.FromArgb(40, 167, 69); };
                txtBox.Leave += (s, e) => { line.BackColor = Color.LightGray; lbl.ForeColor = Color.Gray; };
    
                container.Controls.Add(lbl);
                container.Controls.Add(txtBox);
                container.Controls.Add(line);
                
                return container;
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
                this.Close();
            }
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
#endif