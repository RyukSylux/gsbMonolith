using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Fournit des méthodes pour interagir avec la table Adherents dans la base de données.
    /// </summary>
    public class AdherentDAO
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Récupère tous les adhérents.
        /// </summary>
        public List<Adherent> GetAllAdherents()
        {
            var adherents = new List<Adherent>();
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Adherents;";
                    using var cmd = new MySqlCommand(query, connection);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        adherents.Add(new Adherent(
                            reader.GetInt32("id_adherent"),
                            reader.GetString("nom"),
                            reader.GetString("prenom"),
                            reader.GetString("email"),
                            reader.GetString("telephone"),
                            reader.GetDateTime("date_adhesion"),
                            reader.GetBoolean("cotisation_reglee")
                        ));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des adhérents : {ex.Message}", "Erreur SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return adherents;
        }

        /// <summary>
        /// Ajoute un nouvel adhérent.
        /// </summary>
        public bool CreateAdherent(Adherent adherent)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"INSERT INTO Adherents (nom, prenom, email, telephone, date_adhesion, cotisation_reglee) 
                                     VALUES (@Nom, @Prenom, @Email, @Telephone, @DateAdhesion, @Cotisation);";
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Nom", adherent.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", adherent.Prenom);
                    cmd.Parameters.AddWithValue("@Email", adherent.Email);
                    cmd.Parameters.AddWithValue("@Telephone", adherent.Telephone);
                    cmd.Parameters.AddWithValue("@DateAdhesion", adherent.DateAdhesion);
                    cmd.Parameters.AddWithValue("@Cotisation", adherent.CotisationReglee);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout de l'adhérent : {ex.Message}", "Erreur SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Met à jour un adhérent existant.
        /// </summary>
        public bool UpdateAdherent(Adherent adherent)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE Adherents SET nom = @Nom, prenom = @Prenom, email = @Email, 
                                     telephone = @Telephone, date_adhesion = @DateAdhesion, cotisation_reglee = @Cotisation 
                                     WHERE id_adherent = @Id;";
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Nom", adherent.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", adherent.Prenom);
                    cmd.Parameters.AddWithValue("@Email", adherent.Email);
                    cmd.Parameters.AddWithValue("@Telephone", adherent.Telephone);
                    cmd.Parameters.AddWithValue("@DateAdhesion", adherent.DateAdhesion);
                    cmd.Parameters.AddWithValue("@Cotisation", adherent.CotisationReglee);
                    cmd.Parameters.AddWithValue("@Id", adherent.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la modification de l'adhérent : {ex.Message}", "Erreur SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Supprime un adhérent par son ID.
        /// </summary>
        public bool DeleteAdherent(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Adherents WHERE id_adherent = @Id;";
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}
