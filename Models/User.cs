using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Represents a user of the system, typically a doctor or administrator.
    /// Contains identification, credentials, and role information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <remarks>
        /// This value should ideally be stored hashed and salted,
        /// not in plaintext.
        /// </remarks>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        /// <remarks>
        /// The role is stored as a boolean.
        /// You may interpret this value as:
        /// <c>true</c> = admin / doctor, <c>false</c> = regular user,
        /// depending on your business logic.
        /// </remarks>
        public bool Role { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with predefined values,
        /// except for the password which must be assigned separately.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="name">The last name of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="role">The role associated with the user.</param>
        /// <param name="email">The email address of the user.</param>
        public User(int id, string name, string firstName, bool role, string email)
        {
            Id = id;
            Name = name;
            Email = email;
            FirstName = firstName;
            Role = role;
        }
    }
}
