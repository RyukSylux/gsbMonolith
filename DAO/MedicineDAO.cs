using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Provides methods to interact with the Medicine table in the database.
    /// Supports CRUD operations and fetching medicines with user information.
    /// </summary>
    public class MedicineDAO
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Retrieves all medicines along with their associated user information.
        /// </summary>
        /// <returns>A list of dynamic objects containing medicine and user data.</returns>
        public List<dynamic> GetAll()
        {
            var medicines = new List<dynamic>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            m.id_medicine,
                            m.id_user,
                            m.dosage,
                            m.name AS medicine_name,
                            m.description,
                            m.molecule,
                            u.firstname AS user_firstname,
                            u.name AS user_name
                        FROM Medicine m
                        INNER JOIN Users u ON m.id_user = u.id_user;
                    ";

                    using var cmd = new MySqlCommand(query, connection);
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        medicines.Add(new
                        {
                            Id_medicine = reader.GetInt32("id_medicine"),
                            Id_user = reader.GetInt32("id_user"),
                            Dosage = reader["dosage"].ToString(),
                            Name = reader["medicine_name"].ToString(),
                            Description = reader["description"].ToString(),
                            Molecule = reader["molecule"].ToString(),
                            User = $"{reader["user_firstname"]} {reader["user_name"]}" // Resolves the user
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching medicines: {ex.Message}");
                }
            }

            return medicines;
        }

        /// <summary>
        /// Retrieves all medicines with minimal information (id and name).
        /// </summary>
        /// <returns>A list of <see cref="Medicine"/> objects.</returns>
        public List<Medicine> GetAllMedicines()
        {
            List<Medicine> medicines = new List<Medicine>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id_medicine, name FROM Medicine ORDER BY name;";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        medicines.Add(new Medicine
                        {
                            Id_medicine = reader.GetInt32("id_medicine"),
                            Name = reader.GetString("name")
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetAllMedicines: " + ex.Message);
                }
            }

            return medicines;
        }

        /// <summary>
        /// Retrieves a single medicine by its ID, including the creator's user info.
        /// </summary>
        /// <param name="id">The ID of the medicine.</param>
        /// <returns>A <see cref="Medicine"/> object or null if not found.</returns>
        public Medicine? GetById(int id)
        {
            Medicine? med = null;

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand(@"
                        SELECT 
                            m.id_medicine,
                            m.id_user,
                            m.dosage,
                            m.name AS medicine_name,
                            m.description,
                            m.molecule,
                            u.name AS user_name,
                            u.firstname AS user_firstname
                        FROM Medicine m
                        INNER JOIN Users u ON m.id_user = u.id_user
                        WHERE m.id_medicine = @id;
                    ", connection);

                    myCommand.Parameters.AddWithValue("@id", id);

                    using var myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        // Safe read to avoid nulls
                        string dosage = myReader["dosage"]?.ToString() ?? "";
                        string name = myReader["medicine_name"]?.ToString() ?? "";
                        string description = myReader["description"]?.ToString() ?? "";
                        string molecule = myReader["molecule"]?.ToString() ?? "";

                        med = new Medicine(
                            myReader.GetInt32("id_medicine"),
                            myReader.GetInt32("id_user"),
                            dosage,
                            name,
                            description,
                            molecule
                        );

                        // Add creator info safely
                        string userFirst = myReader["user_firstname"]?.ToString() ?? "";
                        string userName = myReader["user_name"]?.ToString() ?? "";

                        med.Description = (med.Description ?? "") +
                                          $" (Added by {userFirst} {userName})";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching medicine by ID: {ex.Message}");
                }
            }

            return med;
        }

        /// <summary>
        /// Inserts a new medicine into the database.
        /// </summary>
        /// <param name="med">The <see cref="Medicine"/> object to insert.</param>
        /// <returns>True if the insert was successful; otherwise, false.</returns>
        public bool Insert(Medicine med)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand(@"
                        INSERT INTO Medicine (id_user, dosage, name, description, molecule)
                        VALUES (@id_user, @dosage, @name, @description, @molecule);
                    ", connection);

                    myCommand.Parameters.AddWithValue("@id_user", med.Id_user);
                    myCommand.Parameters.AddWithValue("@dosage", med.Dosage);
                    myCommand.Parameters.AddWithValue("@name", med.Name);
                    myCommand.Parameters.AddWithValue("@description", med.Description);
                    myCommand.Parameters.AddWithValue("@molecule", med.Molecule);

                    int rows = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting medicine: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Updates an existing medicine in the database.
        /// </summary>
        /// <param name="med">The <see cref="Medicine"/> object containing updated data.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public bool Update(Medicine med)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand(@"
                        UPDATE Medicine
                        SET id_user = @id_user,
                            dosage = @dosage,
                            name = @name,
                            description = @description,
                            molecule = @molecule
                        WHERE id_medicine = @id_medicine;
                    ", connection);

                    myCommand.Parameters.AddWithValue("@id_medicine", med.Id_medicine);
                    myCommand.Parameters.AddWithValue("@id_user", med.Id_user);
                    myCommand.Parameters.AddWithValue("@dosage", med.Dosage);
                    myCommand.Parameters.AddWithValue("@name", med.Name);
                    myCommand.Parameters.AddWithValue("@description", med.Description);
                    myCommand.Parameters.AddWithValue("@molecule", med.Molecule);

                    int rows = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating medicine: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes a medicine from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the medicine to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public bool Delete(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand(
                        "DELETE FROM Medicine WHERE id_medicine = @id;", connection);
                    myCommand.Parameters.AddWithValue("@id", id);

                    int rows = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting medicine: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
