using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;

namespace ModulosTaller.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public ProveedoresController(TallerMotosDbContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.Compras)
                .ToListAsync();
            return View(proveedores);
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores
                .Include(p => p.Compras)              
                    .ThenInclude(c => c.CompraDetalles) 
                        .ThenInclude(cd => cd.IdProductoNavigation)
                .FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }


        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreProveedor,Telefono,Direccion")] Proveedore proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProveedor,NombreProveedor,Telefono,Direccion")] Proveedore proveedor)
        {
            if (id != proveedor.IdProveedor)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.IdProveedor))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _context.Proveedores
                .Include(p => p.Compras)
                .FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (proveedor == null)
                return NotFound();

            // No permitir eliminar proveedor con compras
            if (proveedor.Compras.Any())
            {
                TempData["Error"] = "No se puede eliminar un proveedor con compras registradas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(e => e.IdProveedor == id);
        }
    }
}
