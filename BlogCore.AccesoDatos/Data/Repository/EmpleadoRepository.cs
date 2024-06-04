using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using System.Linq;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class EmpleadoRepository : Repository<Empleado>, IEmpleadoRepository
    {
        private readonly ApplicationDbContext _db;

        public EmpleadoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Empleado empleado)
        {
            var objDesdeDb = _db.Empleado.FirstOrDefault(e => e.Id == empleado.Id);
            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = empleado.Nombre;
                objDesdeDb.FechaNacimiento = empleado.FechaNacimiento;
                objDesdeDb.Salario = empleado.Salario;
                objDesdeDb.Direccion = empleado.Direccion;
                objDesdeDb.CorreoElectronico = empleado.CorreoElectronico;
                objDesdeDb.NumeroTelefono = empleado.NumeroTelefono;
                objDesdeDb.FechaContratacion = empleado.FechaContratacion;
                objDesdeDb.Departamento = empleado.Departamento;
                objDesdeDb.Cargo = empleado.Cargo;

                //_db.SaveChanges();
            }
        }
    }
}
