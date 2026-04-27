using System;
using System.Drawing;
using System.Windows.Forms;
using gsbMonolith.Models;
using gsbMonolith.DAO;
using gsbMonolith.Views.Modals;

namespace gsbMonolith.Views
{
    /// <summary>
    /// Represents the dashboard view, displaying user-specific content and administrative tools if the user has the 'Admin' role.
    /// </summary>
    public class DashboardView : UserControl
    {
        private User currentUser;
        private UserDAO userDAO = new UserDAO();
        
        // Admin UI
        private DataGridView dgvUsers;
        private Panel adminPanel;

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
                Location = new Point(0, 15)
            };

            Panel btnContainer = new Panel { Dock = DockStyle.Right, Width = 400 };
            
            Button btnDeleteUser = new Button
            {
                Text = "Supprimer",
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 35),
                Cursor = Cursors.Hand,
                Location = new Point(20, 15)
            };
            btnDeleteUser.Click += BtnDeleteUser_Click;

            Button btnAddUser = new Button
            {
                Text = "+ Nouvel Utilisateur",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(180, 35),
                Cursor = Cursors.Hand,
                Location = new Point(200, 15)
            };
            btnAddUser.Click += (s, e) => OpenUserModal(null);

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
            dgvUsers.DataBindingComplete += (s, e) => dgvUsers.ClearSelection();
            
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvUsers.ColumnHeadersHeight = 40;
            dgvUsers.DefaultCellStyle.Padding = new Padding(10);
            dgvUsers.RowTemplate.Height = 40;

            dgvUsers.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                {
                    int id = (int)dgvUsers.Rows[e.RowIndex].Cells["Id"].Value;
                    OpenUserModal(userDAO.GetUserById(id));
                }
            };

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Modifier", null, (s, e) => {
                 if(dgvUsers.SelectedRows.Count > 0) 
                 {
                    int id = (int)dgvUsers.SelectedRows[0].Cells["Id"].Value;
                    OpenUserModal(userDAO.GetUserById(id));
                 }
            });
            ctx.Items.Add("Supprimer", null, BtnDeleteUser_Click);
            dgvUsers.ContextMenuStrip = ctx;

            adminPanel.Controls.Add(dgvUsers);
            adminPanel.Controls.Add(topBar);
            
            this.Controls.Add(adminPanel);
            adminPanel.BringToFront();
        }

        private void LoadUsers()
        {
            try
            {
                dgvUsers.DataSource = userDAO.GetAllUsers();
                if (dgvUsers.Columns["Id"] != null) dgvUsers.Columns["Id"].Visible = false;
                if (dgvUsers.Columns["Password"] != null) dgvUsers.Columns["Password"].Visible = false;
                
                if (dgvUsers.Columns["Name"] != null) dgvUsers.Columns["Name"].HeaderText = "Nom";
                if (dgvUsers.Columns["FirstName"] != null) dgvUsers.Columns["FirstName"].HeaderText = "Prénom";
                if (dgvUsers.Columns["Role"] != null) dgvUsers.Columns["Role"].HeaderText = "Admin";
                if (dgvUsers.Columns["Email"] != null) dgvUsers.Columns["Email"].HeaderText = "Email";
            }
            catch(Exception ex) { MessageBox.Show("Erreur chargement utilisateurs: " + ex.Message); }
        }

        private void OpenUserModal(User user)
        {
            using (var modal = new UserEditForm(user))
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (user != null)
                        {
                            var u = new User(user.Id, modal.UserName, modal.UserFirstName, modal.UserRole, modal.UserEmail);
                            userDAO.UpdateUser(u);
                            MessageBox.Show("Utilisateur mis à jour.");
                        }
                        else
                        {
                            userDAO.CreateUser(modal.UserEmail, modal.UserPassword, modal.UserName, modal.UserFirstName, modal.UserRole);
                            MessageBox.Show("Utilisateur créé.");
                        }
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement: " + ex.Message);
                    }
                }
            }
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