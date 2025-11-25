using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    partial class RegisterForm
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
            panelRegister = new Panel();
            lblName = new Label();
            txtName = new TextBox();
            lblFirstname = new Label();
            txtFirstname = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnRegister = new Button();
            btnBackToLogin = new Button();
            panelBackground.SuspendLayout();
            panelRegister.SuspendLayout();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.BackColor = Color.FromArgb(245, 247, 250);
            panelBackground.Controls.Add(lblTitle);
            panelBackground.Controls.Add(panelRegister);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(520, 480);
            panelBackground.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(520, 80);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📝 Créer un compte GSB";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelRegister
            // 
            panelRegister.Anchor = AnchorStyles.None;
            panelRegister.BackColor = Color.White;
            panelRegister.Controls.Add(lblName);
            panelRegister.Controls.Add(txtName);
            panelRegister.Controls.Add(lblFirstname);
            panelRegister.Controls.Add(txtFirstname);
            panelRegister.Controls.Add(lblEmail);
            panelRegister.Controls.Add(txtEmail);
            panelRegister.Controls.Add(lblPassword);
            panelRegister.Controls.Add(txtPassword);
            panelRegister.Controls.Add(btnRegister);
            panelRegister.Controls.Add(btnBackToLogin);
            panelRegister.Location = new Point(100, 110);
            panelRegister.MinimumSize = new Size(320, 320);
            panelRegister.Name = "panelRegister";
            panelRegister.Padding = new Padding(20);
            panelRegister.Size = new Size(320, 320);
            panelRegister.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 10F);
            lblName.ForeColor = Color.Gray;
            lblName.Location = new Point(20, 20);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 19);
            lblName.TabIndex = 0;
            lblName.Text = "Nom";
            // 
            // txtName
            // 
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Font = new Font("Segoe UI", 10F);
            txtName.Location = new Point(20, 40);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Dupont";
            txtName.Size = new Size(260, 25);
            txtName.TabIndex = 1;
            // 
            // lblFirstname
            // 
            lblFirstname.AutoSize = true;
            lblFirstname.Font = new Font("Segoe UI", 10F);
            lblFirstname.ForeColor = Color.Gray;
            lblFirstname.Location = new Point(20, 75);
            lblFirstname.Name = "lblFirstname";
            lblFirstname.Size = new Size(57, 19);
            lblFirstname.TabIndex = 2;
            lblFirstname.Text = "Prénom";
            // 
            // txtFirstname
            // 
            txtFirstname.BorderStyle = BorderStyle.FixedSingle;
            txtFirstname.Font = new Font("Segoe UI", 10F);
            txtFirstname.Location = new Point(20, 95);
            txtFirstname.Name = "txtFirstname";
            txtFirstname.PlaceholderText = "Jean";
            txtFirstname.Size = new Size(260, 25);
            txtFirstname.TabIndex = 3;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F);
            lblEmail.ForeColor = Color.Gray;
            lblEmail.Location = new Point(20, 130);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(99, 19);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Adresse e-mail";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(20, 150);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "ex: jean.dupont@gsb.com";
            txtEmail.Size = new Size(260, 25);
            txtEmail.TabIndex = 5;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F);
            lblPassword.ForeColor = Color.Gray;
            lblPassword.Location = new Point(20, 190);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(92, 19);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Mot de passe";
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(20, 210);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '•';
            txtPassword.PlaceholderText = "••••••••";
            txtPassword.Size = new Size(260, 25);
            txtPassword.TabIndex = 7;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(52, 152, 219);
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(20, 250);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(260, 35);
            btnRegister.TabIndex = 8;
            btnRegister.Text = "Créer un compte";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += BtnRegister_Click;
            // 
            // btnBackToLogin
            // 
            btnBackToLogin.BackColor = Color.FromArgb(230, 230, 230);
            btnBackToLogin.Cursor = Cursors.Hand;
            btnBackToLogin.FlatAppearance.BorderSize = 0;
            btnBackToLogin.FlatStyle = FlatStyle.Flat;
            btnBackToLogin.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            btnBackToLogin.ForeColor = Color.FromArgb(52, 73, 94);
            btnBackToLogin.Location = new Point(20, 290);
            btnBackToLogin.Name = "btnBackToLogin";
            btnBackToLogin.Size = new Size(260, 25);
            btnBackToLogin.TabIndex = 9;
            btnBackToLogin.Text = "← Retour à la connexion";
            btnBackToLogin.UseVisualStyleBackColor = false;
            btnBackToLogin.Click += BtnBackToLogin_Click;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(520, 480);
            Controls.Add(panelBackground);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GSB Monolith — Inscription";
            panelBackground.ResumeLayout(false);
            panelRegister.ResumeLayout(false);
            panelRegister.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBackground;
        private Label lblTitle;
        private Panel panelRegister;
        private Label lblName;
        private TextBox txtName;
        private Label lblFirstname;
        private TextBox txtFirstname;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnRegister;
        private Button btnBackToLogin;
    }
}
