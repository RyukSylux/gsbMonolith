using gsbMonolith.DAO;
using gsbMonolith.Models;
using gsbMonolith.Utils;
using gsbMonolith.Views.Modals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gsbMonolith.Views
{
    /// <summary>
    /// Represents the view for managing prescriptions, allowing users to create, view, edit, delete, and export prescriptions.
    /// </summary>
    public class PrescriptionsView : UserControl
    {
        private PrescriptionDAO prescriptionDAO = new PrescriptionDAO();
        private PatientDAO patientDAO = new PatientDAO();
        private MedicineDAO medicineDAO = new MedicineDAO();
        private UserDAO userDAO = new UserDAO();
        private User currentUser;
        
        // Main UI
        private DataGridView dgvPrescriptions;
        private Panel headerPanel;
        private Button btnExport, btnAdd;

        public PrescriptionsView(User user)
        {
            currentUser = user;
            SetupUI();
            LoadPrescriptions();
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
                Text = "Prescriptions", 
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
                Size = new Size(150, 35),
                Location = new Point(headerPanel.Width - 550, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnDelete.Click += BtnDelete_Click;

            btnAdd = new Button
            {
                Text = "Nouvelle Prescription",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(180, 35),
                Location = new Point(headerPanel.Width - 380, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => OpenPrescriptionModal(null);

            btnExport = new Button
            {
                Text = "Exporter PDF",
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Size = new Size(150, 35),
                Location = new Point(headerPanel.Width - 170, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnExport.Click += BtnExport_Click;

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(btnDelete);
            headerPanel.Controls.Add(btnAdd);
            headerPanel.Controls.Add(btnExport);

            // --- Grid ---
            dgvPrescriptions = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            dgvPrescriptions.DataBindingComplete += (s, e) => dgvPrescriptions.ClearSelection();
            
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvPrescriptions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPrescriptions.ColumnHeadersHeight = 40;
            dgvPrescriptions.DefaultCellStyle.Padding = new Padding(10);
            dgvPrescriptions.RowTemplate.Height = 40;

            dgvPrescriptions.CellDoubleClick += (s, e) => {
                if(e.RowIndex >= 0) 
                {
                    int id = (int)dgvPrescriptions.Rows[e.RowIndex].Cells["Id"].Value;
                    OpenPrescriptionModal(prescriptionDAO.GetPrescriptionById(id));
                }
            };

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Modifier", null, (s, e) => {
                 if(dgvPrescriptions.SelectedRows.Count > 0) 
                 {
                    int id = (int)dgvPrescriptions.SelectedRows[0].Cells["Id"].Value;
                    OpenPrescriptionModal(prescriptionDAO.GetPrescriptionById(id));
                 }
            });
            ctx.Items.Add("Supprimer", null, BtnDelete_Click);
            dgvPrescriptions.ContextMenuStrip = ctx;

            // Assembly
            this.Controls.Add(dgvPrescriptions);
            this.Controls.Add(headerPanel);
        }

        private void LoadPrescriptions()
        {
            try
            {
                dgvPrescriptions.DataSource = prescriptionDAO.GetAllPrescriptions();
                if (dgvPrescriptions.Columns["Id"] != null) dgvPrescriptions.Columns["Id"].Visible = false;
            }
            catch(Exception ex) { MessageBox.Show("Erreur: " + ex.Message); }
        }

        private void OpenPrescriptionModal(Prescription prescription)
        {
            List<(int Id, int Quantity)> existingMeds = null;
            if (prescription != null)
            {
                existingMeds = prescriptionDAO.GetMedicinesWithQuantities(prescription.Id_prescription)
                                              .Select(m => (m.Id_medicine, m.Quantity))
                                              .ToList();
            }

            using (var modal = new PrescriptionEditForm(prescription, existingMeds))
            {
                if (modal.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var medList = modal.SelectedMeds;

                        if (prescription != null)
                        {
                            prescriptionDAO.UpdatePrescription(prescription.Id_prescription, currentUser.Id, modal.SelectedPatientId, modal.ValidityDate, medList);
                        }
                        else
                        {
                            var p = new Prescription(0, currentUser.Id, modal.SelectedPatientId, modal.ValidityDate);
                            prescriptionDAO.CreatePrescriptionWithMedicines(p, medList);
                        }

                        LoadPrescriptions();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement: " + ex.Message);
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPrescriptions.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Supprimer ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = (int)dgvPrescriptions.SelectedRows[0].Cells["Id"].Value;
                prescriptionDAO.DeletePrescription(id);
                LoadPrescriptions();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvPrescriptions.SelectedRows.Count == 0) { MessageBox.Show("Sélectionnez une ligne."); return; }

            try 
            {
                int idPresc = Convert.ToInt32(dgvPrescriptions.SelectedRows[0].Cells["Id"].Value);
                var presc = prescriptionDAO.GetPrescriptionById(idPresc);
                var patient = patientDAO.GetPatientById(presc.Id_patient);
                
                var meds = prescriptionDAO.GetMedicinesWithQuantities(idPresc)
                                          .Select(m => (Med: medicineDAO.GetById(m.Id_medicine), m.Quantity))
                                          .Where(x => x.Med != null)
                                          .Select(x => (x.Med!, x.Quantity))
                                          .ToList();

                var doctor = userDAO.GetUserById(presc.Id_user);

                PdfExporter.ExportPrescription(presc, patient, doctor, meds);
            }
            catch(Exception ex) { MessageBox.Show("Erreur PDF: " + ex.Message); }
        }
    }
}
