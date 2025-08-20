using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MvcMovie.Models
{




    public class Formation
    {
        public int Id { get; set; }

        [Required]
        public string Titre { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        public int Capacite { get; set; }
        public ICollection<Inscription>? Inscriptions { get; set; }
    }
}
