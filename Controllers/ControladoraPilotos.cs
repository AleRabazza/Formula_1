using Formula_1.Data;
using Formula_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Formula_1.Controllers
{
    public class ControladoraPilotos : Controller
    {
        private readonly AppDbContext _context;

        public ControladoraPilotos(AppDbContext context)
        {
            _context = context;
        }


        // GET: ControladoraPilotos
        public IActionResult Listado()
        {
            List<Piloto> ListaPilotos = _context.Pilotos.ToList();
            return View(ListaPilotos);
        }

        // GET: ControladoraPilotos/Details/5
        public ActionResult Details(int? numP)
        {
            if (numP == null)
            {
                return NotFound();
            }
            Piloto? pilotoDetalles = _context.Pilotos
               .Include(p => p.NumeroPiloto)
               .FirstOrDefault(m => m.NumeroPiloto == numP);
            if (pilotoDetalles == null)
            {
                return NotFound();
            }

            return View(pilotoDetalles);
            
        }

        // GET: ControladoraPilotos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControladoraPilotos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControladoraPilotos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControladoraPilotos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControladoraPilotos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControladoraPilotos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
