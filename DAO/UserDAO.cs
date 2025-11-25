using gsbMonolith.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace gsbMonolith.DAO
{
    /// <summary>
    /// Provides methods to interact with the Users table in the database.
    /// Supports login, registration, and CRUD operations.
    /// </summary>
    public class UserDAO
    {
        private readonly Database db = new Database();

        /// <summary>
        /// Attempts to login a user with the specified email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The <see cref="User"/> object if successful; otherwise, null.</returns>
        public User? Login(string email, string password)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand myCommand = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = @"SELECT * FROM Users WHERE email = @email AND password = SHA2(@password, 256);"
                    };
                    myCommand.Parameters.AddWithValue("@email", email);
                    myCommand.Parameters.AddWithValue("@password", password);

                    using var myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        int id = myReader.GetInt32("id_user");
                        string name = myReader.GetString("name");
                        string firstname = myReader.GetString("firstname");
                        bool role = myReader.GetBoolean("role");
                        connection.Close();
                        return new User(id, name, firstname, role, email);
                    }

                    connection.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Login error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <param name="name">User's last name.</param>
        /// <param name="firstname">User's first name.</param>
        /// <returns>True if registration is successful; otherwise, false.</returns>
        public bool Register(string email, string password, string name, string firstname)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                        string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(firstname))
                    {
                        MessageBox.Show("All fields are required.", "Registration Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE email = @Email;";
                    using var checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (exists > 0)
                    {
                        MessageBox.Show("A user with this email already exists.", "Registration Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    string insertQuery = @"
                        INSERT INTO Users (email, password, name, firstname, role)
                        VALUES (@Email, SHA2(@Password, 256), @Name, @Firstname, @Role);";

                    using var insertCmd = new MySqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", password);
                    insertCmd.Parameters.AddWithValue("@Name", name);
                    insertCmd.Parameters.AddWithValue("@Firstname", firstname);
                    insertCmd.Parameters.AddWithValue("@Role", false);

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration successful! You can now login.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while creating the account.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (MySqlException sqlEx)
                {
                    MessageBox.Show($"SQL error: {sqlEx.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects.</returns>
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id_user, name, firstname, email, role FROM Users;";
                    using var cmd = new MySqlCommand(query, connection);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User(
                            reader.GetInt32("id_user"),
                            reader.GetString("name"),
                            reader.GetString("firstname"),
                            reader.GetBoolean("role"),
                            reader.GetString("email")
                        ));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return users;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <param name="name">User's last name.</param>
        /// <param name="firstname">User's first name.</param>
        /// <param name="role">Whether the user is an administrator.</param>
        /// <returns>True if creation is successful; otherwise, false.</returns>
        public bool CreateUser(string email, string password, string name, string firstname, bool role)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Users (email, password, name, firstname, role)
                        VALUES (@Email, SHA2(@Password, 256), @Name, @Firstname, @Role);";

                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Firstname", firstname);
                    cmd.Parameters.AddWithValue("@Role", role);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating user: {ex.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="userToUpdate">The <see cref="User"/> object with updated information.</param>
        /// <returns>True if update is successful; otherwise, false.</returns>
        public bool UpdateUser(User userToUpdate)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE Users
                        SET name = @Name, firstname = @Firstname, email = @Email, role = @Role
                        WHERE id_user = @Id;";

                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", userToUpdate.Name);
                    cmd.Parameters.AddWithValue("@Firstname", userToUpdate.FirstName);
                    cmd.Parameters.AddWithValue("@Email", userToUpdate.Email);
                    cmd.Parameters.AddWithValue("@Role", userToUpdate.Role);
                    cmd.Parameters.AddWithValue("@Id", userToUpdate.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating user: {ex.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if deletion is successful; otherwise, false.</returns>
        public bool DeleteUser(int userId)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Users WHERE id_user = @Id;";
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}", "SQL Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
