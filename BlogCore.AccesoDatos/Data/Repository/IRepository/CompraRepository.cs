using BlogCore.Data;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using System;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class CompraRepository : Repository<Compra>, ICompraRepository
    {
        private readonly ApplicationDbContext _db;

        public CompraRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }

    public interface ICompraRepository : IRepository<Compra>
    {
    }
}
