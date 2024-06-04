using BlogCore.AccesoDatos.Data.Repository.IRepository;
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
    public class GraficosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IConverter _converter;

        public GraficosController(IContenedorTrabajo contenedorTrabajo, IConverter converter)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _converter = converter;
        }
        public IActionResult ComprasPorFecha()
        {
            var compras = _contenedorTrabajo.Compra.GetAll();
            var comprasPorFecha = new Dictionary<string, (int cantidad, decimal precio)>();

            foreach (var compra in compras)
            {
                var fecha = compra.FechaCompra.ToString("yyyy-MM-dd");
                if (comprasPorFecha.ContainsKey(fecha))
                {
                    comprasPorFecha[fecha] = (comprasPorFecha[fecha].cantidad + compra.Cantidad, comprasPorFecha[fecha].precio + compra.Precio);
                }
                else
                {
                    comprasPorFecha[fecha] = (compra.Cantidad, compra.Precio);
                }
            }

            var fechas = comprasPorFecha.Keys.ToArray();
            var volumenes = comprasPorFecha.Values.Select(x => x.cantidad).ToArray();
            var precios = comprasPorFecha.Values.Select(x => x.precio).ToArray();

            ViewBag.Fechas = fechas;
            ViewBag.Volumenes = volumenes;
            ViewBag.Precios = precios;

            return View("ComprasPorFecha");
        }
        public IActionResult VentasPorMes()
        {
            var ventas = _contenedorTrabajo.Venta.GetAll();
            var ventasPorMes = new Dictionary<string, decimal>();

            foreach (var venta in ventas)
            {
                var mes = venta.Fecha.ToString("MMM yyyy");
                if (ventasPorMes.ContainsKey(mes))
                {
                    ventasPorMes[mes] += venta.Total;
                }
                else
                {
                    ventasPorMes[mes] = venta.Total;
                }
            }

            var meses = ventasPorMes.Keys.ToArray();
            var totalesPorMes = ventasPorMes.Values.ToArray();

            ViewBag.Meses = meses;
            ViewBag.TotalesPorMes = totalesPorMes;

            return View("VentasPorMes");
        }


        [HttpPost]
        public IActionResult GenerarPdfCompras(string imageData)
        {
            var imageDataBytes = Convert.FromBase64String(imageData.Split(',')[1]);
            var imageFileName = $"GraficoCompras_{DateTime.Now:yyyyMMddHHmmss}.png";
            var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportGraficos", imageFileName);

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportGraficos"));
            System.IO.File.WriteAllBytes(imageFilePath, imageDataBytes);

            var htmlContent = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Reporte de Compras por Fecha</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            padding: 20px;
                        }}
                        .container {{
                            text-align: center;
                        }}
                        img {{
                            max-width: 100%;
                            height: auto;
                        }}
                        .logo {{
                                margin-bottom: 20px;
                                width: 100px; 
                                height: auto;
                            }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <img class='logo' src='C:\Users\sergi\Downloads\Logo_Digito.png' alt='Logo'>
                        <h1>Reporte de Compras por Fecha</h1>
                        <img src='data:image/png;base64,{Convert.ToBase64String(imageDataBytes)}' alt='Gráfico de Compras'>
                    </div>
                </body>
                </html>";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A3Rotated,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Reporte de Compras por Fecha"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var pdfFile = _converter.Convert(pdfDocument);
            var pdfFileName = $"ReporteComprasPorFecha_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportGraficos", pdfFileName);
            System.IO.File.WriteAllBytes(pdfFilePath, pdfFile);

            return Json(new { success = true, message = "Reporte de compras generado correctamente", pdfUrl = $"/ReportGraficos/{pdfFileName}" });
        }



        [HttpPost]
        public IActionResult GenerarPdf(string imageData)
        {
            var imageDataBytes = Convert.FromBase64String(imageData.Split(',')[1]);
            var imageFileName = $"GraficoVentas_{DateTime.Now:yyyyMMddHHmmss}.png";
            var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportGraficos", imageFileName);

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportGraficos"));
            System.IO.File.WriteAllBytes(imageFilePath, imageDataBytes);

            var htmlContent = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Reporte de Ventas por Mes</title>
                    <style>
                        .logo {{
                                margin-bottom: 20px;
                                width: 100px; 
                                height: auto;
                            }}
                        body {{
                            font-family: Arial, sans-serif;
                            padding: 20px;
                        }}
                        .container {{
                            text-align: center;
                        }}
                        img {{
                            max-width: 100%;
                            height: auto;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                    <img class='logo' src='C:\Users\sergi\Downloads\Logo_Digito.png' alt='Logo'>
                        <h1>Reporte de Ventas por Mes</h1>
                        <img src='data:image/png;base64,{Convert.ToBase64String(imageDataBytes)}' alt='Gráfico de Ventas'>
                    </div>
                </body>
                </html>";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A3Rotated,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Reporte de Ventas por Mes"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var pdfFile = _converter.Convert(pdfDocument);
            var pdfFileName = $"ReporteVentasPorMes_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportGraficos", pdfFileName);
            System.IO.File.WriteAllBytes(pdfFilePath, pdfFile);

            return Json(new { success = true, message = "Reporte de ventas generado correctamente", pdfUrl = $"/ReportGraficos/{pdfFileName}" });
        }
    }
}
