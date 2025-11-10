using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public enum StatutFormation
    {
        Active,
        Inactive,
        Terminee
    }

    public class Formation : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est obligatoire")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La date de fin est obligatoire")]
        public DateTime DateFin { get; set; }

        [Range(1, 1000, ErrorMessage = "Veuiilez Vérifier  La capacité")]
        public int Capacite { get; set; }

        [Range(1, 10000, ErrorMessage = " Veuiilez Vérifier  La durée")]
        public int Duree { get; set; }

        public StatutFormation Statut { get; set; } = StatutFormation.Active;

        public ICollection<Inscription>? Inscriptions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currentYear = DateTime.Today.Year;

            if (DateDebut < DateTime.Today)
            {
                yield return new ValidationResult(
                    "Veuiilez Vérifier la date !",
                    new[] { nameof(DateDebut) });
            }

            if (DateFin < DateDebut)
            {
                yield return new ValidationResult(
                    "La date de fin doit être supérieure ou égale à la date de début.",
                    new[] { nameof(DateFin) });
            }

            if (DateDebut.Year != currentYear || DateFin.Year != currentYear)
            {
                yield return new ValidationResult(
                    $"Veuiilez Vérifier la date !.",
                    new[] { nameof(DateDebut), nameof(DateFin) });
            }
        }
    }
}
