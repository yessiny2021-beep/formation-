using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    public enum StatutInscription
    {
        EnAttente,
        Approuvee,
        Refusee,
        Terminee
    }

    public class Inscription
    {
        public int Id { get; set; }

        [Required]
        public int FormationId { get; set; }

        [Required]
        public int EmployeId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateInscription { get; set; } = DateTime.UtcNow;

        public StatutInscription Statut { get; set; } = StatutInscription.EnAttente;

        public bool Presence { get; set; } = false;

        public bool CertificatEmis { get; set; } = false;

        public string? CertificatNumero { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateDelivrance { get; set; }

        // Navigation properties
        public Formation? Formation { get; set; }
        public Employe? Employe { get; set; }
    }
}
