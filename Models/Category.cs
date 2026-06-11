using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Represents a patient category (e.g. Femme enceinte, Enfant) in the system.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        public int Id_category { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        public Category() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class with predefined values.
        /// </summary>
        /// <param name="id_category">The unique identifier of the category.</param>
        /// <param name="name">The name of the category.</param>
        public Category(int id_category, string name)
        {
            Id_category = id_category;
            Name = name;
        }

        /// <summary>
        /// Returns the name of the category.
        /// </summary>
        public override string ToString()
        {
            return Name ?? "";
        }
    }
}
