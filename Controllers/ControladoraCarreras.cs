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

        // GET: ControladoraCarreras/Listado
        public ActionResult Listado()
        {
            List<Carrera> listaCarreras = _context.Carreras.ToList();
            ViewBag.Carreras = listaCarreras;
            return View("Listado");
        }

        // GET: ControladoraCarreras/Detalles/5
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Carrera? carreraDetalles = _context.Carreras
                .FirstOrDefault(c => c.IdCarrera == id);

            if (carreraDetalles == null)
            {
                return NotFound();
            }

            return View(carreraDetalles);
        }

        // GET: ControladoraCarreras/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // POST: ControladoraCarreras/Crear
        [HttpPost]
        public ActionResult Crear(string nombre, string ciudad, DateOnly fechaDeInicio)
        {
            Carrera nuevaCarrera = new Carrera
            {
                Nombre = nombre,
                Ciudad = ciudad,
                fecha = fechaDeInicio
            };

            if (nuevaCarrera.Validacion())
            {
                _context.Carreras.Add(nuevaCarrera);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }

            ViewBag.Error = "Los datos ingresados no son válidos.";
            ViewBag.Nombre = nombre;
            ViewBag.Ciudad = ciudad;
            ViewBag.FechaDeInicio = fechaDeInicio;

            return View("Crear");
        }

        // GET: ControladoraCarreras/Editar/5
        public IActionResult Editar(int id)
        {
            Carrera? carrera = _context.Carreras.Find(id);

            if (carrera == null)
            {
                return NotFound();
            }

            ViewBag.Carrera = carrera;
            return View(carrera);
        }

        // POST: ControladoraCarreras/Editar/5
        [HttpPost]
        public IActionResult Editar(int id, string nombre, string ciudad, DateOnly fechaDeInicio)
        {
            Carrera? carrera = _context.Carreras.Find(id);

            if (carrera == null)
            {
                return NotFound();
            }
           
            carrera.Nombre = nombre;
            carrera.Ciudad = ciudad;
            carrera.fecha = fechaDeInicio;

            if (carrera.Validacion())
            {
                _context.Carreras.Update(carrera);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }

            ViewBag.Error = "Los datos ingresados no son válidos.";
            return View("Editar");
        }

        // POST: ControladoraCarreras/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            Carrera? carrera = _context.Carreras.Find(id);

            if (carrera == null)
            {
                return NotFound();
            }

            _context.Carreras.Remove(carrera);
            _context.SaveChanges();
            return RedirectToAction("Listado");
        }
    }
}
