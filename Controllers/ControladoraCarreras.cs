using Formula_1.Data;
using Formula_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Formula_1.Controllers
{
    public class ControladoraCarreras : Controller
    {
        private readonly AppDbContext _context;

        public ControladoraCarreras(AppDbContext context)
        {
            _context = context;
        }
        // GET: ControladoraCarreras
        public ActionResult Listado()
        {

            return View();
        }

        // GET: ControladoraCarreras/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ControladoraCarreras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControladoraCarreras/Create
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

        // GET: ControladoraCarreras/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControladoraCarreras/Edit/5
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

        // GET: ControladoraCarreras/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControladoraCarreras/Delete/5
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

        public IActionResult AgregarResultado(int id)
        {
            List<Piloto> pilotos = _context.Pilotos
                                        .Include(p => p.Resultados)
                                        .Include(p => p.Escuderia)
                                        .ToList();

            List<Resultado> resultados = _context.Resultados
                                                .Include(r => r.Carrera)
                                                .Where(r => r.IdCarrera == id)
                                                .Include(r => r.Piloto)
                                                .ToList();

            foreach (Resultado resultado in resultados)
            {
                pilotos.Remove(resultado.Piloto);
            }

            ViewBag.Resultados = resultados;
            ViewBag.Pilotos = pilotos;
            ViewBag.Carrera = _context.Carreras.Find(id);

            return View();
        }
    }
}
