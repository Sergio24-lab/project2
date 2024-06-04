using BlogCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Poner aquí todos los modelos que se vayan creando
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Empleado> Empleado { get; set; }



    }


}
