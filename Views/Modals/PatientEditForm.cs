using System;
using System.Drawing;
using System.Windows.Forms;
using gsbMonolith.Models;

namespace gsbMonolith.Views.Modals
{
    public class PatientEditForm : Form
    {
        public string PatientName => txtName.Text;
        public string PatientFirstName => txtFirstname.Text;
        public int PatientAge => int.TryParse(txtAge.Text, out int age) ? age : 0;
        public string PatientGender => txtGender.Text;

        private TextBox txtName, txtFirstname, txtAge, txtGender;
        private Button btnSave, btnCancel;
        private Patient _patient;

        public PatientEditForm(Patient patient = null)
        {
            _patient = patient;
            SetupUI();
            if (_patient != null)
            {
                this.Text = "Modifier Patient";
                txtName.Text = _patient.Name;
                txtFirstname.Text = _patient.Firstname;
                txtAge.Text = _patient.Age.ToString();
                txtGender.Text = _patient.Gender;
                btnSave.Text = "Enregistrer";
            }
            else
            {
                this.Text = "Nouveau Patient";
                btnSave.Text = "Créer";
            }
        }

        private void SetupUI()
        {
            this.Size = new Size(400, 450);
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

            btnSave = new Button { Text = "Enregistrer", BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(20, 330), Size = new Size(160, 40), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Annuler", BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(200, 330), Size = new Size(160, 40), Cursor = Cursors.Hand };
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
