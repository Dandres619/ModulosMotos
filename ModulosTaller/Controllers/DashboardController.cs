using Microsoft.AspNetCore.Mvc;
using ModulosTaller.Models;

namespace ModulosTaller.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public DashboardController(TallerMotosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            var rol = HttpContext.Session.GetString("RolUsuario");
            var nombre = HttpContext.Session.GetString("NombreUsuario");

            // Validación completa: si no hay sesión o no es admin, redirige al login
            if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrEmpty(rol) || rol != "Admin")
            {
                return RedirectToAction("Login", "Acceso");
            }

            var model = new DashboardViewModel
            {
                NombreUsuario = nombre,
                TotalUsuarios = _context.Usuarios.Count(),
                TotalClientes = _context.Clientes.Count(),
                TotalMotocicletas = _context.Motocicletas.Count(),
                TotalProductos = _context.Productos.Count(),
                TotalCompras = _context.Compras.Count(),
                TotalVentas = _context.Ventas.Count(),
                TotalAgendamientos = _context.Agendamientos.Count()
            };

            return View(model);
        }

        public IActionResult VistaUsuario()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            var nombre = HttpContext.Session.GetString("NombreUsuario");

            // Validación: si no hay sesión, redirige al login
            if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrEmpty(nombre))
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewBag.Nombre = nombre;
            return View();
        }
    }
}