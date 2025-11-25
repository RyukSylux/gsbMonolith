namespace gsbMonolith.Forms
{
    partial class UserForm
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
            Firstname_label = new Label();
            Role_label = new Label();
            Email_label = new Label();
            BtnPatients = new Button();
            BtnPrescriptions = new Button();
            BtnMedicines = new Button();
            Logout_button = new Button();
            dgvUsers = new DataGridView();
            gbModifyUser = new GroupBox();
            lblNom = new Label();
            lblPrenom = new Label();
            lblEmail = new Label();
            lblPassword = new Label();
            lblRole = new Label();
            txtNom = new TextBox();
            txtPrenom = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            chkRole = new CheckBox();
            btnSaveUser = new Button();
            btnNewUser = new Button();
            btnDeleteUser = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            gbModifyUser.SuspendLayout();
            SuspendLayout();
            // 
            // Firstname_label
            // 
            Firstname_label.AutoSize = true;
            Firstname_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            Firstname_label.Location = new Point(30, 30);
            Firstname_label.Name = "Firstname_label";
            Firstname_label.Size = new Size(130, 21);
            Firstname_label.TabIndex = 0;
            Firstname_label.Text = "Firstname_label";
            // 
            // Role_label
            // 
            Role_label.AutoSize = true;
            Role_label.Location = new Point(32, 70);
            Role_label.Name = "Role_label";
            Role_label.Size = new Size(30, 15);
            Role_label.TabIndex = 1;
            Role_label.Text = "Role";
            // 
            // Email_label
            // 
            Email_label.AutoSize = true;
            Email_label.Location = new Point(32, 95);
            Email_label.Name = "Email_label";
            Email_label.Size = new Size(36, 15);
            Email_label.TabIndex = 2;
            Email_label.Text = "Email";
            // 
            // BtnPatients
            // 
            BtnPatients.Font = new Font("Segoe UI", 10F);
            BtnPatients.Location = new Point(50, 150);
            BtnPatients.Name = "BtnPatients";
            BtnPatients.Size = new Size(200, 40);
            BtnPatients.TabIndex = 3;
            BtnPatients.Text = "👩‍⚕️ Gérer les patients";
            BtnPatients.UseVisualStyleBackColor = true;
            BtnPatients.Click += BtnPatients_Click;
            // 
            // BtnPrescriptions
            // 
            BtnPrescriptions.Font = new Font("Segoe UI", 10F);
            BtnPrescriptions.Location = new Point(50, 210);
            BtnPrescriptions.Name = "BtnPrescriptions";
            BtnPrescriptions.Size = new Size(200, 40);
            BtnPrescriptions.TabIndex = 4;
            BtnPrescriptions.Text = "💊 Gérer les prescriptions";
            BtnPrescriptions.UseVisualStyleBackColor = true;
            BtnPrescriptions.Click += BtnPrescriptions_Click;
            // 
            // BtnMedicines
            // 
            BtnMedicines.Font = new Font("Segoe UI", 10F);
            BtnMedicines.Location = new Point(50, 270);
            BtnMedicines.Name = "BtnMedicines";
            BtnMedicines.Size = new Size(200, 40);
            BtnMedicines.TabIndex = 5;
            BtnMedicines.Text = "\U0001f9ea Gérer les médicaments";
            BtnMedicines.UseVisualStyleBackColor = true;
            BtnMedicines.Click += BtnMedicines_Click;
            // 
            // Logout_button
            // 
            Logout_button.BackColor = Color.LightCoral;
            Logout_button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Logout_button.Location = new Point(650, 30);
            Logout_button.Name = "Logout_button";
            Logout_button.Size = new Size(120, 35);
            Logout_button.TabIndex = 7;
            Logout_button.Text = "Déconnexion";
            Logout_button.UseVisualStyleBackColor = false;
            Logout_button.Click += Logout_button_Click;
            // 
            // dgvUsers
            // 
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Location = new Point(256, 71);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.Size = new Size(514, 239);
            dgvUsers.TabIndex = 8;
            dgvUsers.Visible = false;
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
            // 
            // gbModifyUser
            // 
            gbModifyUser.Controls.Add(lblNom);
            gbModifyUser.Controls.Add(lblPrenom);
            gbModifyUser.Controls.Add(lblEmail);
            gbModifyUser.Controls.Add(lblPassword);
            gbModifyUser.Controls.Add(lblRole);
            gbModifyUser.Controls.Add(txtNom);
            gbModifyUser.Controls.Add(txtPrenom);
            gbModifyUser.Controls.Add(txtEmail);
            gbModifyUser.Controls.Add(txtPassword);
            gbModifyUser.Controls.Add(chkRole);
            gbModifyUser.Controls.Add(btnSaveUser);
            gbModifyUser.Controls.Add(btnNewUser);
            gbModifyUser.Controls.Add(btnDeleteUser);
            gbModifyUser.Location = new Point(30, 326);
            gbModifyUser.Name = "gbModifyUser";
            gbModifyUser.Size = new Size(740, 204);
            gbModifyUser.TabIndex = 9;
            gbModifyUser.TabStop = false;
            gbModifyUser.Text = "Modifier / Ajouter un utilisateur";
            // 
            // lblNom
            // 
            lblNom.Location = new Point(30, 24);
            lblNom.Name = "lblNom";
            lblNom.Size = new Size(100, 23);
            lblNom.TabIndex = 0;
            lblNom.Text = "Nom :";
            // 
            // lblPrenom
            // 
            lblPrenom.Location = new Point(30, 64);
            lblPrenom.Name = "lblPrenom";
            lblPrenom.Size = new Size(100, 23);
            lblPrenom.TabIndex = 1;
            lblPrenom.Text = "Prénom :";
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(30, 104);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 23);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email :";
            // 
            // lblPassword
            // 
            lblPassword.Location = new Point(370, 24);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(100, 23);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Mot de passe :";
            // 
            // lblRole
            // 
            lblRole.Location = new Point(370, 64);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(100, 23);
            lblRole.TabIndex = 4;
            lblRole.Text = "Administrateur :";
            // 
            // txtNom
            // 
            txtNom.Location = new Point(136, 24);
            txtNom.Name = "txtNom";
            txtNom.Size = new Size(184, 23);
            txtNom.TabIndex = 5;
            // 
            // txtPrenom
            // 
            txtPrenom.Location = new Point(136, 64);
            txtPrenom.Name = "txtPrenom";
            txtPrenom.Size = new Size(184, 23);
            txtPrenom.TabIndex = 6;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(136, 104);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(184, 23);
            txtEmail.TabIndex = 7;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(480, 24);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(200, 23);
            txtPassword.TabIndex = 8;
            // 
            // chkRole
            // 
            chkRole.Location = new Point(480, 64);
            chkRole.Name = "chkRole";
            chkRole.Size = new Size(104, 24);
            chkRole.TabIndex = 9;
            // 
            // btnSaveUser
            // 
            btnSaveUser.BackColor = Color.LightGreen;
            btnSaveUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSaveUser.Location = new Point(370, 140);
            btnSaveUser.Name = "btnSaveUser";
            btnSaveUser.Size = new Size(150, 35);
            btnSaveUser.TabIndex = 10;
            btnSaveUser.Text = "💾 Enregistrer";
            btnSaveUser.UseVisualStyleBackColor = false;
            btnSaveUser.Click += BtnSaveUser_Click;
            // 
            // btnNewUser
            // 
            btnNewUser.BackColor = Color.LightBlue;
            btnNewUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNewUser.Location = new Point(540, 140);
            btnNewUser.Name = "btnNewUser";
            btnNewUser.Size = new Size(140, 35);
            btnNewUser.TabIndex = 11;
            btnNewUser.Text = "➕ Nouveau";
            btnNewUser.UseVisualStyleBackColor = false;
            btnNewUser.Click += BtnNewUser_Click;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.BackColor = Color.LightCoral;
            btnDeleteUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDeleteUser.Location = new Point(200, 140);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(150, 35);
            btnDeleteUser.TabIndex = 12;
            btnDeleteUser.Text = "🗑️ Supprimer";
            btnDeleteUser.UseVisualStyleBackColor = false;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            // 
            // UserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 542);
            Controls.Add(gbModifyUser);
            Controls.Add(dgvUsers);
            Controls.Add(Logout_button);
            Controls.Add(BtnMedicines);
            Controls.Add(BtnPrescriptions);
            Controls.Add(BtnPatients);
            Controls.Add(Email_label);
            Controls.Add(Role_label);
            Controls.Add(Firstname_label);
            MinimumSize = new Size(816, 581);
            Name = "UserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Espace utilisateur GSB";
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            gbModifyUser.ResumeLayout(false);
            gbModifyUser.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Firstname_label;
        private Label Role_label;
        private Label Email_label;
        private Button BtnPatients;
        private Button BtnPrescriptions;
        private Button BtnMedicines;
        private Button Logout_button;
        private DataGridView dgvUsers;
        private GroupBox gbModifyUser;
        private TextBox txtNom;
        private TextBox txtPrenom;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private CheckBox chkRole;
        private Button btnSaveUser;
        private Button btnNewUser;
        private Button btnDeleteUser;
        private Label lblNom;
        private Label lblPrenom;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblRole;
    }
}
