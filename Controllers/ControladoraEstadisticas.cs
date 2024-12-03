using Formula_1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Formula_1.Models;

namespace Formula_1.Controllers
{
    public class ControladoraEstadisticas : Controller
    {
        private readonly AppDbContext _context;

        public ControladoraEstadisticas(AppDbContext context)
        {
            _context = context;
        }

        // GET: ControladoraEstadisticas
        public ActionResult Estadisticas()
        {
            List<Piloto> Pilotos = _context.Pilotos
                                    .Include(p => p.Resultados)
                                    .Include(p => p.Escuderia)
                                    .ToList();

            Pilotos.Sort((p1, p2) => p2.PuntajeAcumulado.CompareTo(p1.PuntajeAcumulado));

            List<Escuderia> Escuderias = _context.Escuderia
                                                    .Include(e => e.Pilotos)
                                                    .ToList();

            Escuderias.Sort((e1, e2) => e2.PuntajeAcumulado.CompareTo(e1.PuntajeAcumulado));

            ViewBag.Pilotos = Pilotos;
            ViewBag.Escuderias = Escuderias;

            return View("Estadistica");
        }

        public IActionResult HistorialPiloto(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Piloto> pilotos = _context.Pilotos.Include(p => p.Resultados).ToList();
            Piloto piloto = pilotos.Find(p => p.NumeroPiloto == id);

            if (piloto == null)
            {
                return NotFound();
            }

            List<Resultado> resultado = new List<Resultado>(_context.Resultados.Include(r => r.Carrera).Where(r => r.IdPiloto == id));

            ViewBag.Piloto = piloto;
            ViewBag.Resultados = resultado;

            return View("HistorialPiloto");
        }

        public IActionResult HistorialEscuderia(int id)
        {
            return View("HistorialEscuderia");
        }
    }

    
}
