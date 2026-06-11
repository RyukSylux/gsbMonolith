using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Provides methods to interact with the Category and ForbiddenMedicine tables in the database.
    /// </summary>
    public class CategoryDAO
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Retrieves all patient categories.
        /// </summary>
        /// <returns>A list of Category objects.</returns>
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id_category, name FROM Category ORDER BY name ASC;";
                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category(
                                reader.GetInt32("id_category"),
                                reader.GetString("name")
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error GetAllCategories: {ex.Message}");
                }
            }

            return categories;
        }

        /// <summary>
        /// Checks if a specific medicine is forbidden for a specific patient category.
        /// </summary>
        /// <param name="idCategory">The category identifier.</param>
        /// <param name="idMedicine">The medicine identifier.</param>
        /// <returns>True if the medicine is forbidden; otherwise, false.</returns>
        public bool IsMedicineForbidden(int idCategory, int idMedicine)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM ForbiddenMedicine 
                        WHERE id_category = @id_category AND id_medicine = @id_medicine;";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id_category", idCategory);
                        cmd.Parameters.AddWithValue("@id_medicine", idMedicine);

                        long count = Convert.ToInt64(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking forbidden medicine: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieves a category name by its identifier.
        /// </summary>
        public string GetCategoryName(int idCategory)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT name FROM Category WHERE id_category = @id_category;";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id_category", idCategory);
                        object val = cmd.ExecuteScalar();
                        return val != null ? val.ToString() : "";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error GetCategoryName: {ex.Message}");
                    return "";
                }
            }
        }
    }
}
