using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Provides methods to interact with the Patients table in the database.
    /// Supports CRUD operations and fetching patient data for display or comboboxes.
    /// </summary>
    public class PatientDAO
    {
        private readonly Database db = new Database();
        private readonly JournalDAO journalDAO = new JournalDAO();
        private readonly User currentUser;

        public PatientDAO(User user)
        {
            this.currentUser = user;
        }

        /// <summary>
        /// Retrieves a patient by their ID.
        /// </summary>
        /// <param name="id_patient">The ID of the patient.</param>
        /// <returns>A <see cref="Patient"/> object or null if not found.</returns>
        public Patient? GetPatientById(int id_patient)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Patients WHERE id_patient = @id_patient;";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_patient", id_patient);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Patient(
                                reader.GetInt32("id_patient"),
                                reader.GetInt32("id_user"),
                                reader.GetString("name"),
                                reader.GetInt32("age"),
                                reader.GetString("firstname"),
                                reader.GetString("gender")
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error GetPatientById: " + ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Inserts a patient object into the database.
        /// </summary>
        /// <param name="patient">The <see cref="Patient"/> object to insert.</param>
        /// <returns>True if insertion was successful; otherwise, false.</returns>
        public bool Insert(Patient patient)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Patients (id_user, name, firstname, age, gender)
                        VALUES (@id_user, @name, @firstname, @age, @gender);";

                    using MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_user", patient.Id_user);
                    cmd.Parameters.AddWithValue("@name", patient.Name);
                    cmd.Parameters.AddWithValue("@firstname", patient.Firstname);
                    cmd.Parameters.AddWithValue("@age", patient.Age);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        journalDAO.Add(new Journal(
                            currentUser.Id,
                            "Création Patient",
                            DateTime.Now,
                            "Patient",
                            $"Nom: {patient.Name} {patient.Firstname}",
                            $"Le patient {patient.Name} {patient.Firstname} a été créé par {currentUser.Name}."
                        ));
                    }

                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting patient: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieves all patients with their associated doctor's information.
        /// </summary>
        /// <returns>A list of dynamic objects representing patients and their doctors.</returns>
        public List<dynamic> GetAllPatients()
        {
            List<dynamic> patients = new List<dynamic>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            p.id_patient,
                            p.name AS patient_name,
                            p.firstname AS patient_firstname,
                            p.age,
                            p.gender,
                            u.firstname AS doctor_firstname,
                            u.name AS doctor_name
                        FROM Patients p
                        INNER JOIN Users u ON p.id_user = u.id_user
                        ORDER BY p.id_patient ASC;";

                    using MySqlCommand cmd = new MySqlCommand(query, connection);
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        patients.Add(new
                        {
                            Id = reader.GetInt32("id_patient"),
                            Name = reader["patient_name"].ToString(),
                            Firstname = reader["patient_firstname"].ToString(),
                            Age = reader.GetInt32("age"),
                            Gender = reader["gender"].ToString(),
                            Doctor = $"{reader["doctor_firstname"]} {reader["doctor_name"]}"
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error GetAllPatients: " + ex.Message);
                }
            }

            return patients;
        }

        /// <summary>
        /// Retrieves all patients for populating a combobox.
        /// </summary>
        /// <returns>A list of <see cref="Patient"/> objects.</returns>
        public List<Patient> GetPatientsForComboBox()
        {
            List<Patient> patients = new List<Patient>();
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT id_patient, id_user, name, firstname, age, gender
                        FROM Patients
                        ORDER BY name, firstname ASC;";

                    using MySqlCommand cmd = new MySqlCommand(query, connection);
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        patients.Add(new Patient(
                            reader.GetInt32("id_patient"),
                            reader.GetInt32("id_user"),
                            reader["name"].ToString(),
                            reader.GetInt32("age"),
                            reader["firstname"].ToString(),
                            reader["gender"].ToString()
                        ));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error GetPatientsForComboBox: " + ex.Message);
                }
            }
            return patients;
        }

        /// <summary>
        /// Deletes a patient by ID.
        /// </summary>
        /// <param name="id_patient">The ID of the patient to delete.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        public bool DeletePatient(int id_patient)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Patients WHERE id_patient = @id_patient;";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        journalDAO.Add(new Journal(
                            currentUser.Id,
                            "Suppression Patient",
                            DateTime.Now,
                            "Patient",
                            $"ID: {id_patient}",
                            $"Le patient ID {id_patient} a été supprimé par {currentUser.Name}."
                        ));
                    }

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error DeletePatient: " + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// Updates a patient in the database.
        /// </summary>
        /// <param name="patient">The <see cref="Patient"/> object with updated data.</param>
        /// <returns>True if update was successful; otherwise, false.</returns>
        public bool UpdatePatient(Patient patient)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE Patients 
                        SET id_user = @id_user, 
                            name = @name, 
                            age = @age, 
                            firstname = @firstname, 
                            gender = @gender
                        WHERE id_patient = @id_patient;";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_user", patient.Id_user);
                    cmd.Parameters.AddWithValue("@name", patient.Name);
                    cmd.Parameters.AddWithValue("@age", patient.Age);
                    cmd.Parameters.AddWithValue("@firstname", patient.Firstname);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@id_patient", patient.Id_patient);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        journalDAO.Add(new Journal(
                            currentUser.Id,
                            "Modification Patient",
                            DateTime.Now,
                            "Patient",
                            $"ID: {patient.Id_patient}",
                            $"Le patient {patient.Name} {patient.Firstname} a été modifié par {currentUser.Name}."
                        ));
                    }

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error UpdatePatient: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
