using gsbMonolith.DAO;
using gsbMonolith.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace gsbMonolith.Forms
{
    public partial class MedicinesForm : Form
    {
        private readonly MedicineDAO medicineDAO = new MedicineDAO();
        private readonly User currentUser;
        private bool isEditMode = false;
        private int editingMedicineId = -1;
        public MedicinesForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadMedicines();

            dgvMedicines.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }
        private void LoadMedicines()
        {
            try
            {
                var medicines = medicineDAO.GetAll();
                dgvMedicines.DataSource = medicines;

                // More readable column names
                dgvMedicines.Columns["Id_medicine"].HeaderText = "ID";
                dgvMedicines.Columns["User"].HeaderText = "Ajouté par";
                dgvMedicines.Columns["Dosage"].HeaderText = "Dosage";
                dgvMedicines.Columns["Name"].HeaderText = "Nom";
                dgvMedicines.Columns["Description"].HeaderText = "Description";
                dgvMedicines.Columns["Molecule"].HeaderText = "Molécule";

                dgvMedicines.Columns["User"].DisplayIndex = 1;

                if (dgvMedicines.Columns.Contains("Id_user"))
                    dgvMedicines.Columns["Id_user"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des médicaments : {ex.Message}");
            }
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadMedicines();
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            isEditMode = false;
            groupBoxAdd.Text = "Ajouter un médicament";
            BtnSave.Text = "💾 Enregistrer";
            groupBoxAdd.Visible = !groupBoxAdd.Visible;
            ClearFields();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Medicine med = new Medicine
                {
                    Id_user = currentUser.Id,
                    Dosage = txtDosage.Text,
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Molecule = txtMolecule.Text
                };

                bool result;

                if (isEditMode)
                {
                    // Mode update
                    med.Id_medicine = editingMedicineId;

                    result = medicineDAO.Update(med);

                    if (result)
                        MessageBox.Show("✅ Médicament mis à jour !");
                    else
                        MessageBox.Show("❌ Erreur lors de la mise à jour.");
                }
                else
                {
                    // Mode insertion
                    result = medicineDAO.Insert(med);

                    if (result)
                        MessageBox.Show("✅ Médicament ajouté !");
                    else
                        MessageBox.Show("❌ Erreur lors de l'ajout.");
                }

                if (result)
                {
                    LoadMedicines();
                    groupBoxAdd.Visible = false;
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un médicament à supprimer.");
                return;
            }

            int id = (int)dgvMedicines.SelectedRows[0].Cells["Id_medicine"].Value;

            if (MessageBox.Show("Voulez-vous vraiment supprimer ce médicament ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                bool result = medicineDAO.Delete(id);
                if (result)
                {
                    MessageBox.Show("🗑️ Médicament supprimé avec succès !");
                    LoadMedicines();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.");
                }
            }
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un médicament à modifier.");
                return;
            }

            // Récupération ID
            editingMedicineId = (int)dgvMedicines.SelectedRows[0].Cells["Id_medicine"].Value;

            // Charger les infos via DAO
            Medicine? med = medicineDAO.GetById(editingMedicineId);

            if (med == null)
            {
                MessageBox.Show("Erreur : médicament introuvable.");
                return;
            }

            // Passer en mode édition
            isEditMode = true;
            groupBoxAdd.Text = "Modifier un médicament";
            BtnSave.Text = "💾 Mettre à jour";
            groupBoxAdd.Visible = true;

            // Remplir les champs
            txtDosage.Text = med.Dosage;
            txtName.Text = med.Name;
            txtDescription.Text = med.Description;
            txtMolecule.Text = med.Molecule;
        }
        private void ClearFields()
        {
            txtDosage.Clear();
            txtName.Clear();
            txtDescription.Clear();
            txtMolecule.Clear();
        }
    }
}
