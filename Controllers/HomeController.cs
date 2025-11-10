using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using Microsoft.AspNetCore.Authorization;

namespace MvcMovie.Controllers
{
    [Authorize(Roles = "Admin,Employe")]
    public class HomeController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(MvcMovieContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Récupérer les formations récentes (dernier mois)
            var recentFormations = await _context.Formations
                .Where(f => f.DateDebut >= DateTime.Today.AddMonths(-1))
                .OrderBy(f => f.DateDebut)
                .Take(6)
                .ToListAsync();

            // Récupérer les formations populaires (plus d'inscriptions)
            var popularFormations = await _context.Formations
                .Include(f => f.Inscriptions)
                .OrderByDescending(f => f.Inscriptions.Count)
                .Take(6)
                .ToListAsync();

            ViewBag.RecentFormations = recentFormations;
            ViewBag.PopularFormations = popularFormations;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                // Si Admin : statistiques globales + calendrier
                if (User.IsInRole("Admin"))
                {
                    ViewBag.TotalEmployes = await _context.Employes.CountAsync();
                    ViewBag.TotalFormations = await _context.Formations.CountAsync();
                    ViewBag.InscriptionsEnAttente = await _context.Inscriptions
                        .CountAsync(i => i.Statut == StatutInscription.EnAttente);

                    // ✅ Toutes les formations avec inscriptions pour le calendrier
                    ViewBag.AllFormations = await _context.Formations
                        .Include(f => f.Inscriptions)
                        .OrderBy(f => f.DateDebut)
                        .ToListAsync();
                }

                if (user != null && user.EmployeId.HasValue)
                {
                    // Récupérer l'employé lié pour afficher son nom
                    var employe = await _context.Employes
                        .FirstOrDefaultAsync(e => e.Id == user.EmployeId.Value);

                    if (employe != null)
                        ViewBag.UserFullName = $"{employe.Nom} {employe.Prenom}";

                    // Si Employé : ses formations + prochaines
                    if (User.IsInRole("Employe"))
                    {
                        ViewBag.MesFormationsCount = await _context.Inscriptions
                            .CountAsync(i => i.EmployeId == user.EmployeId.Value);

                        ViewBag.ProchainesFormations = await _context.Formations
                            .Where(f => f.DateDebut >= DateTime.Today)
                            .OrderBy(f => f.DateDebut)
                            .Take(3)
                            .ToListAsync();
                    }
                }
                else if (User.IsInRole("Admin"))
                {
                    // Si Admin sans EmployeId → utiliser le nom du compte
                    ViewBag.UserFullName = User.Identity.Name;
                }
            }

            return View();
        }
    }
}
