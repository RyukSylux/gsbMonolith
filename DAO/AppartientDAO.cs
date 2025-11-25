using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Data Access Object (DAO) for handling the relationships between prescriptions and medicines.
    /// Provides CRUD operations for the <see cref="Appartient"/> model.
    /// </summary>
    public class AppartientDAO
    {
        /// <summary>
        /// Represents the database connection manager.
        /// </summary>
        private readonly Database db = new Database();

        /// <summary>
        /// Retrieves all prescription-medicine associations.
        /// </summary>
        /// <returns>A list of <see cref="Appartient"/> objects representing all associations.</returns>
        public List<Appartient> GetAll()
        {
            List<Appartient> list = new List<Appartient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        @"SELECT id_prescription, id_medicine, quantity FROM Appartient;", connection);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Appartient a = new Appartient(
                            reader.GetInt32("id_medicine"),
                            reader.GetInt32("id_prescription"),
                            reader.IsDBNull(reader.GetOrdinal("quantity")) ? 0 : reader.GetInt32("quantity")
                        );
                        list.Add(a);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetAll Appartient: {ex.Message}");
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieves all medicines associated with a specific prescription.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription.</param>
        /// <returns>A list of <see cref="Appartient"/> objects for the given prescription.</returns>
        public List<Appartient> GetByPrescriptionId(int id_prescription)
        {
            List<Appartient> list = new List<Appartient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        @"SELECT id_prescription, id_medicine, quantity 
                          FROM Appartient 
                          WHERE id_prescription = @id_prescription;", connection);

                    cmd.Parameters.AddWithValue("@id_prescription", id_prescription);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Appartient a = new Appartient(
                            reader.GetInt32("id_medicine"),
                            reader.GetInt32("id_prescription"),
                            reader.IsDBNull(reader.GetOrdinal("quantity")) ? 0 : reader.GetInt32("quantity")
                        );
                        list.Add(a);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetByPrescriptionId: {ex.Message}");
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieves all prescriptions that include a specific medicine.
        /// </summary>
        /// <param name="id_medicine">The ID of the medicine.</param>
        /// <returns>A list of <see cref="Appartient"/> objects for the given medicine.</returns>
        public List<Appartient> GetByMedicineId(int id_medicine)
        {
            List<Appartient> list = new List<Appartient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(
                        @"SELECT id_prescription, id_medicine, quantity 
                          FROM Appartient 
                          WHERE id_medicine = @id_medicine;", connection);

                    cmd.Parameters.AddWithValue("@id_medicine", id_medicine);

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Appartient a = new Appartient(
                            reader.GetInt32("id_medicine"),
                            reader.GetInt32("id_prescription"),
                            reader.IsDBNull(reader.GetOrdinal("quantity")) ? 0 : reader.GetInt32("quantity")
                        );
                        list.Add(a);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in GetByMedicineId: {ex.Message}");
                }
            }

            return list;
        }

        /// <summary>
        /// Inserts a new prescription-medicine association into the database.
        /// </summary>
        /// <param name="a">The <see cref="Appartient"/> object to insert.</param>
        /// <returns>True if the insertion succeeded; otherwise, false.</returns>
        public bool Insert(Appartient a)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO Appartient (id_prescription, id_medicine, quantity)
                        VALUES (@id_prescription, @id_medicine, @quantity);
                    ", connection);

                    cmd.Parameters.AddWithValue("@id_prescription", a.Id_prescription);
                    cmd.Parameters.AddWithValue("@id_medicine", a.Id_medicine);
                    cmd.Parameters.AddWithValue("@quantity", a.Quantity);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Insert Appartient: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes a specific prescription-medicine association from the database.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription.</param>
        /// <param name="id_medicine">The ID of the medicine.</param>
        /// <returns>True if the deletion succeeded; otherwise, false.</returns>
        public bool Delete(int id_prescription, int id_medicine)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                        DELETE FROM Appartient
                        WHERE id_prescription = @id_prescription AND id_medicine = @id_medicine;
                    ", connection);

                    cmd.Parameters.AddWithValue("@id_prescription", id_prescription);
                    cmd.Parameters.AddWithValue("@id_medicine", id_medicine);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Delete Appartient: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes all associations related to a specific prescription.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription.</param>
        /// <returns>True if the deletion succeeded; otherwise, false.</returns>
        public bool DeleteByPrescriptionId(int id_prescription)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                        DELETE FROM Appartient WHERE id_prescription = @id_prescription;
                    ", connection);

                    cmd.Parameters.AddWithValue("@id_prescription", id_prescription);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in DeleteByPrescriptionId Appartient: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
