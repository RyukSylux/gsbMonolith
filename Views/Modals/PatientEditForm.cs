using System;
using System.Drawing;
using System.Windows.Forms;
using gsbMonolith.Models;

namespace gsbMonolith.Views.Modals
{
    /// <summary>
    /// Form allowing users to create or edit patient records.
    /// Provides input validation for age and required fields.
    /// </summary>
    public class PatientEditForm : Form
    {
        /// <summary>
        /// Gets the entered name of the patient.
        /// </summary>
        public string PatientName => txtName.Text;

        /// <summary>
        /// Gets the entered first name of the patient.
        /// </summary>
        public string PatientFirstName => txtFirstname.Text;

        /// <summary>
        /// Gets the entered age of the patient. Returns 0 if invalid.
        /// </summary>
        public int PatientAge => int.TryParse(txtAge.Text, out int age) ? age : 0;

        /// <summary>
        /// Gets the entered gender of the patient.
        /// </summary>
        public string PatientGender => txtGender.Text;

        /// <summary>
        /// Gets the selected category ID, or null if "Aucune" is selected.
        /// </summary>
        public int? SelectedCategoryId => (cmbCategory.SelectedValue as int?) == 0 ? null : (cmbCategory.SelectedValue as int?);

        private TextBox txtName, txtFirstname, txtAge, txtGender;
        private ComboBox cmbCategory;
        private Button btnSave, btnCancel;
        private Patient _patient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientEditForm"/> class.
        /// </summary>
        /// <param name="patient">The patient object to edit, or null to create a new one.</param>
        public PatientEditForm(Patient patient = null)
        {
            _patient = patient;
            SetupUI();
            LoadCategories();
            if (_patient != null)
            {
                this.Text = "Modifier Patient";
                txtName.Text = _patient.Name;
                txtFirstname.Text = _patient.Firstname;
                txtAge.Text = _patient.Age.ToString();
                txtGender.Text = _patient.Gender;
                if (_patient.Id_category.HasValue)
                    cmbCategory.SelectedValue = _patient.Id_category.Value;
                else
                    cmbCategory.SelectedValue = 0;
                btnSave.Text = "Enregistrer";
            }
            else
            {
                this.Text = "Nouveau Patient";
                cmbCategory.SelectedValue = 0;
                btnSave.Text = "Créer";
            }
        }

        private void LoadCategories()
        {
            try
            {
                var catDAO = new gsbMonolith.DAO.CategoryDAO();
                var categories = catDAO.GetAllCategories();
                var list = new System.Collections.Generic.List<Category>();
                list.Add(new Category(0, "Aucune"));
                list.AddRange(categories);

                cmbCategory.DataSource = list;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "Id_category";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des catégories: " + ex.Message);
            }
        }

        private void SetupUI()
        {
            this.Size = new Size(400, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            Label lblHeader = new Label
            {
                Text = _patient == null ? "Nouveau Patient" : "Fiche Patient",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblHeader);

            txtName = CreateInput("Nom", 80);
            txtFirstname = CreateInput("Prénom", 140);
            txtAge = CreateInput("Âge", 200);
            txtGender = CreateInput("Genre (M/F)", 260);

            Label lblCategory = new Label { Text = "Catégorie de patient", Location = new Point(20, 300), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9F) };
            cmbCategory = new ComboBox { Location = new Point(20, 320), Width = 340, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.AddRange(new Control[] { lblCategory, cmbCategory });

            btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 390), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(200, 390), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { btnSave, btnCancel });
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private TextBox CreateInput(string placeholder, int y)
        {
            Label lbl = new Label { Text = placeholder, Location = new Point(20, y - 20), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9F) };
            TextBox txt = new TextBox { Location = new Point(20, y), Width = 340, Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            return txt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.Show("Veuillez remplir le nom et le prénom.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out _))
            {
                MessageBox.Show("L'âge doit être un nombre valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
