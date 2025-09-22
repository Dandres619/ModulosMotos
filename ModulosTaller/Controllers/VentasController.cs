using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModulosTaller.Controllers
{
    public class VentasController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public VentasController(TallerMotosDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.VentaDetalles)
                    .ThenInclude(vd => vd.IdProductoNavigation)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            return View(ventas);
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.VentaDetalles)
                    .ThenInclude(vd => vd.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public async Task<IActionResult> Create()
        {
            // Cargar clientes activos y productos
            var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
            var productos = await _context.Productos.ToListAsync();

            ViewBag.ClientesList = new SelectList(clientes, "IdCliente", "NombreCompleto");
            ViewBag.ProductosList = new SelectList(productos, "IdProducto", "NombreProducto");

            // Establecer fecha actual por defecto
            ViewBag.FechaActual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            return View();
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta venta, int[] productos, int[] cantidades, decimal[] precios)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Crear la venta
                    _context.Ventas.Add(venta);
                    await _context.SaveChangesAsync();

                    // Agregar detalles de venta si existen productos
                    if (productos != null && productos.Length > 0)
                    {
                        for (int i = 0; i < productos.Length; i++)
                        {
                            if (productos[i] > 0 && cantidades[i] > 0)
                            {
                                var detalle = new VentaDetalle
                                {
                                    IdVenta = venta.IdVenta,
                                    IdProducto = productos[i],
                                    Cantidad = cantidades[i],
                                    PrecioUnitario = precios[i]
                                };
                                _context.VentaDetalles.Add(detalle);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Venta creada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear la venta: " + ex.Message);
                }
            }

            // Recargar los datos si hay error
            var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
            var productosList = await _context.Productos.ToListAsync();

            ViewBag.ClientesList = new SelectList(clientes, "IdCliente", "NombreCompleto", venta.IdCliente);
            ViewBag.ProductosList = new SelectList(productosList, "IdProducto", "NombreProducto");

            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.VentaDetalles)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
            var productos = await _context.Productos.ToListAsync();

            ViewBag.ClientesList = new SelectList(clientes, "IdCliente", "NombreCompleto", venta.IdCliente);
            ViewBag.ProductosList = new SelectList(productos, "IdProducto", "NombreProducto");

            return View(venta);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venta venta, int[] productos, int[] cantidades, decimal[] precios)
        {
            if (id != venta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);

                    // Eliminar detalles existentes
                    var detallesExistentes = _context.VentaDetalles.Where(vd => vd.IdVenta == id);
                    _context.VentaDetalles.RemoveRange(detallesExistentes);

                    // Agregar nuevos detalles
                    if (productos != null && productos.Length > 0)
                    {
                        for (int i = 0; i < productos.Length; i++)
                        {
                            if (productos[i] > 0 && cantidades[i] > 0)
                            {
                                var detalle = new VentaDetalle
                                {
                                    IdVenta = venta.IdVenta,
                                    IdProducto = productos[i],
                                    Cantidad = cantidades[i],
                                    PrecioUnitario = precios[i]
                                };
                                _context.VentaDetalles.Add(detalle);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venta actualizada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.IdVenta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
            var productosList = await _context.Productos.ToListAsync();

            ViewBag.ClientesList = new SelectList(clientes, "IdCliente", "NombreCompleto", venta.IdCliente);
            ViewBag.ProductosList = new SelectList(productosList, "IdProducto", "NombreProducto");

            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                // Eliminar detalles primero
                var detalles = _context.VentaDetalles.Where(vd => vd.IdVenta == id);
                _context.VentaDetalles.RemoveRange(detalles);

                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venta eliminada exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.IdVenta == id);
        }
    }
}