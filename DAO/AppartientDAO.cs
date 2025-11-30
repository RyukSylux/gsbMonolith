using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Data Access Object (DAO) responsible for managing the relationships between
    /// prescriptions and medicines. Provides CRUD operations for the <see cref="Appartient"/> entity.
    /// </summary>
    public class AppartientDAO
    {
        /// <summary>
        /// Handles the creation of MySQL connections.
        /// </summary>
        private readonly Database db = new Database();

        /// <summary>
        /// Retrieves all medicine associations linked to a specific prescription.
        /// </summary>
        /// <param name="id_prescription">The unique identifier of the prescription.</param>
        /// <returns>
        /// A list of <see cref="Appartient"/> instances representing all medicine entries
        /// tied to the given prescription.
        /// </returns>
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
                            reader.IsDBNull(reader.GetOrdinal("quantity"))
                                ? 0
                                : reader.GetInt32("quantity")
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
        /// Inserts a new association between a prescription and a medicine.
        /// </summary>
        /// <param name="a">The <see cref="Appartient"/> object containing the association data.</param>
        /// <param name="conn">An open <see cref="MySqlConnection"/> instance.</param>
        /// <param name="transaction">
        /// The active <see cref="MySqlTransaction"/> used to ensure atomicity when multiple database operations must succeed together.
        /// </param>
        /// <returns>
        /// <c>true</c> if the insertion was performed successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The connection and transaction are passed as parameters to ensure that the insert
        /// can be part of a larger atomic operation (e.g. inserting a prescription and its associated medicines).
        /// </remarks>
        public bool Insert(Appartient a, MySqlConnection conn, MySqlTransaction? transaction)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(@"
                    INSERT INTO Appartient (id_prescription, id_medicine, quantity)
                    VALUES (@id_prescription, @id_medicine, @quantity);
                ", conn, transaction);

                cmd.Parameters.AddWithValue("@id_prescription", a.Id_prescription);
                cmd.Parameters.AddWithValue("@id_medicine", a.Id_medicine);
                cmd.Parameters.AddWithValue("@quantity", a.Quantity);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes all medicine associations linked to a specific prescription.
        /// </summary>
        /// <param name="id_prescription">The ID of the prescription to clear associations for.</param>
        /// <param name="conn">An open <see cref="MySqlConnection"/> instance.</param>
        /// <param name="transaction">
        /// The active <see cref="MySqlTransaction"/> used to guarantee that this deletion can be grouped
        /// with other database operations in a single atomic transaction.
        /// </param>
        /// <returns>
        /// <c>true</c> if the deletion affected at least one row; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Passing the connection and transaction allows this operation to participate in an atomic
        /// workflow, ensuring data consistency when deleting and reinserting associations.
        /// </remarks>
        public bool DeleteByPrescriptionId(int id_prescription, MySqlConnection conn, MySqlTransaction? transaction)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    @"DELETE FROM Appartient WHERE id_prescription = @id;",
                    conn, transaction);

                cmd.Parameters.AddWithValue("@id", id_prescription);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
