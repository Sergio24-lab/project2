using BlogCore.AccesoDatos.Data.Inicializador;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.InteropServices;
using BaselineTypeDiscovery;

var builder = WebApplication.CreateBuilder(args);

// Añade la configuración de DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();


//Agregar contenedor de trabajo al contenedor IoC de inyecciï¿½n de dependencias
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

//Siembra de datos - Paso 1
builder.Services.AddScoped<IInicializadorBD, InicializadorBD>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

//MÃ©todo que ejecuta la siembra de datos
SiembraDatos();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

//Funcionalidad mÃ©todo SiembraDeDatos();
void SiembraDatos()
{
    using (var scope = app.Services.CreateScope())
    {
        var inicializadorBD = scope.ServiceProvider.GetRequiredService<IInicializadorBD>();
        inicializadorBD.Inicializar();
    }
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Otros middlewares...

    // Configuración de la ruta de búsqueda de DLL para DinkToPdf
    var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
    var wkHtmlToPdfPath = Path.Combine(env.ContentRootPath, $"wkhtmltox\\v0.12.6\\{architectureFolder}");

    // Establecer el directorio de búsqueda de la DLL
    SetDllDirectory(wkHtmlToPdfPath);
    // Otros middlewares...
}


[DllImport("kernel32", SetLastError = true)]
 static extern bool SetDllDirectory(string lpPathName);