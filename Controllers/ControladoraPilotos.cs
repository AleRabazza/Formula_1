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

            List<Escuderia> ListaEsc = _context.Escuderia.ToList();           

            Escuderia? esc1 = ListaEsc.Find(esc => esc.IdEscuderia == Escuderia);

            Piloto piloto = new Piloto(numero, nombre, FechaNac, PaisOrg, Escuderia, esc1);
            if (!piloto.Validacion())
            {
                ViewBag.Nombre = nombre;
                ViewBag.Numero = numero;
                ViewBag.FechaNac = FechaNac;
                ViewBag.PaisOrg = PaisOrg;
                ViewBag.Error = "Ingrese todos los datos";
                return View("Crear");
            }
            
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
        public ActionResult Editar(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Piloto piloto = _context.Pilotos.Find(id);

            if (piloto == null)
            {
                return NotFound();
            }

            ViewBag.Piloto = piloto;
            return View("Editar");
        }

        // POST: ControladoraPilotos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string nombre, int numero, DateOnly FechaNac, string PaisOrg, int Escuderia)
        {
            return View();
        }


        
        public ActionResult Eliminar(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Piloto piloto = _context.Pilotos.Find(id);
            if (piloto == null)
            {
                return NotFound();
            }

            _context.Pilotos.Remove(piloto);
            _context.SaveChanges();
            return RedirectToAction("Listado");

        }
    }

}

