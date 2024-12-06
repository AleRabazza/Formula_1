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
            if (_context.Pilotos.Count() == 0)
            {
                Escuderia RedBull = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Red Bull Racing");
                Escuderia Ferrari = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Ferrari");
                Escuderia Mercedes = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Mercedes");
                Escuderia McLaren = _context.Escuderia.FirstOrDefault(e => e.Nombre == "McLaren");
                Escuderia Alpine = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Alpine");
                Escuderia AstonMartin = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Aston Martin");
                Escuderia AlfaRomeo = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Alfa Romeo");
                Escuderia Williams = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Williams");
                Escuderia Haas = _context.Escuderia.FirstOrDefault(e => e.Nombre == "Haas");



                // Lista de pilotos precargados
                List<Piloto> pilotosPrecargados = new List<Piloto>
                {
                    new Piloto(1, "Max Verstappen", DateOnly.Parse("1997-09-30"), "Holanda", RedBull),
                    new Piloto(3, "Daniel Ricciardo", DateOnly.Parse("1989-07-01"), "Australia", RedBull),
                    new Piloto(4, "Lando Norris", DateOnly.Parse("1999-11-13"), "Reino Unido", McLaren),
                    new Piloto(10, "Pierre Gasly", DateOnly.Parse("1996-02-07"), "Francia", Alpine),
                    new Piloto(14, "Fernando Alonso", DateOnly.Parse("1981-07-29"), "España", AstonMartin),
                    new Piloto(16, "Charles Leclerc", DateOnly.Parse("1997-10-16"), "Monaco", Ferrari),
                    new Piloto(18, "Lance Stroll", DateOnly.Parse("1998-10-29"), "Canadá", AstonMartin),
                    new Piloto(20, "Kevin Magnussen", DateOnly.Parse("1992-10-05"), "Dinamarca", Haas),
                    new Piloto(22, "Yuki Tsunoda", DateOnly.Parse("2000-05-11"), "Japón", Mercedes),
                    new Piloto(23, "Alex Albon", DateOnly.Parse("1996-03-23"), "Tailandia", Williams),
                    new Piloto(24, "Guanyu Zhou", DateOnly.Parse("1999-05-30"), "China", AlfaRomeo),
                    new Piloto(27, "Nico Hulkenberg", DateOnly.Parse("1987-08-19"), "Alemania", Haas),
                    new Piloto(30, "Liam Lawson", DateOnly.Parse("2002-02-11"), "Nueva Zelanda", McLaren),
                    new Piloto(31, "Esteban Ocon", DateOnly.Parse("1996-09-17"), "Francia", Alpine),
                    new Piloto(43, "Franco Colapinto", DateOnly.Parse("2003-05-22"), "Argentina", Williams),
                    new Piloto(44, "Lewis Hamilton", DateOnly.Parse("1985-01-07"), "Reino Unido", Mercedes),
                    new Piloto(55, "Carlos Sainz", DateOnly.Parse("1994-09-01"), "España", Ferrari),
                    new Piloto(77, "Valtteri Bottas", DateOnly.Parse("1989-08-28"), "Finlandia", AlfaRomeo)
                };


                _context.Pilotos.AddRange(pilotosPrecargados);
                _context.SaveChanges();
            }

            ViewBag.Pilotos = _context.Pilotos
                    .Include(piloto => piloto.Escuderia)
                    .ToList();

            return View();
        }
    

        // GET: ControladoraPilotos/Create
        public IActionResult Crear()
        {
            List<Escuderia> escuderiaLista = _context.Escuderia.Where(esc => esc.CantidadDePilotos < 2)
                                                .Include(e => e.Pilotos)
                                                .ToList();
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
                .Where(e => e.CantidadDePilotos < 2 && e.IdEscuderia != piloto.Escuderia.IdEscuderia)
                .ToList();

            ViewBag.Nombre = piloto.NombrePiloto;
            ViewBag.Numero = piloto.NumeroAuto;
            ViewBag.FechaNac = piloto.FechaNac;
            ViewBag.PaisOrg = piloto.PaisDeOrigen;
            ViewBag.Escuderia = piloto.Escuderia.IdEscuderia;

            return View("Editar");
        }

        [HttpPost]
        public ActionResult Editar(int id, string nombre, int numero, DateOnly FechaNac, string PaisOrg, int Escuderia)
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


            

            if (piloto.Validacion())
            {
                if (piloto.Escuderia.IdEscuderia != Escuderia)
                {
                    piloto.Escuderia.CantidadDePilotos--;
                    _context.Escuderia.Update(piloto.Escuderia);
                    nuevaEscuderia.CantidadDePilotos++;
                    _context.Escuderia.Update(piloto.Escuderia);
                    piloto.Escuderia = nuevaEscuderia;
                }
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

            Escuderia escuderia = _context.Escuderia.Find(piloto.EscuderiaId);
            if (escuderia != null && escuderia.CantidadDePilotos > 0) 
            {
                escuderia.CantidadDePilotos--;
                _context.Escuderia.Update(escuderia); 
            }


            _context.Pilotos.Remove(piloto);
            _context.SaveChanges();
            return RedirectToAction("Listado");

        }
    }

}