using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IArticuloRepository : IRepository<Articulo>
    {
        void Update(Articulo articulo);
        Articulo GetPrimero(System.Linq.Expressions.Expression<Func<Articulo, bool>> filtro, string includeProperties = null);
        IQueryable<Articulo> AsQueryable();
    }
}
