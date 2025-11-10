using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Documents;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuestPDF.Fluent;

namespace MvcMovie.Controllers
{
    [Authorize(Roles = "Employe,Admin")]
    public class InscriptionController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InscriptionController(MvcMovieContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Inscriptions
        public async Task<IActionResult> Index()
        {
            var inscriptions = await _context.Inscriptions
                .Include(i => i.Employe)
                .Include(i => i.Formation)
                .ToListAsync();

            return View(inscriptions);
        }

        // GET: Inscriptions/Create
        public async Task<IActionResult> Create()
        {
            await ChargerViewBags();
            return View();
        }

        // POST: Inscriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscription inscription)
        {
            if (!ModelState.IsValid)
            {
                await ChargerViewBags();
                return View(inscription);
            }

            if (User.IsInRole("Employe"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null || user.EmployeId == null)
                {
                    ModelState.AddModelError("", "Impossible de récupérer l'employé connecté.");
                    await ChargerViewBags();
                    return View(inscription);
                }

                inscription.EmployeId = user.EmployeId.Value;
            }

            inscription.DateInscription = System.DateTime.Now;

            // Vérification doublon
            var dejaInscrit = await _context.Inscriptions
                .AnyAsync(i => i.EmployeId == inscription.EmployeId 
                            && i.FormationId == inscription.FormationId);

            if (dejaInscrit)
            {
                ModelState.AddModelError("", "⚠ Vous êtes déjà inscrit à cette formation.");
                await ChargerViewBags();
                return View(inscription);
            }

            _context.Add(inscription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Inscriptions/Certificate/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Certificate(int id)
        {
            var inscription = await _context.Inscriptions
                .Include(i => i.Employe)
                .Include(i => i.Formation)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscription == null)
                return NotFound();

            // Générer PDF via QuestPDF
            var doc = new CertificateDocument(inscription);
            var pdfBytes = Document.Create(container => doc.Compose(container)).GeneratePdf();

            return File(pdfBytes, "application/pdf", "Certificat.pdf");
        }

        // POST: Inscriptions/ChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int id, StatutInscription statut)
        {
            var inscription = await _context.Inscriptions.FindAsync(id);
            if (inscription == null)
                return NotFound();

            inscription.Statut = statut;
            _context.Update(inscription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Méthode utilitaire pour charger les ViewBag
        private async Task ChargerViewBags()
        {
            ViewBag.Formations = await _context.Formations.ToListAsync();

            if (User.IsInRole("Admin"))
            {
                ViewBag.Employes = await _context.Employes.ToListAsync();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                if (user?.EmployeId != null)
                {
                    var employe = await _context.Employes.FindAsync(user.EmployeId.Value);
                    ViewBag.EmployeId = employe?.Id;
                    ViewBag.EmployeNom = employe != null ? $"{employe.Nom} {employe.Prenom}" : "";
                }
            }
        }
    }
}
