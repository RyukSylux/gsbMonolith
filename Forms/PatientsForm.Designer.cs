using System;
using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    partial class PatientsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label Title_label;
        private DataGridView dgvPatients;
        private Button BtnRefresh;
        private Button BtnAdd;
        private GroupBox groupBoxAdd;
        private TextBox txtFirstname;
        private TextBox txtName;
        private TextBox txtAge;
        private ComboBox cmbGender;
        private Label labelFirstname;
        private Label labelName;
        private Label labelAge;
        private Label labelGender;
        private Button BtnSave;
        private Button BtnDelete;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Title_label = new Label();
            dgvPatients = new DataGridView();
            BtnRefresh = new Button();
            BtnAdd = new Button();
            BtnDelete = new Button();
            groupBoxAdd = new GroupBox();
            txtFirstname = new TextBox();
            txtName = new TextBox();
            txtAge = new TextBox();
            cmbGender = new ComboBox();
            labelFirstname = new Label();
            labelName = new Label();
            labelAge = new Label();
            labelGender = new Label();
            BtnSave = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvPatients).BeginInit();
            groupBoxAdd.SuspendLayout();
            SuspendLayout();

            // Title
            Title_label.AutoSize = true;
            Title_label.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            Title_label.Location = new Point(20, 20);
            Title_label.Text = "Gestion des patients";

            // DataGridView
            dgvPatients.Location = new Point(20, 70);
            dgvPatients.Size = new Size(740, 250);
            dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatients.ReadOnly = true;

            // Buttons
            BtnRefresh.Location = new Point(650, 25);
            BtnRefresh.Size = new Size(110, 30);
            BtnRefresh.Text = "🔄 Rafraîchir";
            BtnRefresh.Click += BtnRefresh_Click;

            BtnAdd.Location = new Point(520, 25);
            BtnAdd.Size = new Size(110, 30);
            BtnAdd.Text = "➕ Ajouter";
            BtnAdd.Click += BtnAdd_Click;

            BtnDelete.Location = new Point(380, 25);
            BtnDelete.Size = new Size(110, 30);
            BtnDelete.Text = "🗑️ Supprimer";
            BtnDelete.Click += BtnDelete_Click;

            // GroupBox Add
            groupBoxAdd.Location = new Point(20, 340);
            groupBoxAdd.Size = new Size(740, 150);
            groupBoxAdd.Text = "Ajouter un patient";
            groupBoxAdd.Visible = false;

            // Labels and Inputs
            labelFirstname.Location = new Point(20, 28);
            labelFirstname.Text = "Prénom :";
            txtFirstname.Location = new Point(120, 25);
            txtFirstname.Size = new Size(120, 23);

            labelName.Location = new Point(20, 63);
            labelName.Text = "Nom :";
            txtName.Location = new Point(120, 60);
            txtName.Size = new Size(120, 23);

            labelAge.Location = new Point(260, 28);
            labelAge.Text = "Âge :";
            txtAge.Location = new Point(320, 25);
            txtAge.Size = new Size(60, 23);

            labelGender.Location = new Point(260, 63);
            labelGender.Text = "Genre :";
            cmbGender.Location = new Point(320, 60);
            cmbGender.Size = new Size(120, 23);
            cmbGender.Items.AddRange(new string[] { "H", "F"});

            // Save button
            BtnSave.Location = new Point(600, 100);
            BtnSave.Size = new Size(110, 30);
            BtnSave.Text = "💾 Enregistrer";
            BtnSave.Click += BtnSave_Click;

            groupBoxAdd.Controls.Add(txtFirstname);
            groupBoxAdd.Controls.Add(txtName);
            groupBoxAdd.Controls.Add(txtAge);
            groupBoxAdd.Controls.Add(cmbGender);
            groupBoxAdd.Controls.Add(labelFirstname);
            groupBoxAdd.Controls.Add(labelName);
            groupBoxAdd.Controls.Add(labelAge);
            groupBoxAdd.Controls.Add(labelGender);
            groupBoxAdd.Controls.Add(BtnSave);

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 520);
            Controls.Add(groupBoxAdd);
            Controls.Add(BtnAdd);
            Controls.Add(BtnRefresh);
            Controls.Add(BtnDelete);
            Controls.Add(dgvPatients);
            Controls.Add(Title_label);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion des patients";

            ((System.ComponentModel.ISupportInitialize)dgvPatients).EndInit();
            groupBoxAdd.ResumeLayout(false);
            groupBoxAdd.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
