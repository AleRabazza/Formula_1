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


        public ActionResult PreCargaPilotos()
        {
            if (_context.Pilotos.ToList().Count != 0)
            {
                ViewBag.Error = "Ya existen datos cargados";
                return View("Listado");
            }

            if (_context.Escuderia.ToList().Count < 18)
            {
                ViewBag.Error = "Debe realizar la precarga de Escuderias primero";
                return View("Listado");
            }



            Piloto piloto1 = new Piloto(1, "Max Verstappen", DateOnly.Parse("30 / 9 / 1997"), "Holanda", 3, _context.Escuderia.Find(3));
            Piloto piloto2 = new Piloto(3, "Daniel Ricciardo", DateOnly.Parse("01 / 7 / 1989"), "Australia", 3, _context.Escuderia.Find(3));
            Piloto piloto3 = new Piloto(4, "Lando Noris", DateOnly.Parse("13 / 11 / 1999"), "Reino Unido", 4, _context.Escuderia.Find(4));
            Piloto piloto4 = new Piloto(10, "Pierre Gasly", DateOnly.Parse("07 / 2 / 1996"), "Francia", 5, _context.Escuderia.Find(5));
            Piloto piloto5 = new Piloto(14, "Fernando Alonso", DateOnly.Parse("29 / 7 / 1981"), "España", 6, _context.Escuderia.Find(6));
            Piloto piloto6 = new Piloto(16, "Charles Leclerc", DateOnly.Parse("16 /10/1997"), "Monaco", 1, _context.Escuderia.Find(1));
            Piloto piloto7 = new Piloto(18, "Lance Stroll", DateOnly.Parse("29 / 10 / 1998"), "Canada", 6, _context.Escuderia.Find(6));
            Piloto piloto8 = new Piloto(20, "Kevin Magnussen", DateOnly.Parse("05 / 10 / 1992"), "Dinamarca", 8, _context.Escuderia.Find(8));
            Piloto piloto9 = new Piloto(22, "Yuki Tsunoda", DateOnly.Parse("11 / 05 / 2000"), "Japon", 2, _context.Escuderia.Find(2));
            Piloto piloto10 = new Piloto(23, "Alex Albon", DateOnly.Parse("23 / 03 / 1996"), "Tailandia", 9, _context.Escuderia.Find(9));
            Piloto piloto11 = new Piloto(24, "Guanyu Zhou", DateOnly.Parse("30 / 5 / 1999"), "China", 7, _context.Escuderia.Find(7));
            Piloto piloto12 = new Piloto(27, "Nico Hulkenberg", DateOnly.Parse("19 / 8 / 1987"), "Alemania", 8, _context.Escuderia.Find(8));
            Piloto piloto13 = new Piloto(30, "Liam Lawson", DateOnly.Parse("11 / 2 / 2002"), "Nueva Zelanda",4, _context.Escuderia.Find(4));
            Piloto piloto14 = new Piloto(31, "Esteban Ocon", DateOnly.Parse("17 / 9 / 1996"), "Francia", 5, _context.Escuderia.Find(5));
            Piloto piloto15 = new Piloto(43, "Franco Colapinto", DateOnly.Parse("22 / 5 / 2003"), "Argentina", 9, _context.Escuderia.Find(9));
            Piloto piloto16 = new Piloto(44, "Lewis Hamilton", DateOnly.Parse("07 / 01 / 1985"), "Reino Unido", 2, _context.Escuderia.Find(2));
            Piloto piloto17 = new Piloto(55, "Carlos Sainz", DateOnly.Parse("1 / 9 / 1994"), "España", 1, _context.Escuderia.Find(1));
            Piloto piloto18 = new Piloto(77, "Valtteri Bottas", DateOnly.Parse("28 / 8 / 1989"), "Finlandia", 7, _context.Escuderia.Find(7));
            
            _context.Pilotos.Add(piloto1);
            _context.Pilotos.Add(piloto2);
            _context.Pilotos.Add(piloto3);
            _context.Pilotos.Add(piloto4);
            _context.Pilotos.Add(piloto5);
            _context.Pilotos.Add(piloto6);
            _context.Pilotos.Add(piloto7);
            _context.Pilotos.Add(piloto8);
            _context.Pilotos.Add(piloto9);
            _context.Pilotos.Add(piloto10);
            _context.Pilotos.Add(piloto11);
            _context.Pilotos.Add(piloto12);
            _context.Pilotos.Add(piloto13);
            _context.Pilotos.Add(piloto14);
            _context.Pilotos.Add(piloto15);
            _context.Pilotos.Add(piloto16); 
            _context.Pilotos.Add(piloto17);
            _context.Pilotos.Add(piloto18);
            _context.SaveChanges();

        }
    }

}

