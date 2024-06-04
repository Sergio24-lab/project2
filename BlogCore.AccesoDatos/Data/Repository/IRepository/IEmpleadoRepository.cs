using BlogCore.Models;
using System;
using System.Linq;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IEmpleadoRepository : IRepository<Empleado>
    {

        void Update(Empleado empleado);
    }
}
