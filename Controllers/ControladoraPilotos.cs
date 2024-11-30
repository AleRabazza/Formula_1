using System;
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

            if (_context.Pilotos.Count() == 0)
            {
                // Lista de pilotos precargados
                List<Piloto> pilotosPrecargados = new List<Piloto>
        {
            new Piloto(1, "Max Verstappen", DateOnly.Parse("1997-09-30"), "Holanda", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Red Bull Racing")),
            new Piloto(3, "Daniel Ricciardo", DateOnly.Parse("1989-07-01"), "Australia", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Red Bull Racing")),
            new Piloto(4, "Lando Norris", DateOnly.Parse("1999-11-13"), "Reino Unido", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Ferrari")),
            new Piloto(10, "Pierre Gasly", DateOnly.Parse("1996-02-07"), "Francia", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Ferrari")),
            new Piloto(14, "Fernando Alonso", DateOnly.Parse("1981-07-29"), "España", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Mercedes")),
            new Piloto(16, "Charles Leclerc", DateOnly.Parse("1997-10-16"), "Monaco", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Mercedes")),
            new Piloto(18, "Lance Stroll", DateOnly.Parse("1998-10-29"), "Canadá", _context.Escuderia.FirstOrDefault(e => e.Nombre == "McLaren")),
            new Piloto(20, "Kevin Magnussen", DateOnly.Parse("1992-10-05"), "Dinamarca", _context.Escuderia.FirstOrDefault(e => e.Nombre == "McLaren")),
            new Piloto(22, "Yuki Tsunoda", DateOnly.Parse("2000-05-11"), "Japón", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Alpine")),
            new Piloto(23, "Alex Albon", DateOnly.Parse("1996-03-23"), "Tailandia", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Alpine")),
            new Piloto(24, "Guanyu Zhou", DateOnly.Parse("1999-05-30"), "China", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Aston Martin")),
            new Piloto(27, "Nico Hulkenberg", DateOnly.Parse("1987-08-19"), "Alemania", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Aston Martin")),
            new Piloto(30, "Liam Lawson", DateOnly.Parse("2002-02-11"), "Nueva Zelanda", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Alfa Romeo")),
            new Piloto(31, "Esteban Ocon", DateOnly.Parse("1996-09-17"), "Francia", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Alfa Romeo")),
            new Piloto(43, "Franco Colapinto", DateOnly.Parse("2003-05-22"), "Argentina", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Haas")),
            new Piloto(44, "Lewis Hamilton", DateOnly.Parse("1985-01-07"), "Reino Unido", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Haas")),
            new Piloto(55, "Carlos Sainz", DateOnly.Parse("1994-09-01"), "España", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Williams")),
            new Piloto(77, "Valtteri Bottas", DateOnly.Parse("1989-08-28"), "Finlandia", _context.Escuderia.FirstOrDefault(e => e.Nombre == "Williams"))
        };

                _context.Pilotos.AddRange(pilotosPrecargados);
                _context.SaveChanges();

                ViewBag.Pilotos = _context.Pilotos
                    .Include(piloto => piloto.Escuderia)
                    .ToList();
                ViewBag.PuedeAgregar = _context.Pilotos.Count() < 20 ? true : false;

            }

            return View("Listado");
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

            Piloto piloto = new Piloto(numero, nombre, FechaNac, PaisOrg, esc1);

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