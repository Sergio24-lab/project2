using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class ProveedoresController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public ProveedoresController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Proveedor.Add(proveedor);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Proveedor proveedor = _contenedorTrabajo.Proveedor.Get(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Proveedor.Update(proveedor);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Proveedor.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var proveedor = _contenedorTrabajo.Proveedor.Get(id);
            if (proveedor == null)
            {
                return Json(new { success = false, message = "Error borrando proveedor" });
            }

            _contenedorTrabajo.Proveedor.Remove(proveedor);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Proveedor borrado correctamente" });
        }
        #endregion
    }
}
