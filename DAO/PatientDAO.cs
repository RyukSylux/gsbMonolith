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
                    string query = @"
                        SELECT p.*, pc.id_category 
                        FROM Patients p 
                        LEFT JOIN PatientCategory pc ON p.id_patient = pc.id_patient 
                        WHERE p.id_patient = @id_patient;";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_patient", id_patient);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var patient = new Patient(
                                reader.GetInt32("id_patient"),
                                reader.GetInt32("id_user"),
                                reader.GetString("name"),
                                reader.GetInt32("age"),
                                reader.GetString("firstname"),
                                reader.GetString("gender")
                            );
                            if (!reader.IsDBNull(reader.GetOrdinal("id_category")))
                            {
                                patient.Id_category = reader.GetInt32("id_category");
                            }
                            return patient;
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
                        VALUES (@id_user, @name, @firstname, @age, @gender);
                        SELECT LAST_INSERT_ID();";

                    using MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_user", patient.Id_user);
                    cmd.Parameters.AddWithValue("@name", patient.Name);
                    cmd.Parameters.AddWithValue("@firstname", patient.Firstname);
                    cmd.Parameters.AddWithValue("@age", patient.Age);
                    cmd.Parameters.AddWithValue("@gender", patient.Gender);

                    int patientId = Convert.ToInt32(cmd.ExecuteScalar());
                    patient.Id_patient = patientId;

                    if (patient.Id_category.HasValue)
                    {
                        string linkQuery = "INSERT INTO PatientCategory (id_patient, id_category) VALUES (@id_patient, @id_category);";
                        using MySqlCommand linkCmd = new MySqlCommand(linkQuery, connection);
                        linkCmd.Parameters.AddWithValue("@id_patient", patientId);
                        linkCmd.Parameters.AddWithValue("@id_category", patient.Id_category.Value);
                        linkCmd.ExecuteNonQuery();
                    }
                    return patientId > 0;
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
                    // INNER JOIN links each patient (Patients table) to their corresponding doctor (Users table)
                    // using the id_user foreign key, ensuring we retrieve doctor details with patient profiles.
                    string query = @"
                        SELECT 
                            p.id_patient,
                            p.name AS patient_name,
                            p.firstname AS patient_firstname,
                            p.age,
                            p.gender,
                            u.firstname AS doctor_firstname,
                            u.name AS doctor_name,
                            c.name AS category_name
                        FROM Patients p
                        INNER JOIN Users u ON p.id_user = u.id_user
                        LEFT JOIN PatientCategory pc ON p.id_patient = pc.id_patient
                        LEFT JOIN Category c ON pc.id_category = c.id_category
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
                            Category = reader["category_name"] == DBNull.Value ? "Aucune" : reader["category_name"].ToString(),
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

                    // Update category association
                    string deleteLinkQuery = "DELETE FROM PatientCategory WHERE id_patient = @id_patient;";
                    using (MySqlCommand delCmd = new MySqlCommand(deleteLinkQuery, connection))
                    {
                        delCmd.Parameters.AddWithValue("@id_patient", patient.Id_patient);
                        delCmd.ExecuteNonQuery();
                    }

                    if (patient.Id_category.HasValue)
                    {
                        string insertLinkQuery = "INSERT INTO PatientCategory (id_patient, id_category) VALUES (@id_patient, @id_category);";
                        using (MySqlCommand insCmd = new MySqlCommand(insertLinkQuery, connection))
                        {
                            insCmd.Parameters.AddWithValue("@id_patient", patient.Id_patient);
                            insCmd.Parameters.AddWithValue("@id_category", patient.Id_category.Value);
                            insCmd.ExecuteNonQuery();
                        }
                    }

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

        /// <summary>
        /// Retrieves all patients assigned to a specific doctor (user ID), with their associated doctor's information.
        /// </summary>
        /// <param name="id_user">The unique identifier of the doctor/user.</param>
        /// <returns>A list of dynamic objects representing patients and their doctor.</returns>
        public List<dynamic> GetPatientsByDoctorId(int id_user)
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
                            u.name AS doctor_name,
                            c.name AS category_name
                        FROM Patients p
                        INNER JOIN Users u ON p.id_user = u.id_user
                        LEFT JOIN PatientCategory pc ON p.id_patient = pc.id_patient
                        LEFT JOIN Category c ON pc.id_category = c.id_category
                        WHERE p.id_user = @id_user
                        ORDER BY p.id_patient ASC;";

                    using MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_user", id_user);
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
                            Category = reader["category_name"] == DBNull.Value ? "Aucune" : reader["category_name"].ToString(),
                            Doctor = $"{reader["doctor_firstname"]} {reader["doctor_name"]}"
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error GetPatientsByDoctorId: " + ex.Message);
                }
            }

            return patients;
        }

        /// <summary>
        /// Retrieves all patients assigned to a specific doctor (user ID) for populating a combobox.
        /// </summary>
        /// <param name="id_user">The unique identifier of the doctor/user.</param>
        /// <returns>A list of <see cref="Patient"/> objects.</returns>
        public List<Patient> GetPatientsForComboBoxByDoctorId(int id_user)
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
                        WHERE id_user = @id_user
                        ORDER BY name, firstname ASC;";

                    using MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id_user", id_user);
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var patient = new Patient(
                            reader.GetInt32("id_patient"),
                            reader.GetInt32("id_user"),
                            reader["name"].ToString(),
                            reader.GetInt32("age"),
                            reader["firstname"].ToString(),
                            reader["gender"].ToString()
                        );
                        patients.Add(patient);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error GetPatientsForComboBoxByDoctorId: " + ex.Message);
                }
            }
            return patients;
        }
    }
}
