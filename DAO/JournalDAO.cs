using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;

namespace gsbMonolith.DAO
{
    public class JournalDAO
    {
        private readonly Database db = new Database();

        public bool Add(Journal journal)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Journal (id_user, event_name, date, type, related_object, description) 
                        VALUES (@id_user, @event_name, @date, @type, @related_object, @description);";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        if (journal.IdUser.HasValue)
                            command.Parameters.AddWithValue("@id_user", journal.IdUser.Value);
                        else
                            command.Parameters.AddWithValue("@id_user", DBNull.Value);

                        command.Parameters.AddWithValue("@event_name", journal.EventName);
                        command.Parameters.AddWithValue("@date", journal.Date);
                        command.Parameters.AddWithValue("@type", journal.Type);
                        command.Parameters.AddWithValue("@related_object", journal.RelatedObject);
                        command.Parameters.AddWithValue("@description", journal.Description);
                        
                        int rows = command.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error JournalDAO.Add: " + ex.Message);
                    return false;
                }
            }
        }
        public System.Collections.Generic.List<dynamic> GetAll()
        {
            var logs = new System.Collections.Generic.List<dynamic>();
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT j.id, j.event_name, j.date, j.type, j.related_object, j.description, u.name, u.firstname
                        FROM Journal j
                        LEFT JOIN Users u ON j.id_user = u.id_user
                        ORDER BY j.date DESC;";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logs.Add(new
                            {
                                Id = reader.GetInt32("id"),
                                Date = reader.IsDBNull(reader.GetOrdinal("date")) ? "" : reader.GetDateTime("date").ToString("g"),
                                Event = reader.IsDBNull(reader.GetOrdinal("event_name")) ? "" : reader.GetString("event_name"),
                                User = reader.IsDBNull(reader.GetOrdinal("firstname")) ? "Unknown" : $"{reader.GetString("firstname")} {reader.GetString("name")}",
                                Type = reader.IsDBNull(reader.GetOrdinal("type")) ? "" : reader.GetString("type"),
                                Object = reader.IsDBNull(reader.GetOrdinal("related_object")) ? "" : reader.GetString("related_object"),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error JournalDAO.GetAll: " + ex.Message);
                }
            }
            return logs;
        }
    }
}
