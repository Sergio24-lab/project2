using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class ProveedorRepository : Repository<Proveedor>, IProveedorRepository
    {
        private readonly ApplicationDbContext _db;

        public ProveedorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaProveedores()
        {
            return _db.Proveedor.Select(p => new SelectListItem()
            {
                Text = p.Nombre,
                Value = p.Id.ToString()
            });
        }

        public void Update(Proveedor proveedor)
        {
            var objDesdeDb = _db.Proveedor.FirstOrDefault(s => s.Id == proveedor.Id);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = proveedor.Nombre;
                objDesdeDb.Direccion = proveedor.Direccion;
                objDesdeDb.Telefono = proveedor.Telefono;
                //_db.SaveChanges(); // Asegúrate de guardar los cambios
            }
        }
    }
}
