using Formula_1.Data;
using Formula_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            return View();
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
            var escuderia = _context.Escuderia.Find(id);
            if (escuderia == null)
            {
                return NotFound();
            }
            return View(escuderia);
        }

        [HttpPost]
        public IActionResult Editar(int id, string nombre, string paisOrigen, string sponsorPrincipal, int puntaje) 
        {
            Escuderia escuderia = _context.Escuderia.Find(id);
            if (escuderia == null) {

                return NotFound();
             }

            escuderia.Nombre=nombre;
            escuderia.PaisDeOrigen=paisOrigen;
            escuderia.SponsorPrincipal=sponsorPrincipal;
            escuderia.PuntajeAcumulado= puntaje;

            if (escuderia.Validacion())
            {
                _context.Escuderia.Update(escuderia);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }
            return View("Editar");
        }

       
        public ActionResult Eliminar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Escuderia escuderia = _context.Escuderia.Find(id);
            if (escuderia == null)
            {
                return NotFound();
            }
            _context.Escuderia.Remove(escuderia);
            _context.SaveChanges();
            return RedirectToAction("Listado");
        }
    }
}
