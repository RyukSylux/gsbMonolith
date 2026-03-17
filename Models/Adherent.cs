using System;

namespace gsbMonolith.Models
{
    /// <summary>
    /// Représente un adhérent du Club de Tennis Lumière.
    /// </summary>
    public class Adherent
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateAdhesion { get; set; }
        public bool CotisationReglee { get; set; }

        public Adherent() { }

        public Adherent(int id, string nom, string prenom, string email, string telephone, DateTime dateAdhesion, bool cotisationReglee)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Telephone = telephone;
            DateAdhesion = dateAdhesion;
            CotisationReglee = cotisationReglee;
        }

        public override string ToString()
        {
            return $"{Prenom} {Nom}";
        }
    }
}
