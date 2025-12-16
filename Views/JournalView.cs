using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    public class JournalView : UserControl
    {
        private JournalDAO journalDAO = new JournalDAO();
        private DataGridView dgvJournal;
        private Panel headerPanel;
        private FlowLayoutPanel filterPanel;

        // Filters
        private ComboBox cmbUser;
        private ComboBox cmbType; // Object type (Patient, Medoc...)
        private ComboBox cmbEvent; // Action name
        private TextBox txtSearch;
        private Button btnReset;

        // Data Cache
        private List<dynamic> allLogs = new List<dynamic>();

        public JournalView(User user)
        {
            if (!user.Role)
            {
                MessageBox.Show("Accès refusé.");
                return;
            }
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10F);

            // --- Header ---
            headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(20) };
            var titleLabel = new Label
            {
                Text = "Journal des Événements",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            
            Button btnRefresh = new Button
            {
                Text = "Actualiser",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(120, 35),
                Location = new Point(headerPanel.Width - 140, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += (s, e) => LoadData();

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnRefresh);

            // --- Filter Panel ---
            filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.WhiteSmoke,
                AutoSize = false
            };

            // Helpers for filters
            cmbUser = CreateFilterCombo("Utilisateur");
            cmbType = CreateFilterCombo("Objet");
            cmbEvent = CreateFilterCombo("Action");
            
            txtSearch = new TextBox 
            { 
                Width = 200, 
                Font = new Font("Segoe UI", 10F), 
                ForeColor = Color.Gray,
                Text = "Rechercher..."
            };
            txtSearch.GotFocus += (s, e) => { if(txtSearch.Text == "Rechercher...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; } };
            txtSearch.LostFocus += (s, e) => { if(string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Rechercher..."; txtSearch.ForeColor = Color.Gray; } };
            txtSearch.TextChanged += (s, e) => ApplyFilters();

            btnReset = new Button
            {
                Text = "X",
                Width = 40,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGray
            };
            btnReset.Click += (s, e) => ResetFilters();

            filterPanel.Controls.Add(new Label { Text = "Filtres :", AutoSize = true, Margin = new Padding(0, 8, 10, 0), Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
            filterPanel.Controls.Add(cmbUser);
            filterPanel.Controls.Add(cmbType);
            filterPanel.Controls.Add(cmbEvent);
            filterPanel.Controls.Add(txtSearch);
            filterPanel.Controls.Add(btnReset);

            // --- Grid ---
            dgvJournal = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvJournal.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvJournal.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvJournal.ColumnHeadersHeight = 40;
            dgvJournal.DefaultCellStyle.Padding = new Padding(5);
            dgvJournal.RowTemplate.Height = 35;

            // Assembly
            this.Controls.Add(dgvJournal);
            this.Controls.Add(filterPanel);
            this.Controls.Add(headerPanel);
        }

        private ComboBox CreateFilterCombo(string placeholder)
        {
            var cmb = new ComboBox
            {
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmb.Items.Add($"-- {placeholder} --");
            cmb.SelectedIndex = 0;
            cmb.SelectedIndexChanged += (s, e) => ApplyFilters();
            return cmb;
        }

        private void ResetFilters()
        {
            cmbUser.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            cmbEvent.SelectedIndex = 0;
            txtSearch.Text = "Rechercher...";
            txtSearch.ForeColor = Color.Gray;
            ApplyFilters();
        }

        private void LoadData()
        {
            try
            {
                allLogs = journalDAO.GetAll(); // Store in cache
                PopulateFilterOptions(); // Fill combos
                ApplyFilters(); // Show data
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur chargement journal: " + ex.Message);
            }
        }

        private void PopulateFilterOptions()
        {
            // Helper to populate distinct sorted values
            void Fill(ComboBox cmb, Func<dynamic, string> selector)
            {
                string currentSelection = cmb.SelectedIndex > 0 ? cmb.SelectedItem.ToString() : null;
                
                cmb.Items.Clear();
                cmb.Items.Add(cmb == cmbUser ? "-- Utilisateur --" : (cmb == cmbType ? "-- Objet --" : "-- Action --"));
                
                var items = allLogs.Select(selector).Distinct().OrderBy(s => s).ToList();
                foreach (var item in items)
                {
                    if(!string.IsNullOrEmpty(item))
                        cmb.Items.Add(item);
                }

                if (currentSelection != null && cmb.Items.Contains(currentSelection))
                    cmb.SelectedItem = currentSelection;
                else
                    cmb.SelectedIndex = 0;
            }

            Fill(cmbUser, x => x.User);
            Fill(cmbType, x => x.Type); // "Type" column in DB (Patient, Medicine...)
            Fill(cmbEvent, x => x.Event); // "EventName" column in DB
        }

        private void ApplyFilters()
        {
            if (allLogs == null) return;

            var query = allLogs.AsEnumerable();

            // 1. Filter by User
            if (cmbUser.SelectedIndex > 0)
            {
                string selected = cmbUser.SelectedItem.ToString();
                query = query.Where(x => (string)x.User == selected);
            }

            // 2. Filter by Object Type
            if (cmbType.SelectedIndex > 0)
            {
                string selected = cmbType.SelectedItem.ToString();
                query = query.Where(x => (string)x.Type == selected);
            }

            // 3. Filter by Action Name
            if (cmbEvent.SelectedIndex > 0)
            {
                string selected = cmbEvent.SelectedItem.ToString();
                query = query.Where(x => (string)x.Event == selected);
            }

            // 4. Search Bar
            string search = txtSearch.Text;
            if (!string.IsNullOrWhiteSpace(search) && search != "Rechercher...")
            {
                search = search.ToLower();
                query = query.Where(x => 
                    ((string)x.Description ?? "").ToLower().Contains(search) ||
                    ((string)x.Object ?? "").ToLower().Contains(search) ||
                    ((string)x.User ?? "").ToLower().Contains(search) ||
                    ((string)x.Event ?? "").ToLower().Contains(search)
                );
            }

            var filteredList = query.ToList();

            // Safe Binding
            dgvJournal.DataSource = null; // Reset to force refresh
            if (filteredList.Count > 0)
            {
                dgvJournal.DataSource = filteredList;
                SafeStyleGrid();
            }
        }

        private void SafeStyleGrid()
        {
            try
            {
                // Hiding ID column safely
                if (dgvJournal.Columns.Contains("Id")) 
                    dgvJournal.Columns["Id"].Visible = false;
                
                //// Optional: Widths
                //if (dgvJournal.Columns.Contains("Date")) dgvJournal.Columns["Date"].Width = 140;
                //if (dgvJournal.Columns.Contains("User")) dgvJournal.Columns["User"].Width = 140;
                //if (dgvJournal.Columns.Contains("Type")) dgvJournal.Columns["Type"].Width = 100;
            }
            catch { /* Ignore styling errors to prevent crashes */ }
        }
    }
}