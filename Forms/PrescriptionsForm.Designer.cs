using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    partial class PrescriptionsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Title_label = new Label();
            dgvPrescriptions = new DataGridView();
            BtnRefresh = new Button();
            BtnAdd = new Button();
            BtnDelete = new Button();
            groupBoxAdd = new GroupBox();
            cmbPatients = new ComboBox();
            dtpValidity = new DateTimePicker();
            labelPatient = new Label();
            labelValidity = new Label();
            labelMedicines = new Label();
            BtnSave = new Button();
            dgvMedicines = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvPrescriptions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMedicines).BeginInit();
            groupBoxAdd.SuspendLayout();
            SuspendLayout();

            // Title
            Title_label.AutoSize = true;
            Title_label.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            Title_label.Location = new Point(20, 20);
            Title_label.Text = "Gestion des prescriptions";

            // dgvPrescriptions
            dgvPrescriptions.AllowUserToAddRows = false;
            dgvPrescriptions.AllowUserToDeleteRows = false;
            dgvPrescriptions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrescriptions.Location = new Point(20, 70);
            dgvPrescriptions.Size = new Size(760, 250);
            dgvPrescriptions.ReadOnly = true;
            dgvPrescriptions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPrescriptions.MultiSelect = false;
            dgvPrescriptions.CellClick += DgvPrescriptions_CellClick;

            // BtnRefresh
            BtnRefresh.Font = new Font("Segoe UI", 10F);
            BtnRefresh.Location = new Point(670, 25);
            BtnRefresh.Size = new Size(110, 30);
            BtnRefresh.Text = "🔄 Rafraîchir";
            BtnRefresh.Click += BtnRefresh_Click;

            // BtnAdd
            BtnAdd.Font = new Font("Segoe UI", 10F);
            BtnAdd.Location = new Point(540, 25);
            BtnAdd.Size = new Size(110, 30);
            BtnAdd.Text = "➕ Ajouter";
            BtnAdd.Click += BtnAdd_Click;

            // BtnDelete
            BtnDelete.Font = new Font("Segoe UI", 10F);
            BtnDelete.Location = new Point(410, 25);
            BtnDelete.Size = new Size(110, 30);
            BtnDelete.Text = "❌ Supprimer";
            BtnDelete.Click += BtnDelete_Click;

            // BtnExportPdf
            BtnExportPdf = new Button();
            BtnExportPdf.Font = new Font("Segoe UI", 10F);
            BtnExportPdf.Location = new Point(280, 25);
            BtnExportPdf.Size = new Size(110, 30);
            BtnExportPdf.Text = "📄 Export PDF";
            BtnExportPdf.Click += BtnExportPdf_Click;
            Controls.Add(BtnExportPdf);

            // groupBoxAdd
            groupBoxAdd.Controls.Add(cmbPatients);
            groupBoxAdd.Controls.Add(labelPatient);
            groupBoxAdd.Controls.Add(dtpValidity);
            groupBoxAdd.Controls.Add(labelValidity);
            groupBoxAdd.Controls.Add(dgvMedicines);
            groupBoxAdd.Controls.Add(labelMedicines);
            groupBoxAdd.Controls.Add(BtnSave);
            groupBoxAdd.Location = new Point(20, 340);
            groupBoxAdd.Size = new Size(760, 250);
            groupBoxAdd.Text = "Ajouter / Éditer une prescription";
            groupBoxAdd.Visible = false;

            // cmbPatients
            cmbPatients.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPatients.Location = new Point(130, 30);
            cmbPatients.Size = new Size(200, 23);

            // labelPatient
            labelPatient.AutoSize = true;
            labelPatient.Location = new Point(20, 33);
            labelPatient.Text = "Patient :";

            // dtpValidity
            dtpValidity.Location = new Point(130, 70);
            dtpValidity.Size = new Size(200, 23);

            // labelValidity
            labelValidity.AutoSize = true;
            labelValidity.Location = new Point(20, 73);
            labelValidity.Text = "Validité :";

            // dgvMedicines
            dgvMedicines.Location = new Point(380, 30);
            dgvMedicines.Size = new Size(360, 170);
            dgvMedicines.AllowUserToAddRows = false;
            dgvMedicines.RowHeadersVisible = false;
            dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMedicines.BackgroundColor = Color.White;
            dgvMedicines.BorderStyle = BorderStyle.FixedSingle;

            // Colonnes
            dgvMedicines.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                Name = "colSelect",
                HeaderText = "✔",
                FillWeight = 15,
            });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colId",
                Visible = false
            });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colName",
                HeaderText = "Médicament",
                ReadOnly = true,
                FillWeight = 150
            });
            dgvMedicines.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colQuantity",
                HeaderText = "Quantité",
                FillWeight = 60
            });

            // labelMedicines
            labelMedicines.AutoSize = true;
            labelMedicines.Location = new Point(380, 10);
            labelMedicines.Text = "Médicaments :";

            // BtnSave
            BtnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            BtnSave.Location = new Point(630, 210);
            BtnSave.Size = new Size(110, 30);
            BtnSave.Text = "💾 Enregistrer";
            BtnSave.Click += BtnSave_Click;

            // Form
            ClientSize = new Size(800, 600);
            Controls.Add(groupBoxAdd);
            Controls.Add(BtnAdd);
            Controls.Add(BtnDelete);
            Controls.Add(BtnRefresh);
            Controls.Add(dgvPrescriptions);
            Controls.Add(Title_label);
            Text = "Gestion des prescriptions";

            ((System.ComponentModel.ISupportInitialize)dgvPrescriptions).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMedicines).EndInit();
            groupBoxAdd.ResumeLayout(false);
            groupBoxAdd.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Title_label;
        private DataGridView dgvPrescriptions;
        private Button BtnRefresh;
        private Button BtnAdd;
        private Button BtnDelete;
        private Button BtnExportPdf;
        private GroupBox groupBoxAdd;
        private ComboBox cmbPatients;
        private DateTimePicker dtpValidity;
        private Label labelPatient;
        private Label labelValidity;
        private Label labelMedicines;
        private Button BtnSave;
        private DataGridView dgvMedicines;
    }
}
