﻿using Formula_1.Data;
using Formula_1.Models;
using Microsoft.AspNetCore.Mvc;

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
            // Verifica si la base de datos está vacía
            if (_context.Escuderia.Count() == 0)
            {
                // Precarga de datos si no existen escuderías
                List<Escuderia> escuderiasPreCargadas = new List<Escuderia>
                {
                    new Escuderia("Ferrari", "Italia", "Shell", 0, 2),
                    new Escuderia("Mercedes", "Alemania", "Petronas", 0, 2),
                    new Escuderia("Red Bull Racing", "Austria", "Oracle", 0, 2),
                    new Escuderia("McLaren", "Reino Unido", "Gulf Oil", 0, 2),
                    new Escuderia("Alpine", "Francia", "BWT", 0 , 2),
                    new Escuderia("Aston Martin", "Reino Unido", "Cognizant", 0, 2),
                    new Escuderia("Alfa Romeo", "Suiza", "Orlen", 0, 2),
                    new Escuderia("Haas", "Estados Unidos", "MoneyGram", 0 , 2),
                    new Escuderia("Williams", "Reino Unido", "Duracell", 0, 2)
        };

                // Agrega las escuderías al contexto y guarda cambios
                _context.Escuderia.AddRange(escuderiasPreCargadas);
                _context.SaveChanges();
            }

            // Obtiene la lista de escuderías actualizada
            List<Escuderia> listaEscuderias = _context.Escuderia.ToList();
            ViewBag.Escuderias = listaEscuderias;
            ViewBag.PuedeAgregar = Validacion1();

            return View("Listado");
        }
        private bool Validacion1()
        {
            if (_context.Escuderia.Count() < 10)
            {
                return true;
            }
            return false;
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
            if (escuderia == null)
            {

                return View("Listado");

            }

            escuderia.Nombre = nombre;
            escuderia.PaisDeOrigen = paisDeOrigen;
            escuderia.SponsorPrincipal = sponsorPrincipal;


            if (escuderia.Validacion())
            {
                _context.Escuderia.Update(escuderia);
                _context.SaveChanges();
                return RedirectToAction("Listado");
            }
            ViewBag.Nombre = nombre;
            ViewBag.PaisOrigen = paisDeOrigen;
            ViewBag.SponsorPrincipal = sponsorPrincipal;



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

    }
}
