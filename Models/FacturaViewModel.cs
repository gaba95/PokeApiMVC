using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class ClienteViewModel
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }
    }

    public class ProductoViewModel
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
    }

    public class FacturaViewModel
    {
        public ClienteViewModel Cliente { get; set; }
        public List<ProductoViewModel> Productos { get; set; } = new List<ProductoViewModel>();
    }
}
