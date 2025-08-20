using System;
using System.ComponentModel.DataAnnotations;


namespace MvcMovie.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string? Poste { get; set; }
        public ICollection<Inscription>? Inscriptions { get; set; }
    }
}
