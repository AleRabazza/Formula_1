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
            List<Carrera> listaCarreras = _context.Carreras
                                                    .Include(c => c.Resultados)
                                                    .ToList();
            ViewBag.Carreras = listaCarreras;
            return View();
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

        public IActionResult IngresoDeResultado(int id, int Piloto, int PosicionSalida, int PosicionLlegada)
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
       
            if (Piloto != null)
            {

                if (pilotos.Find(p => p.NumeroPiloto == Piloto) != null)
                {

                    Resultado res1 = new Resultado(id, Piloto, PosicionSalida, PosicionLlegada);
                    _context.Add(res1);
                    _context.SaveChanges();
                    //Agregar puntos al piloto de acuerdo a su poscion
                    //Agregar puntos a la escuderia de acuerdo a la posicion del piloto
                    //capaz que lo mejor es hacer una funcion en piloto capaz de definicion de puntaje, se le pasa la posicion y devuelve el puntaje de acuerdo a lo establecido en la letra del obligatorio

                    //! puede dar erro el piloto que supuestamente no puede ser null, pero la idea es que si se pasa piloto y posiciones, se guarde. pero en caso de ser la primera vez que se ingresa al ingresarResultado, solo se pase el id. sino habria que hacer otro IActionResult para la 2da y posteriores ingresos de resultados
                }   
                }
            }

            foreach (Resultado resultado in resultados)
            {
                pilotos.Remove(resultado.Piloto);
            }

            ViewBag.Resultados = resultados;
            ViewBag.Pilotos = pilotos;
            ViewBag.Carrera = _context.Carreras.Find(id);

            List<int> PosicionesSalida = new List<int>();
            List<int> PosicionesLlegada = new List<int>();

            for (int i = 1; i <= 20; i++)
            {
                PosicionesSalida.Add(i);
                PosicionesLlegada.Add(i);
            }

            foreach (Resultado resultado in resultados) 
            {
                PosicionesSalida.Remove(resultado.PosicionSalida);
                PosicionesLlegada.Remove(resultado.PosicionLlegada);
            }

            ViewBag.PosicionesSalida = PosicionesSalida;
            ViewBag.PosicionesLlegada = PosicionesLlegada;

            return View();
        }

        
    }
}
