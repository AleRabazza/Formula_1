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

        public IActionResult IngresoDeResultado(int id, int? Piloto, int? PosicionSalida, int? PosicionLlegada)
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

            // Validar si se están ingresando datos de piloto y posiciones
            if (Piloto.HasValue && PosicionSalida.HasValue && PosicionLlegada.HasValue)
            {
                Piloto? pilotoSeleccionado = pilotos.FirstOrDefault(p => p.NumeroPiloto == Piloto.Value);

                if (pilotoSeleccionado != null)
                {
                    // Crear nuevo resultado
                    Resultado nuevoResultado = new Resultado
                    {
                        IdCarrera = id,
                        Piloto = pilotoSeleccionado,
                        PosicionSalida = PosicionSalida.Value,
                        PosicionLlegada = PosicionLlegada.Value
                    };

                    // Guardar el resultado en la base de datos
                    _context.Resultados.Add(nuevoResultado);
                    Console.WriteLine("Guardado");
                    _context.SaveChanges();

                    // Asignar puntos al piloto y a su escudería
                    AsignarPuntos(pilotoSeleccionado, PosicionLlegada.Value);
                }
            }

            // Remover pilotos ya asignados
            foreach (Resultado resultado in resultados)
            {
                pilotos.Remove(resultado.Piloto);
            }

            // Obtener posiciones disponibles
            List<int> PosicionesSalida = Enumerable.Range(1, 20).Except(resultados.Select(r => r.PosicionSalida)).ToList();
            List<int> PosicionesLlegada = Enumerable.Range(1, 20).Except(resultados.Select(r => r.PosicionLlegada)).ToList();

            // Pasar datos a la vista
            ViewBag.Resultados = resultados;
            ViewBag.Pilotos = pilotos;
            ViewBag.Carrera = _context.Carreras.Find(id);
            ViewBag.PosicionesSalida = PosicionesSalida;
            ViewBag.PosicionesLlegada = PosicionesLlegada;

            return View("IngresoDeResultado");
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
