using Microsoft.AspNetCore.Mvc;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    public class EmployeController : Controller
    {
        private readonly MvcMovieContext _context;

        public EmployeController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Employe
        public IActionResult Index()
        {
            var employes = _context.Employes.ToList();
            return View(employes);
        }

        // GET: Employe/Details/5
        public IActionResult Details(int id)
        {
            var employe = _context.Employes.FirstOrDefault(e => e.Id == id);
            if (employe == null) return NotFound();
            return View(employe);
        }

        // GET: Employe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employe employe)
        {
            if (ModelState.IsValid)
            {
                _context.Employes.Add(employe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employe);
        }

        // GET: Employe/Edit/5
        public IActionResult Edit(int id)
        {
            var employe = _context.Employes.Find(id);
            if (employe == null) return NotFound();
            return View(employe);
        }

        // POST: Employe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employe employe)
        {
            if (id != employe.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(employe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employe);
        }

        // GET: Employe/Delete/5
        public IActionResult Delete(int id)
        {
            var employe = _context.Employes.Find(id);
            if (employe == null) return NotFound();
            return View(employe);
        }

        // POST: Employe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
{
    var employe = _context.Employes.Find(id);
    if (employe != null)
    {
        _context.Employes.Remove(employe);
        _context.SaveChanges();
    }
    return RedirectToAction(nameof(Index));
}

          
    }
}
