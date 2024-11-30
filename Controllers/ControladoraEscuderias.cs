using Formula_1.Data;
using Formula_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            List<Escuderia> listaEscuderias = _context.Escuderia.ToList();
            ViewBag.Escuderias = listaEscuderias;
            ViewBag.PuedeAgregar = Validacion();

            return View("Listado");
            
        }
        private bool Validacion()
        {
            if (_context.Escuderia.Count() < 10)
            {
                return true;
            }
            return false;
        }

        public ActionResult Detalles(int? IdEscuderia)
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


        [HttpPost]
        public ActionResult Crear(string nombre, string paisDeOrigen, string sponsorPrincipal, int puntajeAcumulado)
        {
            Escuderia nuevaEscuderia = new Escuderia(nombre, paisDeOrigen, sponsorPrincipal, puntajeAcumulado);
            if (nuevaEscuderia.Validacion())
            {
                _context.Escuderia.Add(nuevaEscuderia);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }

            ViewBag.Error = "Los datos ingresados no son validos";
            ViewBag.Nombre = nombre;
            ViewBag.Pais = paisDeOrigen;
            ViewBag.SponsorPrincipal = sponsorPrincipal;
            ViewBag.Puntaje = puntajeAcumulado;


            return View("Crear");
        }

        public ActionResult Crear()
        {
            return View();
        }


        public IActionResult Editar(int id)
        {
            Escuderia escuderia = _context.Escuderia.Find(id);
            if (escuderia == null)
            {
                return NotFound();
            }

            Escuderia escuderia1 = _context.Escuderia.Find(id);
            ViewBag.Escuderia = escuderia1;

            ViewBag.Nombre = escuderia1.Nombre;
            ViewBag.PaisDeOrigen = escuderia1.PaisDeOrigen;
            ViewBag.SponsorPrincipal = escuderia1.SponsorPrincipal;

            return View(escuderia1);
        }

        [HttpPost]
        public IActionResult Editar(int id, string nombre, string paisDeOrigen, string sponsorPrincipal, int puntaje) 
        {
            Escuderia? escuderia = _context.Escuderia.Find(id);
            if (escuderia == null) {

                return View("Listado");

             }
          
            escuderia.Nombre=nombre;
            escuderia.PaisDeOrigen=paisDeOrigen;
            escuderia.SponsorPrincipal=sponsorPrincipal;
        

            if (escuderia.Validacion())
            {
                _context.Escuderia.Update(escuderia);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }
            ViewBag.Nombre = nombre;
            ViewBag.PaisOrigen = paisDeOrigen;
            ViewBag.SponsorPrincipal=sponsorPrincipal;
           


            return View("Editar");
        }

       
        public ActionResult Eliminar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Escuderia? escuderia = _context.Escuderia.Find(id);
            if (escuderia == null)
            {
                return NotFound();
            }
            _context.Escuderia.Remove(escuderia);
            _context.SaveChanges();
            return RedirectToAction("Listado");
        }

        public IActionResult PreCarga()
        {
            if (_context.Escuderia.Count() <= 0)
            {
                List<Escuderia> escuderias = new List<Escuderia>
                {
                new Escuderia("Ferrari", "Italia", "Shell", 0),
                new Escuderia("Mercedes", "Alemania", "Petronas", 0),
                new Escuderia("Red Bull Racing", "Austria", "Oracle", 0),
                new Escuderia("McLaren", "Reino Unido", "Gulf Oil", 0),
                new Escuderia("Alpine", "Francia", "BWT", 0),
                new Escuderia("Aston Martin", "Reino Unido", "Cognizant", 0),
                new Escuderia("Alfa Romeo", "Suiza", "Orlen", 0),
                new Escuderia("Haas", "Estados Unidos", "MoneyGram", 0),
                new Escuderia("Williams", "Reino Unido", "Duracell", 0),             
                };

                _context.Escuderia.AddRange(escuderias);
                _context.SaveChanges();
            }

            return RedirectToAction("Listado");
        }
    }
}
