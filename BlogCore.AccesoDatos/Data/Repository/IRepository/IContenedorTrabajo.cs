using System;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        ICategoriaRepository Categoria { get; }
        IArticuloRepository Articulo { get; }
        ISliderRepository Slider { get; }
        IUsuarioRepository Usuario { get; }
        IProveedorRepository Proveedor { get; }
        ICompraRepository Compra { get; }
        IEmpleadoRepository Empleado { get; }
        IVentaRepository Venta
        {
            get;
        }
        void Save();
    }
}
