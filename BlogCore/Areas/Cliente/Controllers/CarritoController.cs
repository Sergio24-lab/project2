using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class CarritoController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IConverter _converter;
        private static Carrito carrito = new Carrito();

        public CarritoController(IContenedorTrabajo contenedorTrabajo, IConverter converter)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _converter = converter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(carrito);
        }

        [HttpPost]
        public IActionResult AgregarAlCarrito([FromBody] ItemCarrito itemCarrito)
        {
            carrito.AgregarItem(itemCarrito);
            return Json(new { success = true, message = "Artículo agregado al carrito" });
        }

        [HttpPost]
        public IActionResult EliminarProducto(int id)
        {
            var item = carrito.Items.FirstOrDefault(x => x.ArticuloId == id);

            if (item != null)
            {
                carrito.Items.Remove(item);
                // Actualizar el carrito en la base de datos si es necesario
            }

            return Json(new { success = true, total = carrito.Items.Sum(x => x.Cantidad * x.Precio).ToString("C") });
        }

        [HttpPost]
        public IActionResult ProcesarPago()
        {
            if (carrito.Items.Count == 0)
            {
                return Json(new { success = false, message = "El carrito está vacío" });
            }

            // Calcular total a pagar
            decimal totalPagar = carrito.Items.Sum(item => item.Cantidad * item.Precio);

            // Generar PDF
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A3Rotated,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Factura de Compra"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GetHtmlString(),
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Página [page] de [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Factura de Compra" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "facturas", $"factura_{DateTime.Now.Ticks}.pdf");
            System.IO.File.WriteAllBytes(filePath, file);

            // Restar stock y registrar venta
            foreach (var item in carrito.Items)
            {
                var articulo = _contenedorTrabajo.Articulo.Get(item.ArticuloId);
                if (articulo != null)
                {
                    if (articulo.Stock >= item.Cantidad)
                    {
                        articulo.Stock -= item.Cantidad;

                        Venta venta = new Venta
                        {
                            Fecha = DateTime.Now,
                            Total = totalPagar,
                            Detalles = System.Text.Json.JsonSerializer.Serialize(carrito.Items)
                        };
                        _contenedorTrabajo.Venta.Add(venta);
                        _contenedorTrabajo.Save();
                    }
                    else
                    {
                        return Json(new { success = false, message = $"No hay suficiente stock para el producto: {item.Nombre}" });
                    }
                }
            }

            // Limpiar carrito
            carrito = new Carrito();

            return Json(new { success = true, message = "Compra procesada exitosamente", pdfUrl = $"/facturas/{Path.GetFileName(filePath)}" });
        }

        private string GetHtmlString()
        {
            var html = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Factura de Compra</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #593196; 
                                padding: 20px;
                            }
                            .invoice-container {
                                background-color: #E6E6FA;
                                padding: 20px;
                                border-radius: 10px;
                                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                            }
                            .logo {
                                margin-bottom: 20px;
                                width: 100px; 
                                height: auto;
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
                        <div class='invoice-container'>
                            <img class='logo' src='C:\Users\sergi\Downloads\Logo_Digito.png' alt='Logo'>
                            <h1>Factura de Compra</h1>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Producto</th>
                                        <th>Cantidad</th>
                                        <th>Precio</th>
                                        <th>Total</th>
                                    </tr>
                                </thead>
                                <tbody>";

            decimal totalPagar = 0;

            foreach (var item in carrito.Items)
            {
                decimal subtotal = item.Cantidad * item.Precio;
                totalPagar += subtotal;
                html += $@"
                        <tr>
                            <td>{item.Nombre}</td>
                            <td>{item.Cantidad}</td>
                            <td>{item.Precio.ToString("C")}</td>
                            <td>{subtotal.ToString("C")}</td>
                        </tr>";
            }

            html += $@"
                        <tr>
                            <td colspan='3'><strong>Total a pagar:</strong></td>
                            <td>{totalPagar.ToString("C")}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            </body>
            </html>";

            return html;
        }
    }
}
