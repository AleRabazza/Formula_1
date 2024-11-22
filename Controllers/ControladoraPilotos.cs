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
            List<Piloto> ListaPilotos = _context.Pilotos
                .Include(piloto => piloto.Escuderia)                
                .ToList();
            ViewBag.Pilotos = ListaPilotos;
            return View();
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
            List<Escuderia> escuderiaLista = _context.Escuderia.Where(esc => esc.CantidadDePilotos < 2).ToList();
            ViewBag.Escuderias = escuderiaLista;
           
            return View();
        }

        // POST: ControladoraPilotos/Create
        [HttpPost]
        public IActionResult Crear(string nombre, int numero, DateOnly FechaNac, string PaisOrg, int Escuderia)
        {
            List<Escuderia> escuderiaLista = _context.Escuderia.Where(esc => esc.CantidadDePilotos < 2).ToList();
            ViewBag.Escuderias = escuderiaLista;

            if (string.IsNullOrEmpty(nombre))
            {
                ViewBag.Error = "Datos invalidos";
                Console.WriteLine("Nombre mal");
                return View("Crear");
            }

            List<Escuderia> ListaEsc = _context.Escuderia.ToList();

            if ((ListaEsc.Find(esc => esc.IdEscuderia == Escuderia) == null))
            {
                Console.WriteLine("Mal escuderia");
                return View("Crear");

            }

            Escuderia? esc1 = ListaEsc.Find(esc => esc.IdEscuderia == Escuderia);

            Piloto piloto = new Piloto(numero, nombre, FechaNac, PaisOrg, Escuderia, esc1); 
            _context.Pilotos.Add(piloto);
            _context.SaveChanges();
            Console.WriteLine("Agreado");

            List<Piloto> ListaPilotos = _context.Pilotos
                .Include(piloto => piloto.Escuderia)
                .ToList();
            ViewBag.Pilotos = ListaPilotos;

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
