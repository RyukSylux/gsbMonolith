using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Represents a medicine stored in the system, including its dosage, description,
    /// active molecule, and the user (typically a doctor or administrator) who created or manages it.
    /// </summary>
    public class Medicine
    {
        /// <summary>
        /// Gets or sets the unique identifier of the medicine.
        /// </summary>
        public int Id_medicine { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user associated with the creation or management of the medicine.
        /// </summary>
        public int Id_user { get; set; }

        /// <summary>
        /// Gets or sets the recommended dosage of the medicine.
        /// </summary>
        public string Dosage { get; set; }

        /// <summary>
        /// Gets or sets the commercial name of the medicine.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a short description of the medicine, such as indications or general information.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the active molecule contained in the medicine.
        /// </summary>
        public string Molecule { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Medicine"/> class.
        /// </summary>
        public Medicine() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Medicine"/> class with predefined values.
        /// </summary>
        /// <param name="id_medicine">The unique identifier of the medicine.</param>
        /// <param name="id_user">The identifier of the associated user.</param>
        /// <param name="dosage">The recommended dosage of the medicine.</param>
        /// <param name="name">The commercial name of the medicine.</param>
        /// <param name="description">A short description of the medicine.</param>
        /// <param name="molecule">The active molecule contained in the medicine.</param>
        public Medicine(int id_medicine, int id_user, string dosage, string name, string description, string molecule)
        {
            Id_medicine = id_medicine;
            Id_user = id_user;
            Dosage = dosage;
            Name = name;
            Description = description;
            Molecule = molecule;
        }
    }
}
