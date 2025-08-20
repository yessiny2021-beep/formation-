using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Documents;
using MvcMovie.Models;
using QuestPDF.Fluent;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    public class InscriptionController : Controller
    {
        private readonly MvcMovieContext _context;

        public InscriptionController(MvcMovieContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewBag.Employes = _context.Employes.ToList();
            ViewBag.Formations = _context.Formations.ToList();
            return View();
        }

        // POST: Inscriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscription inscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inscription);
        }

        // GET: Inscriptions/Certificate/5
        public async Task<IActionResult> Certificate(int id)
        {
            var inscription = await _context.Inscriptions
                .Include(i => i.Employe)
                .Include(i => i.Formation)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscription == null) return NotFound();

            var doc = new CertificateDocument(inscription);
            var pdf = doc.GeneratePdf();

            return File(pdf, "application/pdf", "Certificat.pdf");
        }
    }
}
