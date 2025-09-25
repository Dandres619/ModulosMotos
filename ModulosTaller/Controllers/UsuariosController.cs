using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;

namespace ModulosTaller.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public UsuariosController(TallerMotosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .ToList();

            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View(usuarios);
        }

        // Modal: Crear Usuario
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return PartialView("_Create", new Usuario());
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");

            if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
            {
                TempData["Error"] = "El correo ya está registrado.";
                return PartialView("_Create", usuario);
            }

            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                TempData["Toast"] = "Usuario creado con éxito.";
                return RedirectToAction("Index");
            }

            return PartialView("_Create", usuario);
        }

        // Modal: Editar Usuario
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return PartialView("_Edit", usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);

            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuario);
                _context.SaveChanges();
                TempData["Toast"] = "Usuario actualizado";
                return RedirectToAction("Index");
            }

            return PartialView("_Edit", usuario);
        }

        // Modal: Eliminar Usuario
        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            return PartialView("_Delete", usuario);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            TempData["Toast"] = "Usuario eliminado";
            return RedirectToAction("Index");
        }

        // Modal: Detalles del Usuario
        public IActionResult Details(int id)
        {
            var usuario = _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefault(u => u.IdUsuario == id);

            return usuario == null ? NotFound() : PartialView("_ModalDetalles", usuario);
        }

        // Botón de estado dinámico
        [HttpPost]
        public IActionResult CambiarEstado(int id, bool activo)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            usuario.Activo = activo;
            _context.SaveChanges();
            return Ok();
        }
    }
}