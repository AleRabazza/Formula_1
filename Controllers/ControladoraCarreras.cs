using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Formula_1.Controllers
{
    public class ControladoraCarreras : Controller
    {
        // GET: ControladoraCarreras
        public ActionResult Listado()
        {
            return View();
        }

        // GET: ControladoraCarreras/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ControladoraCarreras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControladoraCarreras/Create
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

        // GET: ControladoraCarreras/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControladoraCarreras/Edit/5
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

        // GET: ControladoraCarreras/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControladoraCarreras/Delete/5
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
