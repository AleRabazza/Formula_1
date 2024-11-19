using Formula_1.Data;
using Formula_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Formula_1.Controllers
{
    public class ControladoraEscuderias : Controller
    {
        private readonly AppDbContext _context;

        public ControladoraEscuderias(AppDbContext context)
        {
            _context = context;
        }

        // GET: ControladoraEscuderias
        public ActionResult Listado()
        {
            List<Escuderia> ListaEsc = _context.Escuderia.ToList();
            return View(ListaEsc);
        }

        // GET: ControladoraEscuderias/Details/5
        public ActionResult Details(int? IdEscuderia)
        {
            if (IdEscuderia == null)
            {
                return NotFound();
            }
            Escuderia? escuderiaDetalles = _context.Escuderia
               .Include(e => e.IdEscuderia)
               .FirstOrDefault(m => m.IdEscuderia == IdEscuderia);
            if (escuderiaDetalles == null)
            {
                return NotFound();
            }

            return View(escuderiaDetalles);
        }

        // GET: ControladoraEscuderias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControladoraEscuderias/Create
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

        // GET: ControladoraEscuderias/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControladoraEscuderias/Edit/5
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

        // GET: ControladoraEscuderias/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControladoraEscuderias/Delete/5
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
