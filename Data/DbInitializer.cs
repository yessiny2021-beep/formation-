using System;
using System.Linq;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MvcMovieContext context)
        {
            // Si la base contient déjà des données, on ne fait rien
            if (context.Formations.Any() || context.Employes.Any() || context.Inscriptions.Any())
                return;

            // Ajouter quelques formations
            var formations = new Formation[]
            {
                new Formation { Titre = "Sécurité au travail", Description = "Formation sur la sécurité", DateDebut = DateTime.Parse("2025-09-01"), DateFin = DateTime.Parse("2025-09-05") },
                new Formation { Titre = "Gestion de projet", Description = "Introduction à la gestion de projet", DateDebut = DateTime.Parse("2025-10-01"), DateFin = DateTime.Parse("2025-10-07") }
            };
            context.Formations.AddRange(formations);
            context.SaveChanges();

            // Ajouter quelques employés
            var employes = new Employe[]
            {
                new Employe { Nom = "Rekik", Prenom = "Majdi", Email = "majdi.rekik@gmail.com", Poste = "Technicien" },
                new Employe { Nom = "Yengui", Prenom = "Yessin", Email = "yessin.yengui@gmail.com", Poste = "Chef de projet" }
            };
            context.Employes.AddRange(employes);
            context.SaveChanges();
            // Ajouter quelques inscriptions
            var inscriptions = new Inscription[]
            {
                new Inscription { FormationId = formations[0].Id, EmployeId = employes[0].Id, DateInscription = DateTime.Now },
                new Inscription { FormationId = formations[1].Id, EmployeId = employes[1].Id, DateInscription = DateTime.Now }
            };
            context.Inscriptions.AddRange(inscriptions);
            context.SaveChanges();
        }
    }
}
