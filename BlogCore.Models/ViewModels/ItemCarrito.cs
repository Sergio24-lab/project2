using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class ItemCarrito
    {
        public int ArticuloId { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string UrlImagen { get; set; }
    }


}
