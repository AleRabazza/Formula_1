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
        public ActionResult detalles(int? numP)
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
        public IActionResult Crear()
        {
            ViewBag.Escuderias = _context.Escuderia.ToList();
            foreach (Escuderia escuderia in ViewBag.Escuderias)
            {
                if (escuderia.CantidadDePilotos == 2)
                {
                    ViewBag.Escuderias.Remove(escuderia);
                }
            }
            
            return View();
        }

        // POST: ControladoraPilotos/Create
        [HttpPost]
        public IActionResult Crear(string nombre, int numero, DateOnly FechaNac, string PaisOrg, int Escuderia)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                ViewBag.Error = "Datos invalidos";
                return View("Crear");
            }

            List<Escuderia> ListaEsc = _context.Escuderia.ToList();
            Escuderia? esc1;

            if ((ListaEsc.Find(esc => esc.IdEscuderia == Escuderia) == null))
            {
                ListaEsc = _context.Escuderia.ToList();
                esc1 = ListaEsc.Find(esc => esc.IdEscuderia == Escuderia);
            }else
            {
                return View("Crear");
            }

            Piloto piloto = new Piloto(nombre, numero, FechaNac, PaisOrg, Escuderia); 
            _context.Pilotos.Add(piloto);
            _context.SaveChanges();

            return View("Listado");
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
