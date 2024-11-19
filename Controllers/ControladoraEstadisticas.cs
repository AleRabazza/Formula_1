using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Formula_1.Controllers
{
    public class ControladoraEstadisticas : Controller
    {
        // GET: ControladoraEstadisticas
        public ActionResult Listado()
        {
            return View();
        }

        // GET: ControladoraEstadisticas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ControladoraEstadisticas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControladoraEstadisticas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControladoraEstadisticas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControladoraEstadisticas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControladoraEstadisticas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControladoraEstadisticas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
