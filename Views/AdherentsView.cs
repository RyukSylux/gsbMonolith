using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    /// <summary>
    /// Vue pour la gestion des adhérents du Club de Tennis Lumière.
    /// </summary>
    public class AdherentsView : UserControl
    {
        private AdherentDAO adherentDAO = new AdherentDAO();
        private User currentUser;
        
        // UI Controls
        private DataGridView dgvAdherents;
        private Panel headerPanel;
        private Button btnAdd;
        private Label lblEmptyMessage;

        // Edit Panel
        private Panel editPanel;
        private TextBox txtNom, txtPrenom, txtEmail, txtTelephone;
        private DateTimePicker dtpAdhesion;
        private CheckBox chkCotisation;
        private Button btnSave, btnCancel;
        private int? editingId = null;

        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public AdherentsView(User user)
        {
            currentUser = user;
            SetupUI();
            LoadAdherents();
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
                Text = "Gestion des Adhérents", 
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var btnDelete = new Button
            {
                Text = "Supprimer",
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(130, 35),
                Cursor = Cursors.Hand,
                Location = new Point(headerPanel.Width - 510, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDelete.Click += BtnDelete_Click;

            var btnImport = new Button
            {
                Text = "Importer Excel",
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 35),
                Cursor = Cursors.Hand,
                Location = new Point(headerPanel.Width - 370, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnImport.Click += BtnImport_Click;

            btnAdd = new Button
            {
                Text = "Nouvel Adhérent",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(170, 35),
                Cursor = Cursors.Hand,
                Location = new Point(headerPanel.Width - 210, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAdd.Click += (s, e) => ShowEditPanel(null);

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnDelete);
            headerPanel.Controls.Add(btnImport);
            headerPanel.Controls.Add(btnAdd);

            // --- Grid ---
            dgvAdherents = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = false, // Permettre de cocher les cases
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            // Ajout de la colonne de sélection
            var checkColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Select",
                HeaderText = "Sél.",
                Width = 40,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = false
            };
            dgvAdherents.Columns.Add(checkColumn);
            
            dgvAdherents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvAdherents.ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray;
            dgvAdherents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvAdherents.ColumnHeadersHeight = 40;
            dgvAdherents.DefaultCellStyle.Padding = new Padding(10);
            dgvAdherents.RowTemplate.Height = 40;

            dgvAdherents.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0)
                    ShowEditPanel((int)dgvAdherents.Rows[e.RowIndex].Cells["Id"].Value);
            };

            // --- Empty Message Label ---
            lblEmptyMessage = new Label
            {
                Text = "Il n'y a pas d'adhérents",
                Font = new Font("Segoe UI", 14F, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Dock = DockStyle.Fill,
                Visible = false
            };
            dgvAdherents.Controls.Add(lblEmptyMessage);

            // --- Edit Panel ---
            editPanel = new Panel 
            { 
                Size = new Size(400, 550), 
                BackColor = Color.White, 
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel editHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(240, 240, 240) };
            editHeader.MouseDown += (s, e) => { isDragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = editPanel.Location; };
            editHeader.MouseMove += (s, e) => { if (isDragging) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); editPanel.Location = Point.Add(dragFormPoint, new Size(dif)); } };
            editHeader.MouseUp += (s, e) => { isDragging = false; };

            Label lblEditTitle = new Label { Text = "Fiche Adhérent", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(15, 12), AutoSize = true };
            editHeader.Controls.Add(lblEditTitle);

            Button btnCloseX = new Button 
            { 
                Text = "✕", 
                Dock = DockStyle.Right, 
                Width = 50, 
                FlatStyle = FlatStyle.Flat, 
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnCloseX.Click += (s, e) => editPanel.Visible = false;
            editHeader.Controls.Add(btnCloseX);

            editPanel.Controls.Add(editHeader);
            
            txtNom = CreateInput("Nom", 80);
            txtPrenom = CreateInput("Prénom", 140);
            txtEmail = CreateInput("Email", 200);
            txtTelephone = CreateInput("Téléphone", 260);

            Label lblDate = new Label { Text = "Date Adhésion", Location = new Point(20, 310), AutoSize = true, ForeColor = Color.Gray };
            dtpAdhesion = new DateTimePicker { Location = new Point(20, 330), Width = 340 };
            
            chkCotisation = new CheckBox { Text = "Cotisation réglée", Location = new Point(20, 370), AutoSize = true };

            btnSave = new Button 
            { 
                Text = "Enregistrer", 
                BackColor = Color.FromArgb(40, 167, 69), 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(20, 420), 
                Size = new Size(150, 35) 
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button 
            { 
                Text = "Annuler", 
                BackColor = Color.Gray, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(190, 420), 
                Size = new Size(150, 35) 
            };
            btnCancel.Click += (s, e) => editPanel.Visible = false;

            editPanel.Controls.AddRange(new Control[] { dtpAdhesion, lblDate, chkCotisation, btnSave, btnCancel });

            // Assembly
            this.Controls.Add(editPanel);
            this.Controls.Add(dgvAdherents);
            this.Controls.Add(headerPanel);

            this.SizeChanged += (s, e) => CenterEditPanel();
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

        private void LoadAdherents()
        {
            var list = adherentDAO.GetAllAdherents();
            dgvAdherents.DataSource = list;
            
            if(dgvAdherents.Columns["Id"] != null) dgvAdherents.Columns["Id"].Visible = false;
            
            // On s'assure que toutes les colonnes sauf 'Select' sont en lecture seule
            foreach (DataGridViewColumn col in dgvAdherents.Columns)
            {
                if (col.Name != "Select") col.ReadOnly = true;
            }

            if(dgvAdherents.Columns["DateAdhesion"] != null)
            {
                dgvAdherents.Columns["DateAdhesion"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvAdherents.Columns["DateAdhesion"].HeaderText = "Date d'adhésion";
            }
            
            // Gestion de l'affichage du message vide
            lblEmptyMessage.Visible = (list == null || list.Count == 0);
        }

        private void ShowEditPanel(int? id)
        {
            editingId = id;
            if (id.HasValue)
            {
                var adherents = (System.Collections.Generic.List<Adherent>)dgvAdherents.DataSource;
                var a = adherents.Find(x => x.Id == id.Value);
                if (a != null)
                {
                    txtNom.Text = a.Nom;
                    txtPrenom.Text = a.Prenom;
                    txtEmail.Text = a.Email;
                    txtTelephone.Text = a.Telephone;
                    dtpAdhesion.Value = a.DateAdhesion;
                    chkCotisation.Checked = a.CotisationReglee;
                }
            }
            else
            {
                txtNom.Clear(); txtPrenom.Clear(); txtEmail.Clear(); txtTelephone.Clear();
                dtpAdhesion.Value = DateTime.Now; chkCotisation.Checked = false;
            }
            editPanel.Visible = true;
            editPanel.BringToFront();
            CenterEditPanel();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var a = new Adherent(editingId ?? 0, txtNom.Text, txtPrenom.Text, txtEmail.Text, txtTelephone.Text, dtpAdhesion.Value, chkCotisation.Checked);

            if (editingId.HasValue) adherentDAO.UpdateAdherent(a);
            else adherentDAO.CreateAdherent(a);
            
            editPanel.Visible = false;
            LoadAdherents();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
             var idsToDelete = new System.Collections.Generic.List<int>();

             // Vérifier les cases cochées
             foreach (DataGridViewRow row in dgvAdherents.Rows)
             {
                 if (Convert.ToBoolean(row.Cells["Select"].Value))
                 {
                     idsToDelete.Add((int)row.Cells["Id"].Value);
                 }
             }

             // Si rien n'est coché, prendre la ligne sélectionnée
             if (idsToDelete.Count == 0 && dgvAdherents.SelectedRows.Count > 0)
             {
                 idsToDelete.Add((int)dgvAdherents.SelectedRows[0].Cells["Id"].Value);
             }

             if (idsToDelete.Count == 0)
             {
                 MessageBox.Show("Veuillez sélectionner au moins un adhérent à supprimer.");
                 return;
             }

             string msg = idsToDelete.Count > 1 ? $"Supprimer les {idsToDelete.Count} adhérents sélectionnés ?" : "Supprimer cet adhérent ?";
             if (MessageBox.Show(msg, "Confirmer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
             {
                 foreach (int id in idsToDelete)
                 {
                     adherentDAO.DeleteAdherent(id);
                 }
                 LoadAdherents();
             }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichiers Excel (*.xlsx)|*.xlsx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var importer = new gsbMonolith.Utils.ExcelImporter();
                        var newAdherents = importer.ImportAdherents(openFileDialog.FileName);
                        
                        int count = 0;
                        foreach (var a in newAdherents)
                        {
                            if (adherentDAO.CreateAdherent(a)) count++;
                        }
                        
                        MessageBox.Show($"{count} adhérents importés avec succès !");
                        LoadAdherents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'import : " + ex.Message);
                    }
                }
            }
        }
    }
}
