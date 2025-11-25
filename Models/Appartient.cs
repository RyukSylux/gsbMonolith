using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Represents the relationship between a prescription and a medicine,
    /// defining which medicine belongs to which prescription and in what quantity.
    /// </summary>
    public class Appartient
    {
        /// <summary>
        /// Gets or sets the identifier of the associated medicine.
        /// </summary>
        public int Id_medicine { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated prescription.
        /// </summary>
        public int Id_prescription { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the medicine included in the prescription.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appartient"/> class.
        /// </summary>
        public Appartient() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appartient"/> class
        /// with predefined values.
        /// </summary>
        /// <param name="id_medicine">The identifier of the medicine.</param>
        /// <param name="id_prescription">The identifier of the prescription.</param>
        /// <param name="quantity">The quantity of the medicine in the prescription.</param>
        public Appartient(int id_medicine, int id_prescription, int quantity)
        {
            Id_medicine = id_medicine;
            Id_prescription = id_prescription;
            Quantity = quantity;
        }
    }
}
