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
        public ActionResult Editar(int id, string nombre, string ciudad, DateOnly fechaDeInicio)
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

        public IActionResult IngresoDeResultado(int id)
        {
            // Obtener lista de pilotos con sus escuderías y resultados
            List<Piloto> pilotos = _context.Pilotos
                                           .Include(p => p.Resultados)
                                           .Include(p => p.Escuderia)
                                           .ToList();

            // Obtener lista de resultados asociados a la carrera
            List<Resultado> resultados = _context.Resultados
                                                 .Include(r => r.Carrera)
                                                 .Include(r => r.Piloto)
                                                 .Where(r => r.IdCarrera == id)
                                                 .ToList();

            
            // Remover pilotos ya asignados
            foreach (Resultado resultado in resultados)
            {
                pilotos.Remove(resultado.Piloto);
            }

            // Obtener posiciones disponibles
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

            // Pasar datos a la vista
            ViewBag.Resultados = resultados;
            ViewBag.Pilotos = pilotos;
            ViewBag.Carrera = _context.Carreras.Find(id);
            ViewBag.PosicionesSalida = PosicionesSalida;
            ViewBag.PosicionesLlegada = PosicionesLlegada;

            return View("IngresoDeResultado");
        }

        [HttpPost]
        public ActionResult GuardarResultado(int IdCarrera, int Piloto, int PosicionLlegada, int PosicionSalida)
        {
           if (_context.Carreras.Find(IdCarrera) == null)
            {
                ViewBag.Error = "Carrera no encontrada";
                return View("IngresoDeResultado");
            }

            // Crear nuevo resultado
            Resultado nuevoResultado = new Resultado
            (
                IdCarrera,
                Piloto,
                PosicionSalida,
                PosicionLlegada
            );

            // Guardar el resultado en la base de datos
            _context.Resultados.Add(nuevoResultado);
            _context.SaveChanges();

            // Asignar puntos al piloto y a su escudería
            AsignarPuntos(_context.Pilotos.Find(Piloto), PosicionLlegada);
            return View("IngresoDeResultado", IdCarrera);
        }

        // Método para asignar puntos al piloto y su escudería
        private void AsignarPuntos(Piloto piloto, int posicionLlegada)
        {
            int puntos = CalcularPuntosPorPosicion(posicionLlegada);

            // Asignar puntos al piloto
            piloto.PuntajeAcumulado += puntos;

            // Asignar puntos a la escudería
            if (piloto.Escuderia != null)
            {
                piloto.Escuderia.PuntajeAcumulado += puntos;
            }

            _context.SaveChanges();
        }

        // Método para calcular puntos según la posición de llegada
        private int CalcularPuntosPorPosicion(int posicion)
        {
            switch (posicion)
            {
                case 1: return 25;
                case 2: return 18;
                case 3: return 15;
                case 4: return 12;
                case 5: return 10;
                case 6: return 8;
                case 7: return 6;
                case 8: return 4;
                case 9: return 2;
                case 10: return 1;
                default: return 0;
            }
        }


    }
}
