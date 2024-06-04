using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IProveedorRepository : IRepository<Proveedor>
    {
        void Update(Proveedor proveedor);

        IEnumerable<SelectListItem> GetListaProveedores();
    }
}
