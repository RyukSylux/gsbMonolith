using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Represents a patient in the system, including personal information
    /// such as name, age, gender, and the associated user who manages the record.
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets or sets the unique identifier of the patient.
        /// </summary>
        public int Id_patient { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user responsible for managing this patient
        /// (usually the doctor or medical staff).
        /// </summary>
        public int Id_user { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the patient.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets the full name of the patient, combining first name and last name.
        /// </summary>
        public string FullName => $"{Firstname} {Name}";

        /// <summary>
        /// Gets or sets the gender of the patient.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        public Patient() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class with predefined values.
        /// </summary>
        /// <param name="id_patient">The unique identifier of the patient.</param>
        /// <param name="id_user">The identifier of the user associated with the patient's record.</param>
        /// <param name="name">The last name of the patient.</param>
        /// <param name="age">The age of the patient.</param>
        /// <param name="firstname">The first name of the patient.</param>
        /// <param name="gender">The gender of the patient.</param>
        public Patient(int id_patient, int id_user, string name, int age, string firstname, string gender)
        {
            Id_patient = id_patient;
            Id_user = id_user;
            Name = name;
            Age = age;
            Firstname = firstname;
            Gender = gender;
        }
    }
}
