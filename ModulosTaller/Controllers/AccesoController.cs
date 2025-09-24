using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;

namespace ModulosTaller.Controllers
{
    public class AccesoController : Controller
    {
        private readonly TallerMotosDbContext _context;

        private const string AdminCorreo = "admin@rafamotos.com";
        private const string AdminClave = "Admin123";

        public AccesoController(TallerMotosDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string correo, string clave)
        {
            if (correo == AdminCorreo && clave == AdminClave)
            {
                HttpContext.Session.SetString("UsuarioId", "0");
                HttpContext.Session.SetString("NombreUsuario", "Administrador");
                HttpContext.Session.SetString("RolUsuario", "Admin");

                return RedirectToAction("Index", "Dashboard");
            }

            var usuario = _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefault(u => u.Correo == correo && u.Clave == clave);

            if (usuario == null)
            {
                TempData["Error"] = "Credenciales inválidas";
                return View();
            }

            HttpContext.Session.SetString("UsuarioId", usuario.IdUsuario.ToString());
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetString("RolUsuario", usuario.IdRolNavigation?.NombreRol ?? "Usuario");

            if (usuario.IdRolNavigation?.NombreRol == "Admin")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("VistaUsuario", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Recuperar() => PartialView("_ModalRecuperar");

        [HttpPost]
        public IActionResult EnviarRecuperacion(string correo)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);
            if (usuario == null)
            {
                TempData["Error"] = "Correo no registrado";
                return RedirectToAction("Login");
            }

            return RedirectToAction("CambiarClave", new { correo });
        }

        public IActionResult CambiarClave(string correo) => View(model: correo);

        [HttpPost]
        public IActionResult CambiarClave(string correo, string nueva, string confirmar)
        {
            if (nueva != confirmar)
            {
                TempData["Error"] = "Las contraseñas no coinciden";
                return View(model: correo);
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);
            if (usuario == null) return NotFound();

            usuario.Clave = nueva;
            _context.SaveChanges();
            TempData["Toast"] = "Contraseña actualizada";
            return RedirectToAction("Login");
        }

        // ✅ NUEVO: Registro público de usuarios
        public IActionResult Registro()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            bool correoExiste = _context.Usuarios.Any(u => u.Correo == usuario.Correo);

            if (correoExiste)
            {
                TempData["Error"] = "El correo ya está registrado.";
                return View(usuario);
            }

            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                TempData["Mensaje"] = "Registro exitoso. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            return View(usuario);
        }
    }
}