using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovie.Data;
using MvcMovie.Documents;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;   // ← OBLIGATOIRE pour avoir .GeneratePdf()

namespace MvcMovie.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MvcMovieContext _context;

        public DashboardController(MvcMovieContext context)
        {
            _context = context;
        }

        // Tableau global des formations
        public async Task<IActionResult> Index()
        {
            var formations = await _context.Formations
                .Include(f => f.Inscriptions)
                    .ThenInclude(i => i.Employe)
                .ToListAsync();

            return View(formations);
        }

        // Détail d’une formation et ses inscriptions
        public async Task<IActionResult> Details(int id)
        {
            var formation = await _context.Formations
                .Include(f => f.Inscriptions)
                    .ThenInclude(i => i.Employe)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }

        [HttpGet]
        public IActionResult GenerateCertificate(int inscriptionId)
        {
            var inscription = _context.Inscriptions
                .Include(i => i.Employe)
                .Include(i => i.Formation)
                .FirstOrDefault(i => i.Id == inscriptionId);

            if (inscription == null)
                return NotFound();

            var document = new CertificateDocument(inscription);
            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", $"Certificat_{inscription.Employe.Nom}.pdf");
        }
    }
}
