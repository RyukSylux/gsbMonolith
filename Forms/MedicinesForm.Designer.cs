using System.Drawing;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    partial class MedicinesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Title_label = new Label();
            dgvMedicines = new DataGridView();
            BtnRefresh = new Button();
            BtnAdd = new Button();
            BtnDelete = new Button();
            groupBoxAdd = new GroupBox();
            txtDosage = new TextBox();
            txtName = new TextBox();
            txtDescription = new TextBox();
            txtMolecule = new TextBox();
            labelDosage = new Label();
            labelName = new Label();
            labelDescription = new Label();
            labelMolecule = new Label();
            BtnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMedicines).BeginInit();
            groupBoxAdd.SuspendLayout();
            SuspendLayout();
            // 
            // Title_label
            // 
            Title_label.AutoSize = true;
            Title_label.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            Title_label.Location = new Point(20, 20);
            Title_label.Name = "Title_label";
            Title_label.Size = new Size(241, 25);
            Title_label.TabIndex = 0;
            Title_label.Text = "Gestion des médicaments";
            // 
            // dgvMedicines
            // 
            dgvMedicines.AllowUserToAddRows = false;
            dgvMedicines.AllowUserToDeleteRows = false;
            dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMedicines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMedicines.Location = new Point(20, 70);
            dgvMedicines.Name = "dgvMedicines";
            dgvMedicines.ReadOnly = true;
            dgvMedicines.RowTemplate.Height = 25;
            dgvMedicines.Size = new Size(760, 250);
            dgvMedicines.TabIndex = 1;
            // 
            // BtnRefresh
            // 
            BtnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            BtnRefresh.Location = new Point(670, 25);
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new Size(110, 30);
            BtnRefresh.TabIndex = 2;
            BtnRefresh.Text = "🔄 Rafraîchir";
            BtnRefresh.UseVisualStyleBackColor = true;
            BtnRefresh.Click += BtnRefresh_Click;
            // 
            // BtnAdd
            // 
            BtnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            BtnAdd.Location = new Point(540, 25);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(110, 30);
            BtnAdd.TabIndex = 3;
            BtnAdd.Text = "➕ Ajouter";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // BtnDelete
            // 
            BtnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            BtnDelete.Location = new Point(410, 25);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(110, 30);
            BtnDelete.TabIndex = 4;
            BtnDelete.Text = "🗑️ Supprimer";
            BtnDelete.UseVisualStyleBackColor = true;
            BtnDelete.Click += BtnDelete_Click;
            // 
            // groupBoxAdd
            // 
            groupBoxAdd.Controls.Add(txtDosage);
            groupBoxAdd.Controls.Add(txtName);
            groupBoxAdd.Controls.Add(txtDescription);
            groupBoxAdd.Controls.Add(txtMolecule);
            groupBoxAdd.Controls.Add(labelDosage);
            groupBoxAdd.Controls.Add(labelName);
            groupBoxAdd.Controls.Add(labelDescription);
            groupBoxAdd.Controls.Add(labelMolecule);
            groupBoxAdd.Controls.Add(BtnSave);
            groupBoxAdd.Location = new Point(20, 340);
            groupBoxAdd.Name = "groupBoxAdd";
            groupBoxAdd.Size = new Size(760, 180);
            groupBoxAdd.TabIndex = 5;
            groupBoxAdd.TabStop = false;
            groupBoxAdd.Text = "Ajouter un médicament";
            groupBoxAdd.Visible = false;
            // 
            // txtDosage
            // 
            txtDosage.Location = new Point(120, 30);
            txtDosage.Name = "txtDosage";
            txtDosage.Size = new Size(150, 23);
            txtDosage.TabIndex = 0;
            // 
            // txtName
            // 
            txtName.Location = new Point(400, 30);
            txtName.Name = "txtName";
            txtName.Size = new Size(150, 23);
            txtName.TabIndex = 1;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(120, 70);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(430, 23);
            txtDescription.TabIndex = 2;
            // 
            // txtMolecule
            // 
            txtMolecule.Location = new Point(120, 110);
            txtMolecule.Name = "txtMolecule";
            txtMolecule.Size = new Size(150, 23);
            txtMolecule.TabIndex = 3;
            // 
            // labelDosage
            // 
            labelDosage.AutoSize = true;
            labelDosage.Location = new Point(20, 33);
            labelDosage.Name = "labelDosage";
            labelDosage.Size = new Size(56, 15);
            labelDosage.TabIndex = 4;
            labelDosage.Text = "Dosage :";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(320, 33);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 5;
            labelName.Text = "Nom :";
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(20, 73);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(72, 15);
            labelDescription.TabIndex = 6;
            labelDescription.Text = "Description :";
            // 
            // labelMolecule
            // 
            labelMolecule.AutoSize = true;
            labelMolecule.Location = new Point(20, 113);
            labelMolecule.Name = "labelMolecule";
            labelMolecule.Size = new Size(64, 15);
            labelMolecule.TabIndex = 7;
            labelMolecule.Text = "Molécule :";
            // 
            // BtnSave
            // 
            BtnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            BtnSave.Location = new Point(630, 130);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(110, 30);
            BtnSave.TabIndex = 8;
            BtnSave.Text = "💾 Enregistrer";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // MedicinesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 550);
            Controls.Add(groupBoxAdd);
            Controls.Add(BtnDelete);
            Controls.Add(BtnAdd);
            Controls.Add(BtnRefresh);
            Controls.Add(dgvMedicines);
            Controls.Add(Title_label);
            Name = "MedicinesForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion des médicaments";
            ((System.ComponentModel.ISupportInitialize)dgvMedicines).EndInit();
            groupBoxAdd.ResumeLayout(false);
            groupBoxAdd.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Title_label;
        private DataGridView dgvMedicines;
        private Button BtnRefresh;
        private Button BtnAdd;
        private Button BtnDelete;
        private GroupBox groupBoxAdd;
        private TextBox txtDosage;
        private TextBox txtName;
        private TextBox txtDescription;
        private TextBox txtMolecule;
        private Label labelDosage;
        private Label labelName;
        private Label labelDescription;
        private Label labelMolecule;
        private Button BtnSave;
    }
}
