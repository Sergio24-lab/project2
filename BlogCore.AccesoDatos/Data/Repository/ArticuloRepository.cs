using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IQueryable<Articulo> AsQueryable()
        {
            return _db.Set<Articulo>().AsQueryable();
        }



        public Articulo GetPrimero(Expression<Func<Articulo, bool>> filtro, string includeProperties = null)
        {
            IQueryable<Articulo> query = _db.Articulo;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault(filtro);
        }


        public void Update(Articulo articulo)
        {
            var objDesdeDb = _db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = articulo.Nombre;
                objDesdeDb.Descripcion = articulo.Descripcion;
                objDesdeDb.UrlImagen = articulo.UrlImagen;
                objDesdeDb.CategoriaId = articulo.CategoriaId;
                objDesdeDb.ProveedorId = articulo.ProveedorId;
                objDesdeDb.PrecioCompra = articulo.PrecioCompra;
                objDesdeDb.Stock = articulo.Stock;

                //_db.SaveChanges();
            }
        }
    }
}
