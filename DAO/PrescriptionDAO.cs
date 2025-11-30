using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Data Access Object handling CRUD operations for prescriptions.
    /// It relies on <see cref="AppartientDAO"/> to manage associated medicines.
    /// </summary>
    public class PrescriptionDAO
    {
        private readonly Database db = new Database();
        private readonly AppartientDAO appartientDAO = new AppartientDAO();

        /// <summary>
        /// Retrieves a prescription by its ID.
        /// </summary>
        /// <param name="id_prescription">ID of the prescription.</param>
        /// <returns>A <see cref="Prescription"/> object, or null if not found.</returns>
        public Prescription? GetPrescriptionById(int id_prescription)
        {
            using var connection = db.GetConnection();

            try
            {
                connection.Open();

                var cmd = new MySqlCommand(
                    @"SELECT id_prescription, id_user, id_patient, validity
                      FROM Prescription 
                      WHERE id_prescription = @id_prescription;",
                    connection);

                cmd.Parameters.AddWithValue("@id_prescription", id_prescription);

                using var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                return new Prescription(
                    reader.GetInt32("id_prescription"),
                    reader.GetInt32("id_user"),
                    reader.GetInt32("id_patient"),
                    reader.GetDateTime("validity").ToString("yyyy-MM-dd")
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetPrescriptionById: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Creates a prescription and associates medicines to it within a single transaction.
        /// </summary>
        /// <param name="prescription">The prescription to insert.</param>
        /// <param name="medicines">List of (medicine ID, quantity) tuples.</param>
        /// <returns>True if creation succeeded; otherwise false.</returns>
        public bool CreatePrescriptionWithMedicines(Prescription prescription, List<(int Id_medicine, int Quantity)> medicines)
        {
            using var connection = db.GetConnection();
            MySqlTransaction? transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                // Insert main prescription
                var cmd = new MySqlCommand(
                    @"INSERT INTO Prescription (id_user, id_patient, validity)
                      VALUES (@id_user, @id_patient, @validity);",
                    connection, transaction);

                cmd.Parameters.AddWithValue("@id_user", prescription.Id_user);
                cmd.Parameters.AddWithValue("@id_patient", prescription.Id_patient);
                cmd.Parameters.AddWithValue("@validity", prescription.Date);
                cmd.ExecuteNonQuery();

                long newId = cmd.LastInsertedId;

                // Insert associations using AppartientDAO
                foreach (var med in medicines)
                {
                    var assoc = new Appartient(med.Id_medicine, (int)newId, med.Quantity);
                    appartientDAO.Insert(assoc, connection, transaction);
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error CreatePrescriptionWithMedicines: " + ex.Message);

                try { transaction?.Rollback(); } catch { }

                return false;
            }
        }

        /// <summary>
        /// Retrieves all prescriptions with doctor, patient, and medicine information.
        /// </summary>
        /// <returns>A list of dynamic objects representing prescriptions.</returns>
        public List<dynamic> GetAllPrescriptions()
        {
            var prescriptions = new List<dynamic>();

            using var connection = db.GetConnection();

            try
            {
                connection.Open();

                string query = @"
                    SELECT 
                        p.id_prescription, p.validity,
                        u.firstname AS doctor_firstname, u.name AS doctor_name,
                        pa.firstname AS patient_firstname, pa.name AS patient_name, pa.age AS patient_age,
                        GROUP_CONCAT(CONCAT(m.name, ' (', a.quantity, ')') SEPARATOR ', ') AS medicines
                    FROM Prescription p
                    INNER JOIN Users u ON p.id_user = u.id_user
                    INNER JOIN Patients pa ON p.id_patient = pa.id_patient
                    LEFT JOIN Appartient a ON p.id_prescription = a.id_prescription
                    LEFT JOIN Medicine m ON a.id_medicine = m.id_medicine
                    GROUP BY p.id_prescription
                    ORDER BY p.id_prescription ASC;";

                using var cmd = new MySqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    prescriptions.Add(new
                    {
                        Id = reader.GetInt32("id_prescription"),
                        Validité = reader.GetDateTime("validity").ToString("yyyy-MM-dd"),
                        Docteur = $"{reader["doctor_firstname"]} {reader["doctor_name"]}",
                        Patient = $"{reader["patient_firstname"]} {reader["patient_name"]} ({reader["patient_age"]} years)",
                        Médicaments = reader["medicines"] != DBNull.Value ? reader["medicines"].ToString() : "None"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetAllPrescriptions: " + ex.Message);
            }

            return prescriptions;
        }

        /// <summary>
        /// Returns medicines and quantities associated with a prescription.
        /// </summary>
        /// <param name="id_prescription">ID of the prescription.</param>
        /// <returns>A list of (medicine ID, quantity) tuples.</returns>
        public List<(int Id_medicine, int Quantity)> GetMedicinesWithQuantities(int id_prescription)
        {
            var result = new List<(int, int)>();

            var assoc = appartientDAO.GetByPrescriptionId(id_prescription);

            foreach (var a in assoc)
                result.Add((a.Id_medicine, a.Quantity));

            return result;
        }

        /// <summary>
        /// Updates a prescription’s validity and optionally its associated medicines.
        /// </summary>
        /// <param name="id_prescription">ID of the prescription.</param>
        /// <param name="newValidity">New validity date.</param>
        /// <param name="medicines">List of new medicine associations.</param>
        /// <returns>True if the update succeeded; otherwise false.</returns>
        public bool UpdatePrescription(int id_prescription, string newValidity, List<(int Id_medicine, int Quantity)> medicines)
        {
            using var connection = db.GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Update validity
                using var updateCmd = new MySqlCommand(
                    @"UPDATE Prescription SET validity = @validity WHERE id_prescription = @id;",
                    connection, transaction);

                updateCmd.Parameters.AddWithValue("@validity", newValidity);
                updateCmd.Parameters.AddWithValue("@id", id_prescription);
                updateCmd.ExecuteNonQuery();

                // Fetch old medicines
                var old = appartientDAO.GetByPrescriptionId(id_prescription)
                                        .Select(a => (a.Id_medicine, a.Quantity))
                                        .OrderBy(x => x.Id_medicine)
                                        .ToList();

                var newList = medicines.OrderBy(x => x.Id_medicine).ToList();

                bool changed = !old.SequenceEqual(newList);

                if (changed)
                {
                    // Clear old
                    appartientDAO.DeleteByPrescriptionId(id_prescription, connection, transaction);

                    // Insert new
                    foreach (var med in medicines)
                    {
                        var assoc = new Appartient(med.Id_medicine, id_prescription, med.Quantity);
                        appartientDAO.Insert(assoc, connection, transaction);
                    }
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                try { transaction.Rollback(); } catch { }
                return false;
            }
        }

        /// <summary>
        /// Deletes a prescription and its associated medicines inside a single transaction.
        /// </summary>
        /// <param name="id_prescription">ID of the prescription to delete.</param>
        /// <returns>True if delete succeeded; otherwise false.</returns>
        public bool DeletePrescription(int id_prescription)
        {
            using var connection = db.GetConnection();
            MySqlTransaction? transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                // Delete associations
                appartientDAO.DeleteByPrescriptionId(id_prescription, connection, transaction);

                // Delete prescription
                var cmd = new MySqlCommand(
                    @"DELETE FROM Prescription WHERE id_prescription = @id;",
                    connection, transaction);

                cmd.Parameters.AddWithValue("@id", id_prescription);
                cmd.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error DeletePrescription: " + ex.Message);
                try { transaction?.Rollback(); } catch { }
                return false;
            }
        }
    }
}
