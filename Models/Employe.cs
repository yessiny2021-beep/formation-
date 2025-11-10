using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcMovie.Data;

namespace MvcMovie.Models
{
    public class Employe : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public string Prenom { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateEmbauche { get; set; }

        public ICollection<Inscription>? Inscriptions { get; set; }

        // Validation côté serveur
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 1️⃣ Vérification de la date d'embauche
            if (DateEmbauche > DateTime.Today)
            {
                yield return new ValidationResult(
                    "La date d'embauche ne peut pas dépasser aujourd'hui.",
                    new[] { nameof(DateEmbauche) });
            }

            // 2️⃣ Vérification email unique
            if (validationContext.GetService(typeof(MvcMovieContext)) is MvcMovieContext db)
            {
                var emailNormalized = Email.Trim().ToLower();
                var exists = db.Employes.Any(e => e.Email.ToLower() == emailNormalized && e.Id != Id);
                if (exists)
                {
                    yield return new ValidationResult(
                        "Cet email existe déjà.",
                        new[] { nameof(Email) });
                }
            }
        }
    }
}
