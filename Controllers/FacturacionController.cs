using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PokeApiMVC.Controllers
{
    public class FacturacionController : Controller
    {
        public ActionResult Index()
        {
            var modelo = new FacturaViewModel
            {
                Cliente = new ClienteViewModel(),
                Productos = new List<ProductoViewModel>()
            };
            return View(modelo);
        }

        [HttpPost]
        public ActionResult GenerarFactura(FacturaViewModel modelo)
        {
            if (modelo.Productos == null || modelo.Productos.Count == 0)
            {
                TempData["Error"] = "Por favor completa los datos del cliente y agrega al menos un producto.";
                return View("Index", modelo);
            }

            try
            {
                XNamespace ns = "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.4/facturaElectronica";
                XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

                string clave = "506" + DateTime.Now.ToString("ddMMyyHHmmss") + "999999999999" + "01" + "00100001010000001";

                var xml = new XDocument(
                    new XElement(ns + "FacturaElectronica",
                        new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                        new XAttribute(xsi + "schemaLocation",
                            "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.4/facturaElectronica " +
                            "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.4/facturaElectronica.xsd"),

                        new XElement(ns + "Clave", clave),
                        new XElement(ns + "NumeroConsecutivo", "00100001010000001"),
                        new XElement(ns + "FechaEmision", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),

                        new XElement(ns + "Emisor",
                            new XElement(ns + "Nombre", "ENLASA"),
                            new XElement(ns + "Identificacion",
                                new XElement(ns + "Tipo", "01"),
                                new XElement(ns + "Numero", "3030303")
                            ),
                            new XElement(ns + "NombreComercial", "FILIAL 1"),
                            new XElement(ns + "Ubicacion",
                                new XElement(ns + "Provincia", "1"),
                                new XElement(ns + "Canton", "01"),
                                new XElement(ns + "Distrito", "01"),
                                new XElement(ns + "Barrio", "01"),
                                new XElement(ns + "OtrasSenas", "San José centro")
                            ),
                            new XElement(ns + "Telefono",
                                new XElement(ns + "CodigoPais", "506"),
                                new XElement(ns + "NumTelefono", "88888888")
                            ),
                            new XElement(ns + "CorreoElectronico", "ventas@ENLASA.com")
                        ),

                        // Receptor
                        new XElement(ns + "Receptor",
                            new XElement(ns + "Nombre", modelo.Cliente.Nombre),
                            new XElement(ns + "Identificacion",
                                new XElement(ns + "Tipo", "01"),
                                new XElement(ns + "Numero", modelo.Cliente.Identificacion)
                            ),
                            new XElement(ns + "CorreoElectronico", modelo.Cliente.Correo)
                        ),

                        // DetalleServicio
                        new XElement(ns + "DetalleServicio",
                            from p in modelo.Productos
                            select new XElement(ns + "LineaDetalle",
                                new XElement(ns + "NumeroLinea", modelo.Productos.IndexOf(p) + 1),
                                new XElement(ns + "Codigo", p.Codigo),
                                new XElement(ns + "Cantidad", p.Cantidad),
                                new XElement(ns + "UnidadMedida", "Unid"),
                                new XElement(ns + "Detalle", p.Nombre),
                                new XElement(ns + "PrecioUnitario", p.Precio),
                                new XElement(ns + "MontoTotal", p.Precio * p.Cantidad),
                                new XElement(ns + "SubTotal", p.Precio * p.Cantidad),
                                new XElement(ns + "Impuesto",
                                    new XElement(ns + "Codigo", "01"),
                                    new XElement(ns + "Tarifa", p.PorcentajeImpuesto),
                                    new XElement(ns + "Monto", (p.Precio * p.Cantidad * p.PorcentajeImpuesto / 100))
                                ),
                                new XElement(ns + "MontoTotalLinea", p.Precio * p.Cantidad * (1 + p.PorcentajeImpuesto / 100))
                            )
                        ),

                        // ResumenFactura
                        new XElement(ns + "ResumenFactura",
                            new XElement(ns + "TotalServGravados", modelo.Productos.Sum(p => p.Precio * p.Cantidad)),
                            new XElement(ns + "TotalImpuesto", modelo.Productos.Sum(p => p.Precio * p.Cantidad * p.PorcentajeImpuesto / 100)),
                            new XElement(ns + "TotalComprobante", modelo.Productos.Sum(p => p.Precio * p.Cantidad * (1 + p.PorcentajeImpuesto / 100)))
                        )
                    )
                );

                // Guardar XML en el servidor
                string rutaCarpeta = Server.MapPath("~/FacturasXML/");
                if (!Directory.Exists(rutaCarpeta))
                    Directory.CreateDirectory(rutaCarpeta);

                string nombreArchivo = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);
                xml.Save(rutaArchivo);

                TempData["Exito"] = $"Factura generada correctamente. Archivo: {nombreArchivo}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al generar la factura: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
