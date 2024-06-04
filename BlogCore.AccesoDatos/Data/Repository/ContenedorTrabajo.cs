using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using System;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);
            Articulo = new ArticuloRepository(_db);
            Proveedor = new ProveedorRepository(_db);
            Slider = new SliderRepository(_db);
            Usuario = new UsuarioRepository(_db);
            Compra = new CompraRepository(_db);
            Venta = new VentaRepository(_db);
            Empleado = new EmpleadoRepository(_db);

        }

        public ICategoriaRepository Categoria { get; private set; }
        public IArticuloRepository Articulo { get; private set; }
        public IProveedorRepository Proveedor { get; private set; }
        public ISliderRepository Slider { get; private set; }
        public IUsuarioRepository Usuario { get; private set; }
        public IVentaRepository Venta { get; private set; }
        public ICompraRepository Compra { get; private set; }
        public IEmpleadoRepository Empleado { get; private set; }


        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
