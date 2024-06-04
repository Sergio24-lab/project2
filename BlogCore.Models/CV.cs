using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{

        public class Compra
        {
            public int Id { get; set; }
            public int ArticuloId { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
            public DateTime FechaCompra { get; set; }

            // Relación con el modelo Articulo
            public virtual Articulo Articulo { get; set; }
        }
    


    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Detalles { get; set; } // Puedes almacenar una descripción o un JSON con los detalles
    }
}
