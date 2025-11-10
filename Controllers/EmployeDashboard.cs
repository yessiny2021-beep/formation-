using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    [Authorize(Roles = "Employe")]
    public class EmployeDashboard : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeDashboard(MvcMovieContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 📌 Page principale (liste des inscriptions)
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.EmployeId == null)
                return Unauthorized();

            var inscriptions = await _context.Inscriptions
                .Include(i => i.Formation)
                .Include(i => i.Employe)
                .Where(i => i.EmployeId == user.EmployeId)
                .ToListAsync();

            return View(inscriptions);
        }

        // 📌 Détails d’une inscription
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.EmployeId == null)
                return Unauthorized();

            var inscription = await _context.Inscriptions
                .Include(i => i.Formation)
                .Include(i => i.Employe)
                .FirstOrDefaultAsync(i => i.Id == id && i.EmployeId == user.EmployeId);

            if (inscription == null)
                return NotFound();

            return View(inscription);
        }
    }
}