using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelBackground = new Panel();
            lblTitle = new Label();
            groupLogin = new Panel();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnRegister = new Button();
            panelBackground.SuspendLayout();
            groupLogin.SuspendLayout();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.BackColor = Color.FromArgb(245, 247, 250);
            panelBackground.Controls.Add(lblTitle);
            panelBackground.Controls.Add(groupLogin);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(500, 400);
            panelBackground.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(500, 80);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🔐 Connexion à GSB";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupLogin
            // 
            groupLogin.Anchor = AnchorStyles.None;
            groupLogin.BackColor = Color.White;
            groupLogin.Controls.Add(lblEmail);
            groupLogin.Controls.Add(txtEmail);
            groupLogin.Controls.Add(lblPassword);
            groupLogin.Controls.Add(txtPassword);
            groupLogin.Controls.Add(btnLogin);
            groupLogin.Controls.Add(btnRegister);
            groupLogin.Location = new Point(111, 110);
            groupLogin.MinimumSize = new Size(302, 238);
            groupLogin.Name = "groupLogin";
            groupLogin.Padding = new Padding(20);
            groupLogin.Size = new Size(302, 238);
            groupLogin.TabIndex = 1;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F);
            lblEmail.ForeColor = Color.Gray;
            lblEmail.Location = new Point(20, 25);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(99, 19);
            lblEmail.TabIndex = 0;
            lblEmail.Text = "Adresse e-mail";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(20, 45);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "ex: jean.dupont@gsb.com";
            txtEmail.Size = new Size(260, 25);
            txtEmail.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F);
            lblPassword.ForeColor = Color.Gray;
            lblPassword.Location = new Point(20, 85);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(92, 19);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Mot de passe";
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(20, 105);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '•';
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.Size = new Size(260, 25);
            txtPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(52, 152, 219);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(20, 150);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(260, 35);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Se connecter";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += login_Click;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.WhiteSmoke;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRegister.ForeColor = Color.FromArgb(52, 152, 219);
            btnRegister.Location = new Point(20, 190);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(260, 30);
            btnRegister.TabIndex = 5;
            btnRegister.Text = "📝 S’enregistrer";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // MainForm
            // 
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(500, 400);
            Controls.Add(panelBackground);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GSB Monolith — Connexion";
            panelBackground.ResumeLayout(false);
            groupLogin.ResumeLayout(false);
            groupLogin.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBackground;
        private Label lblTitle;
        private Panel groupLogin;
        private Label lblEmail;
        private Label lblPassword;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
    }
}
