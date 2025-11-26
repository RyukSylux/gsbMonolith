using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Provides methods to interact with the Prescription table in the database.
    /// Supports CRUD operations and handling associated medicines.
    /// </summary>
    public class PrescriptionDAO
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Retrieves a prescription by its ID.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription.</param>
        /// <returns>A <see cref="Prescription"/> object or null if not found.</returns>
        public Prescription? GetPrescriptionById(int id_prescription)
        {
            using var connection = db.GetConnection();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(
                    @"SELECT id_prescription, id_user, id_patient, validity
                      FROM Prescription 
                      WHERE id_prescription = @id_prescription;", connection);
                cmd.Parameters.AddWithValue("@id_prescription", id_prescription);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Prescription(
                        reader.GetInt32("id_prescription"),
                        reader.GetInt32("id_user"),
                        reader.GetInt32("id_patient"),
                        reader.GetDateTime("validity").ToString("yyyy-MM-dd")
                    );
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetPrescriptionById: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Creates a prescription along with associated medicines and quantities.
        /// </summary>
        /// <param name="prescription">The <see cref="Prescription"/> object to create.</param>
        /// <param name="medicines">A list of medicine IDs and their quantities.</param>
        /// <returns>True if creation was successful; otherwise, false.</returns>
        public bool CreatePrescriptionWithMedicines(Prescription prescription, List<(int Id_medicine, int Quantity)> medicines)
        {
            using var connection = db.GetConnection();
            MySqlTransaction? transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                MySqlCommand cmd = new MySqlCommand(
                    @"INSERT INTO Prescription (id_user, id_patient, validity)
                      VALUES (@id_user, @id_patient, @validity);", connection, transaction);
                cmd.Parameters.AddWithValue("@id_user", prescription.Id_user);
                cmd.Parameters.AddWithValue("@id_patient", prescription.Id_patient);
                cmd.Parameters.AddWithValue("@validity", prescription.Date);
                cmd.ExecuteNonQuery();
                long newPrescriptionId = cmd.LastInsertedId;

                foreach (var med in medicines)
                {
                    MySqlCommand medCmd = new MySqlCommand(
                        @"INSERT INTO Appartient (id_prescription, id_medicine, quantity)
                          VALUES (@id_prescription, @id_medicine, @quantity);", connection, transaction);
                    medCmd.Parameters.AddWithValue("@id_prescription", newPrescriptionId);
                    medCmd.Parameters.AddWithValue("@id_medicine", med.Id_medicine);
                    medCmd.Parameters.AddWithValue("@quantity", med.Quantity);
                    medCmd.ExecuteNonQuery();
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
        /// Retrieves all prescriptions with doctor, patient, and medicine details.
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
                    GROUP BY p.id_prescription, p.validity, u.firstname, u.name, pa.firstname, pa.name, pa.age
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
        /// Retrieves medicines and their quantities for a given prescription.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription.</param>
        /// <returns>A list of tuples containing medicine ID and quantity.</returns>
        public List<(int Id_medicine, int Quantity)> GetMedicinesWithQuantities(int id_prescription)
        {
            var list = new List<(int, int)>();
            using var connection = db.GetConnection();
            try
            {
                connection.Open();
                string query = @"SELECT id_medicine, quantity FROM Appartient WHERE id_prescription = @id";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id_prescription);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((reader.GetInt32("id_medicine"), reader.GetInt32("quantity")));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetMedicinesWithQuantities: " + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Updates a prescription's validity date and its medicines/quantities.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription.</param>
        /// <param name="newValidity">The new validity date.</param>
        /// <param name="medicines">List of medicines and quantities.</param>
        /// <returns>True if update was successful; otherwise, false.</returns>
        public bool UpdatePrescription(int id_prescription, string newValidity, List<(int Id_medicine, int Quantity)> medicines)
        {
            using var connection = db.GetConnection();
            MySqlTransaction? transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                MySqlCommand cmd = new MySqlCommand(
                    @"UPDATE Prescription SET validity = @validity WHERE id_prescription = @id;", connection, transaction);
                cmd.Parameters.AddWithValue("@validity", newValidity);
                cmd.Parameters.AddWithValue("@id", id_prescription);
                cmd.ExecuteNonQuery();

                MySqlCommand delCmd = new MySqlCommand(
                    @"DELETE FROM Appartient WHERE id_prescription = @id;", connection, transaction);
                delCmd.Parameters.AddWithValue("@id", id_prescription);
                delCmd.ExecuteNonQuery();

                foreach (var med in medicines)
                {
                    MySqlCommand medCmd = new MySqlCommand(
                        @"INSERT INTO Appartient (id_prescription, id_medicine, quantity)
                          VALUES (@id_prescription, @id_medicine, @quantity);", connection, transaction);
                    medCmd.Parameters.AddWithValue("@id_prescription", id_prescription);
                    medCmd.Parameters.AddWithValue("@id_medicine", med.Id_medicine);
                    medCmd.Parameters.AddWithValue("@quantity", med.Quantity);
                    medCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error UpdatePrescription: " + ex.Message);
                try { transaction?.Rollback(); } catch { }
                return false;
            }
        }

        /// <summary>
        /// Deletes a prescription and its associated medicines.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription to delete.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        public bool DeletePrescription(int id_prescription)
        {
            using var connection = db.GetConnection();
            MySqlTransaction? transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                MySqlCommand delAppCmd = new MySqlCommand(
                    @"DELETE FROM Appartient WHERE id_prescription = @id;", connection, transaction);
                delAppCmd.Parameters.AddWithValue("@id", id_prescription);
                delAppCmd.ExecuteNonQuery();

                MySqlCommand delPrescCmd = new MySqlCommand(
                    @"DELETE FROM Prescription WHERE id_prescription = @id;", connection, transaction);
                delPrescCmd.Parameters.AddWithValue("@id", id_prescription);
                delPrescCmd.ExecuteNonQuery();

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
