using BlogCore.Data;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using System;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        private readonly ApplicationDbContext _db;

        public VentaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }

    public interface IVentaRepository : IRepository<Venta>
    {
    }
}
