using Microsoft.AspNetCore.Mvc;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Linq;

namespace MvcMovie.Controllers
{
    public class FormationController : Controller
    {
        private readonly MvcMovieContext _context;

        public FormationController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Formation
        public IActionResult Index()
        {
            var formations = _context.Formations.ToList();
            return View(formations);
        }

        // GET: Formation/Details/5
        public IActionResult Details(int id)
        {
            var formation = _context.Formations.FirstOrDefault(f => f.Id == id);
            if (formation == null) return NotFound();
            return View(formation);
        }

        // GET: Formation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Formation/Create
        [HttpPost]
        public IActionResult Create(Formation formation)
        {
            if (ModelState.IsValid)
            {
                _context.Formations.Add(formation);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(formation);
        }

        // GET: Formation/Edit/5
        public IActionResult Edit(int id)
        {
            var formation = _context.Formations.FirstOrDefault(f => f.Id == id);
            if (formation == null) return NotFound();
            return View(formation);
        }

        // POST: Formation/Edit/5
        [HttpPost]
        public IActionResult Edit(Formation formation)
        {
            if (ModelState.IsValid)
            {
                _context.Formations.Update(formation);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(formation);
        }

        // GET: Formation/Delete/5
        public IActionResult Delete(int id)
        {
            var formation = _context.Formations.FirstOrDefault(f => f.Id == id);
            if (formation == null) return NotFound();
            return View(formation);
        }

        // POST: Formation/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var formation = _context.Formations.FirstOrDefault(f => f.Id == id);
            if (formation != null)
            {
                _context.Formations.Remove(formation);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
