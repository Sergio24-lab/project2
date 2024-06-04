using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogCore.Models
{
    public class Articulo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del Artículo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public int ProveedorId { get; set; }

        [ForeignKey("ProveedorId")]
        public Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio Compra")]
        public decimal PrecioCompra { get; set; }

        [NotMapped]
        [Display(Name = "Precio Venta")]
        public decimal PrecioVenta
        {
            get
            {
                return PrecioCompra * 1.35m;
            }
        }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }
    }
}
