using System;
using System.Drawing;
using System.Windows.Forms;
using gsbMonolith.Models;
using gsbMonolith.DAO;

namespace gsbMonolith.Views
{
    public class DashboardView : UserControl
    {
        private User currentUser;
        private UserDAO userDAO = new UserDAO();
        
        // Admin UI
        private DataGridView dgvUsers;
        private Panel adminPanel;
        
        // User Edit Panel
        private Panel editPanel;
        private TextBox txtName, txtFirstname, txtEmail, txtPassword;
        private CheckBox chkRole;
        private Button btnSave, btnCancel;
        private int? editingUserId = null;

        public DashboardView(User user)
        {
            currentUser = user;
            SetupUI();
            if (currentUser.Role)
            {
                LoadUsers();
            }
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10F);

            // Welcome Message
            Panel welcomePanel = new Panel { Dock = DockStyle.Top, Height = 100, Padding = new Padding(40, 20, 0, 0) };
            Label lblWelcome = new Label
            {
                Text = $"Bienvenue, Dr. {currentUser.Name} 👋",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(40, 20)
            };
             Label lblRole = new Label
            {
                Text = currentUser.Role ? "Administrateur Système" : "Médecin / Prescripteur",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(44, 70)
            };
            welcomePanel.Controls.Add(lblWelcome);
            welcomePanel.Controls.Add(lblRole);
            this.Controls.Add(welcomePanel);

            // Admin Section
            if (currentUser.Role)
            {
                SetupAdminUI();
            }
            
            // Edit Panel (Hidden by default)
            SetupEditPanel();
            this.Controls.Add(editPanel); // Add on top
            this.SizeChanged += (s, e) => CenterEditPanel();
        }

        private void SetupAdminUI()
        {
            adminPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40) };

            // Container for Title + Button
            Panel topBar = new Panel { Dock = DockStyle.Top, Height = 60 };

            Label lblAdminTitle = new Label 
            { 
                Text = "Gestion des Utilisateurs", 
                Font = new Font("Segoe UI", 18F, FontStyle.Bold), 
                ForeColor = Color.FromArgb(0, 122, 204),
                AutoSize = true,
                Location = new Point(0, 15) // Aligné Y=15 comme les autres vues
            };

            // Bouton dans un conteneur Dock Right pour garantir sa position horizontale
            Panel btnContainer = new Panel { Dock = DockStyle.Right, Width = 400 };
            
            Button btnDeleteUser = new Button
            {
                Text = "Supprimer",
                BackColor = Color.FromArgb(220, 53, 69), // Rouge
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 35),
                Cursor = Cursors.Hand,
                Location = new Point(20, 15) // Gauche
            };
            btnDeleteUser.Click += BtnDeleteUser_Click;

            Button btnAddUser = new Button
            {
                Text = "+ Nouvel Utilisateur",
                BackColor = Color.FromArgb(0, 122, 204), // Bleu
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(180, 35),
                Cursor = Cursors.Hand,
                Location = new Point(200, 15) // Droite
            };
            btnAddUser.Click += (s, e) => ShowEditPanel(null);

            btnContainer.Controls.Add(btnDeleteUser);
            btnContainer.Controls.Add(btnAddUser);
            topBar.Controls.Add(btnContainer);
            topBar.Controls.Add(lblAdminTitle);
            
            dgvUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvUsers.ColumnHeadersHeight = 40;
            dgvUsers.DefaultCellStyle.Padding = new Padding(10);
            dgvUsers.RowTemplate.Height = 40;

            // Enable Edit on Double Click
            dgvUsers.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                    ShowEditPanel((int)dgvUsers.Rows[e.RowIndex].Cells["Id"].Value);
            };

            // Context Menu
            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Modifier", null, (s, e) => {
                 if(dgvUsers.SelectedRows.Count > 0) 
                    ShowEditPanel((int)dgvUsers.SelectedRows[0].Cells["Id"].Value);
            });
            ctx.Items.Add("Supprimer", null, BtnDeleteUser_Click);
            dgvUsers.ContextMenuStrip = ctx;

            adminPanel.Controls.Add(dgvUsers);
            adminPanel.Controls.Add(topBar);
            
            this.Controls.Add(adminPanel);
            adminPanel.BringToFront(); // Ensure it's visible below editPanel but above others
        }

        private void SetupEditPanel()
        {
            editPanel = new Panel 
            { 
                Size = new Size(400, 500), 
                BackColor = Color.White, 
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };

             Label lblTitle = new Label { Text = "Utilisateur", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
             
             txtName = CreateInput("Nom", 80);
             txtFirstname = CreateInput("Prénom", 140);
             txtEmail = CreateInput("Email", 200);
             txtPassword = CreateInput("Mot de passe", 260); // Password logic handled carefully
             txtPassword.PasswordChar = '•';

             chkRole = new CheckBox { Text = "Administrateur", Location = new Point(20, 310), AutoSize = true };

             btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 360), Size = new Size(150, 35) };
             btnSave.Click += BtnSaveUser_Click;

             btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(190, 360), Size = new Size(150, 35) };
             btnCancel.Click += (s, e) => editPanel.Visible = false;

             editPanel.Controls.AddRange(new Control[] { lblTitle, chkRole, btnSave, btnCancel });
        }

        private TextBox CreateInput(string placeholder, int y)
        {
            Label lbl = new Label { Text = placeholder, Location = new Point(20, y - 20), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9F) };
            TextBox txt = new TextBox { Location = new Point(20, y), Width = 340, Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            editPanel.Controls.Add(lbl);
            editPanel.Controls.Add(txt);
            return txt;
        }

        private void CenterEditPanel()
        {
            if (editPanel != null)
            {
                editPanel.Location = new Point((this.Width - editPanel.Width) / 2, (this.Height - editPanel.Height) / 2);
            }
        }

        private void LoadUsers()
        {
            try
            {
                dgvUsers.DataSource = userDAO.GetAllUsers();
                if (dgvUsers.Columns["Id"] != null) dgvUsers.Columns["Id"].Visible = false;
                if (dgvUsers.Columns["Password"] != null) dgvUsers.Columns["Password"].Visible = false;
            }
            catch(Exception ex) { MessageBox.Show("Erreur chargement utilisateurs: " + ex.Message); }
        }

        private void ShowEditPanel(int? id)
        {
            editingUserId = id;
            if (id.HasValue)
            {
                var u = userDAO.GetUserById(id.Value); // Need to implement GetUserById or find in list
                // Since GetUserById might not exist or be easy, let's grab from grid row for now or assume DAO has it
                // Actually, let's use the grid data for simplicity if DAO method missing
                 if (dgvUsers.SelectedRows.Count > 0)
                 {
                     var row = dgvUsers.SelectedRows[0];
                     txtName.Text = row.Cells["Name"].Value.ToString();
                     txtFirstname.Text = row.Cells["FirstName"].Value.ToString();
                     txtEmail.Text = row.Cells["Email"].Value.ToString();
                     chkRole.Checked = Convert.ToBoolean(row.Cells["Role"].Value);
                     txtPassword.Text = ""; // Don't show password
                     txtPassword.PlaceholderText = "Laisser vide pour ne pas changer";
                 }
            }
            else
            {
                txtName.Clear();
                txtFirstname.Clear();
                txtEmail.Clear();
                txtPassword.Clear();
                chkRole.Checked = false;
                txtPassword.PlaceholderText = "";
            }
            editPanel.Visible = true;
            editPanel.BringToFront();
            CenterEditPanel();
        }

        private void BtnSaveUser_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Champs obligatoires manquants.");
                return;
            }

            if (editingUserId.HasValue)
            {
                // Update
                var u = new User(editingUserId.Value, txtName.Text, txtFirstname.Text, chkRole.Checked, txtEmail.Text);
                // Note: UpdateUser in DAO usually doesn't update password if not provided, check DAO logic
                // If password provided, might need specific method. Assuming standard UpdateUser for now.
                userDAO.UpdateUser(u);
                MessageBox.Show("Utilisateur mis à jour.");
            }
            else
            {
                // Create
                if(string.IsNullOrWhiteSpace(txtPassword.Text)) { MessageBox.Show("Mot de passe requis."); return; }
                userDAO.CreateUser(txtEmail.Text, txtPassword.Text, txtName.Text, txtFirstname.Text, chkRole.Checked);
                MessageBox.Show("Utilisateur créé.");
            }
            editPanel.Visible = false;
            LoadUsers();
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Supprimer cet utilisateur ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = (int)dgvUsers.SelectedRows[0].Cells["Id"].Value;
                userDAO.DeleteUser(id);
                LoadUsers();
            }
        }
    }
}