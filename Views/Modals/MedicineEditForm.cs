using System;
using System.Drawing;
using System.Windows.Forms;
using gsbMonolith.Models;

namespace gsbMonolith.Views.Modals
{
    /// <summary>
    /// Form allowing users to create or edit medicine records.
    /// Provides input validation and returns the entered data via properties.
    /// </summary>
    public class MedicineEditForm : Form
    {
        /// <summary>
        /// Gets the entered name of the medicine.
        /// </summary>
        public string MedName => txtName.Text;

        /// <summary>
        /// Gets the entered active molecule of the medicine.
        /// </summary>
        public string MedMolecule => txtMolecule.Text;

        /// <summary>
        /// Gets the entered dosage of the medicine.
        /// </summary>
        public string MedDosage => txtDosage.Text;

        /// <summary>
        /// Gets the entered description of the medicine.
        /// </summary>
        public string MedDescription => txtDescription.Text;

        private TextBox txtName, txtMolecule, txtDosage, txtDescription;
        private Button btnSave, btnCancel;
        private Medicine _medicine;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicineEditForm"/> class.
        /// </summary>
        /// <param name="medicine">The medicine object to edit, or null to create a new one.</param>
        public MedicineEditForm(Medicine medicine = null)
        {
            _medicine = medicine;
            SetupUI();
            if (_medicine != null)
            {
                this.Text = "Modifier Médicament";
                txtName.Text = _medicine.Name;
                txtMolecule.Text = _medicine.Molecule;
                txtDosage.Text = _medicine.Dosage;
                txtDescription.Text = _medicine.Description;
                btnSave.Text = "Enregistrer";
            }
            else
            {
                this.Text = "Nouveau Médicament";
                btnSave.Text = "Créer";
            }
        }

        private void SetupUI()
        {
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            Label lblHeader = new Label
            {
                Text = _medicine == null ? "Nouveau Médicament" : "Détails Médicament",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblHeader);

            txtName = CreateInput("Nom du médicament", 80);
            txtMolecule = CreateInput("Molécule", 140);
            txtDosage = CreateInput("Dosage", 200);
            txtDescription = CreateInput("Description", 260);

            btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 360), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(200, 360), Size = new Size(160, 40), Cursor = Cursors.Hand };
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
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom du médicament est obligatoire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
