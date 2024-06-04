using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class EmpleadoController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IConverter _converter;

        public EmpleadoController(IContenedorTrabajo contenedorTrabajo, IConverter converter)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _converter = converter;
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
        public IActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Empleado.Add(empleado);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Empleado empleado = _contenedorTrabajo.Empleado.Get(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Empleado.Update(empleado);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Empleado.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var empleado = _contenedorTrabajo.Empleado.Get(id);
            if (empleado == null)
            {
                return Json(new { success = false, message = "Error borrando empleado" });
            }

            _contenedorTrabajo.Empleado.Remove(empleado);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Empleado borrado correctamente" });
        }

        [HttpPost]
        public IActionResult GenerarReporte()
        {
            var empleados = _contenedorTrabajo.Empleado.GetAll().ToList();

            var html = GetHtmlString(empleados);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.TabloidExtra,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Reporte de Empleados"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Página [page] de [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Reporte de Empleados" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            var fileName = $"ReporteEmpleados_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reportes", fileName);
            System.IO.File.WriteAllBytes(filePath, file);

            return Json(new { success = true, message = "Reporte de empleados generado correctamente", pdfUrl = $"/reportes/{fileName}" });
        }

        private string GetHtmlString(List<Empleado> empleados)
        {
            var html = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Reporte de Empleados</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #593196;
                                padding: 20px;
                            }
                            .logo {
                                margin-bottom: 20px;
                                width: 100px; 
                                height: auto;
                            }
                            .container {
                                background-color: #E6E6FA;
                                padding: 20px;
                                border-radius: 10px;
                                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                            }
                            table {
                                width: 100%;
                                border-collapse: collapse;
                            }
                            th, td {
                                padding: 10px;
                                text-align: left;
                                border-bottom: 1px solid #ddd;
                            }
                            th {
                                background-color: White;
                            }
                            h1 {
                                color: #333;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <img class='logo' src='C:\Users\sergi\Downloads\Logo_Digito.png' alt='Logo'>
                            <h1>Reporte de Empleados</h1>
                            <table>
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Nombre</th>
                                        <th>Fecha de Nacimiento</th>
                                        <th>Salario</th>
                                        <th>Dirección</th>
                                        <th>Correo Electrónico</th>
                                        <th>Número de Teléfono</th>
                                        <th>Fecha de Contratación</th>
                                        <th>Departamento</th>
                                        <th>Cargo</th>
                                    </tr>
                                </thead>
                                <tbody>";

            foreach (var empleado in empleados)
            {
                html += $@"
                                    <tr>
                                        <td>{empleado.Id}</td>
                                        <td>{empleado.Nombre}</td>
                                        <td>{empleado.FechaNacimiento.ToShortDateString()}</td>
                                        <td>{empleado.Salario}</td>
                                        <td>{empleado.Direccion}</td>
                                        <td>{empleado.CorreoElectronico}</td>
                                        <td>{empleado.NumeroTelefono}</td>
                                        <td>{empleado.FechaContratacion.ToShortDateString()}</td>
                                        <td>{empleado.Departamento}</td>
                                        <td>{empleado.Cargo}</td>
                                    </tr>";
            }

            html += @"
                                </tbody>
                            </table>
                        </div>
                    </body>
                    </html>";

            return html;
        }
        #endregion
    }
}
