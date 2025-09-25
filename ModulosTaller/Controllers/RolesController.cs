using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;

namespace ModulosTaller.Controllers
{
    public class RolesController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public RolesController(TallerMotosDbContext context)
        {
            _context = context;
        }

        // Vista principal
        public IActionResult Index()
        {
            var roles = _context.Roles
                .Include(r => r.Permisos)
                .ToList();

            ViewData["Permisos"] = _context.Permisos.ToList();
            return View(roles);
        }

        // Modal: Crear Rol
        public IActionResult Create()
        {
            ViewData["Permisos"] = _context.Permisos.ToList();
            return PartialView("_Create", new Role());
        }

        [HttpPost]
        public IActionResult Create(Role rol, int[] PermisosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                rol.Permisos = _context.Permisos
                    .Where(p => PermisosSeleccionados.Contains(p.IdPermiso))
                    .ToList();

                _context.Roles.Add(rol);
                _context.SaveChanges();
                TempData["Toast"] = "Rol creado correctamente";
                return RedirectToAction("Index");
            }

            ViewData["Permisos"] = _context.Permisos.ToList();
            return PartialView("_Create", rol);
        }

        // Modal: Crear Permiso
        [HttpPost]
        public IActionResult CrearPermiso(Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Permisos.Add(permiso);
                _context.SaveChanges();
                TempData["Toast"] = "Permiso creado correctamente";
                return RedirectToAction("Index");
            }

            return PartialView("_CreatePermiso", permiso);
        }

        // Modal: Detalles
        public IActionResult Details(int id)
        {
            var rol = _context.Roles
                .Include(r => r.Permisos)
                .FirstOrDefault(r => r.IdRol == id);

            if (rol == null) return NotFound();
            return PartialView("_Details", rol);
        }

        // Modal: Editar
        public IActionResult Edit(int id)
        {
            var rol = _context.Roles
                .Include(r => r.Permisos)
                .FirstOrDefault(r => r.IdRol == id);

            if (rol == null) return NotFound();

            ViewData["Permisos"] = _context.Permisos.ToList();
            return PartialView("_Edit", rol);
        }

        [HttpPost]
        public IActionResult Edit(Role rol, int[] PermisosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                var existente = _context.Roles
                    .Include(r => r.Permisos)
                    .FirstOrDefault(r => r.IdRol == rol.IdRol);

                if (existente == null) return NotFound();

                existente.NombreRol = rol.NombreRol;
                existente.Descripcion = rol.Descripcion;
                existente.Activo = rol.Activo;

                existente.Permisos.Clear();
                existente.Permisos = _context.Permisos
                    .Where(p => PermisosSeleccionados.Contains(p.IdPermiso))
                    .ToList();

                _context.SaveChanges();
                TempData["Toast"] = "Rol actualizado";
                return RedirectToAction("Index");
            }

            ViewData["Permisos"] = _context.Permisos.ToList();
            return PartialView("_Edit", rol);
        }

        // Modal: Eliminar
        public IActionResult Delete(int id)
        {
            var rol = _context.Roles.Find(id);
            if (rol == null) return NotFound();
            return PartialView("_Delete", rol);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var rol = _context.Roles
                .Include(r => r.Permisos)
                .FirstOrDefault(r => r.IdRol == id);

            if (rol == null) return NotFound();

            rol.Permisos.Clear();
            _context.Roles.Remove(rol);
            _context.SaveChanges();
            TempData["Toast"] = "Rol eliminado";
            return RedirectToAction("Index");
        }

        // AJAX: Cambiar estado activo/inactivo
        [HttpPost]
        public IActionResult CambiarEstado(int id, bool activo)
        {
            var rol = _context.Roles.Find(id);
            if (rol == null) return NotFound();

            rol.Activo = activo;
            _context.SaveChanges();
            return Ok(new { success = true });
        }
    }
}