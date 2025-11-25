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
        /// Creates a new patient in the database.
        /// </summary>
        /// <param name="id_user">The ID of the user who added the patient.</param>
        /// <param name="name">The patient's last name.</param>
        /// <param name="age">The patient's age.</param>
        /// <param name="firstname">The patient's first name.</param>
        /// <param name="gender">The patient's gender.</param>
        /// <returns>True if creation was successful; otherwise, false.</returns>
        public bool CreatePatient(int id_user, string name, int age, string firstname, string gender)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO Patients (id_user, name, age, firstname, gender) 
                                     VALUES (@id_user, @name, @age, @firstname, @gender);";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_user", id_user);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@gender", gender);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error CreatePatient: " + ex.Message);
                    return false;
                }
            }
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
                    cmd.Parameters.AddWithValue("@id_patient", id_patient);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
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
