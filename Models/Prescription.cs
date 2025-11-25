using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Represents a medical prescription created by a user (doctor) for a specific patient.
    /// Contains metadata such as the date and the associated identifiers.
    /// </summary>
    public class Prescription
    {
        /// <summary>
        /// Gets or sets the unique identifier of the prescription.
        /// </summary>
        public int Id_prescription { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user (typically a doctor) who created the prescription.
        /// </summary>
        public int Id_user { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the patient associated with the prescription.
        /// </summary>
        public int Id_patient { get; set; }

        /// <summary>
        /// Gets or sets the date of the prescription, stored as a string.
        /// </summary>
        /// <remarks>
        /// The date format may vary depending on input.
        /// Use <see cref="DateTime.TryParse(string, out DateTime)"/> to safely convert it.
        /// </remarks>
        public string Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prescription"/> class.
        /// </summary>
        public Prescription() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prescription"/> class with predefined values.
        /// </summary>
        /// <param name="id_prescription">The unique identifier of the prescription.</param>
        /// <param name="id_user">The identifier of the user who created the prescription.</param>
        /// <param name="id_patient">The identifier of the patient concerned by the prescription.</param>
        /// <param name="date">The date of the prescription.</param>
        public Prescription(int id_prescription, int id_user, int id_patient, string date)
        {
            Id_prescription = id_prescription;
            Id_user = id_user;
            Id_patient = id_patient;
            Date = date;
        }
    }
}
