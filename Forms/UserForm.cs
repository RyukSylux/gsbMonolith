using gsbMonolith.Models;
using gsbMonolith.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    public partial class UserForm : Form
    {
        private User currentUser;
        
        // Layout Controls
        private Panel navbarPanel;
        private Panel contentPanel;
        private Button activeNavButton;

        public UserForm(User user)
        {
            InitializeComponent(); // Vide maintenant
            currentUser = user;
            SetupLayout();
            
            // Charge la vue par défaut
            LoadView(new DashboardView(currentUser));
        }

        private void SetupLayout()
        {
            // --- Window Settings ---
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1024, 768);
            this.Text = "Tennis Club Lumière - Gestion";
            
            // --- Navbar (Left) ---
            navbarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(33, 43, 54) // Dark Sidebar
            };

            // Logo Area
            var logoPanel = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(28, 37, 46) };
            var lblLogo = new Label 
            { 
                Text = "TCL Gestion", 
                ForeColor = Color.White, 
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            logoPanel.Controls.Add(lblLogo);
            navbarPanel.Controls.Add(logoPanel);

            // Nav Buttons
            AddNavButton("🏠 Accueil", 100, () => new DashboardView(currentUser));
            AddNavButton("🎾 Adhérents", 160, () => new AdherentsView(currentUser));

            // Logout Button (Bottom)
            var btnLogout = new Button
            {
                Text = "Déconnexion",
                Dock = DockStyle.Bottom,
                Height = 60,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(198, 40, 40),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F)
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => {
                this.Hide();
                new MainForm().Show(); // Retour au login
            };
            navbarPanel.Controls.Add(btnLogout);

            // --- Content Panel (Right/Fill) ---
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            // Add to Form
            this.Controls.Add(contentPanel);
            this.Controls.Add(navbarPanel);
        }

        private void AddNavButton(string text, int top, Func<UserControl> viewFactory)
        {
            var btn = new Button
            {
                Text = "  " + text, // Petit espace
                Top = top,
                Left = 0,
                Width = 260,
                Height = 60,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btn.Click += (s, e) => {
                SetActiveButton(btn);
                LoadView(viewFactory());
            };

            navbarPanel.Controls.Add(btn);
        }

        private void SetActiveButton(Button btn)
        {
            if (activeNavButton != null)
            {
                activeNavButton.BackColor = Color.Transparent;
                activeNavButton.ForeColor = Color.FromArgb(180, 180, 180);
            }
            activeNavButton = btn;
            activeNavButton.BackColor = Color.FromArgb(45, 55, 72); // Active state color
            activeNavButton.ForeColor = Color.White;
            
            // Petite barre bleue à gauche pour indiquer l'actif (Optionnel, simulé par border ou panel)
        }

        private void LoadView(UserControl view)
        {
            contentPanel.Controls.Clear();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }
    }
}
