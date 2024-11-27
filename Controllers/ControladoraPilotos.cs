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

            Escuderia? esc1 = _context.Escuderia.FirstOrDefault(esc => esc.IdEscuderia == Escuderia);

            if (esc1 == null)
            {
                ViewBag.Error = "La escudería seleccionada no existe.";
                return View("Crear");
            }

            Piloto piloto = new Piloto(numero, nombre, FechaNac, PaisOrg, Escuderia, esc1);

            if (!piloto.Validacion())
            {
                ViewBag.Nombre = nombre;
                ViewBag.Numero = numero;
                ViewBag.FechaNac = FechaNac;
                ViewBag.PaisOrg = PaisOrg;
                ViewBag.Error = "Ingrese todos los datos correctamente.";
                return View("Crear");
            }

            
            _context.Pilotos.Add(piloto);
            esc1.CantidadDePilotos++;
            _context.SaveChanges();
            
            List<Piloto> ListaPilotos = _context.Pilotos
                .Include(p => p.Escuderia)
                .ToList();
            ViewBag.Pilotos = ListaPilotos;

            return View("Listado");
        }


        // GET: ControladoraPilotos/Edit/5
        public IActionResult Editar(int id)
        {
            Piloto? piloto = _context.Pilotos
                .Include(p => p.Escuderia) 
                .FirstOrDefault(p => p.NumeroPiloto == id);

            if (piloto == null)
            {
                return NotFound();
            }

            
            ViewBag.Piloto = piloto;
            ViewBag.Escuderias = _context.Escuderia
                .Where(e => e.CantidadDePilotos < 2 || e.IdEscuderia == piloto.Escuderia.IdEscuderia) 
                .ToList();

            ViewBag.Nombre = piloto.NombrePiloto;
            ViewBag.Numero = piloto.NumeroAuto;
            ViewBag.FechaNac = piloto.FechaNac;
            ViewBag.PaisOrg = piloto.PaisDeOrigen;
            ViewBag.Escuderia = piloto.Escuderia.IdEscuderia;

            return View("Editar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, string nombre, int numero, DateOnly FechaNac, string PaisOrg, int Escuderia)
        {
            Piloto? piloto = _context.Pilotos
                .Include(p => p.Escuderia)
                .FirstOrDefault(p => p.NumeroPiloto == id);

            if (piloto == null)
            {
                return NotFound();
            }

            Escuderia? nuevaEscuderia = _context.Escuderia
                .FirstOrDefault(e => e.IdEscuderia == Escuderia);

            
            piloto.NombrePiloto = nombre;
            piloto.NumeroAuto = numero;
            piloto.FechaNac = FechaNac;
            piloto.PaisDeOrigen = PaisOrg;

            
            if (piloto.Escuderia.IdEscuderia != Escuderia)
            {
                piloto.Escuderia.CantidadDePilotos--; 
                nuevaEscuderia.CantidadDePilotos++; 
                piloto.Escuderia = nuevaEscuderia; 
            }

            if (piloto.Validacion())
            {
                _context.Pilotos.Update(piloto);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }

            // En caso de error, regresar los datos a la vista
            ViewBag.Nombre = nombre;
            ViewBag.Numero = numero;
            ViewBag.FechaNac = FechaNac;
            ViewBag.PaisOrg = PaisOrg;
            ViewBag.Escuderia = Escuderia;
            ViewBag.Error = "Por favor, complete todos los datos correctamente.";

            return View("Editar");
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

