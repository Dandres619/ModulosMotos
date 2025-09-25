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
            try
            {
                // Cargar clientes activos y productos con stock
                var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
                var productos = await _context.Productos.Where(p => p.Stock > 0).ToListAsync();

                // Pasar las listas como ViewBag (no como SelectList) para que funcione con tu vista
                ViewBag.Clientes = clientes;
                ViewBag.Productos = productos;

                // Establecer fecha actual por defecto
                ViewBag.FechaActual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

                return View();
            }
            catch (Exception ex)
            {
                // En caso de error, pasar listas vacías
                ViewBag.Clientes = new List<Cliente>();
                ViewBag.Productos = new List<Producto>();
                ViewBag.Error = "Error al cargar los datos: " + ex.Message;
                return View();
            }
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,IdCliente,FechaVenta,Observaciones")] Venta venta, List<VentaDetalle> VentaDetalles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar que haya detalles de venta
                    if (VentaDetalles == null || !VentaDetalles.Any())
                    {
                        ModelState.AddModelError("", "Debe agregar al menos un producto a la venta");
                    }
                    else
                    {
                        // Crear la venta
                        venta.FechaVenta = DateTime.Now; // Asegurar la fecha actual
                        _context.Ventas.Add(venta);
                        await _context.SaveChangesAsync();

                        // Agregar detalles de venta
                        foreach (var detalle in VentaDetalles)
                        {
                            if (detalle.IdProducto > 0 && detalle.Cantidad > 0)
                            {
                                detalle.IdVenta = venta.IdVenta;
                                _context.VentaDetalles.Add(detalle);

                                // Actualizar stock del producto
                                var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                                if (producto != null)
                                {
                                    if (producto.Stock >= detalle.Cantidad)
                                    {
                                        producto.Stock -= detalle.Cantidad;
                                        _context.Productos.Update(producto);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", $"Stock insuficiente para el producto: {producto.NombreProducto}");
                                        // Recargar datos y retornar la vista
                                        await RecargarDatosViewBag();
                                        return View(venta);
                                    }
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Venta creada exitosamente";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear la venta: " + ex.Message);
                }
            }

            // Recargar los datos si hay error
            await RecargarDatosViewBag();
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
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,IdCliente,FechaVenta,Observaciones")] Venta venta, int[] productos, int[] cantidades, decimal[] precios)
        {
            if (id != venta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener la venta existente
                    var ventaExistente = await _context.Ventas
                        .Include(v => v.VentaDetalles)
                        .FirstOrDefaultAsync(v => v.IdVenta == id);

                    if (ventaExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar propiedades básicas
                    ventaExistente.IdCliente = venta.IdCliente;
                    ventaExistente.FechaVenta = venta.FechaVenta;
                    ventaExistente.Observaciones = venta.Observaciones;

                    // Restaurar stock de los detalles antiguos
                    foreach (var detalle in ventaExistente.VentaDetalles)
                    {
                        var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                        if (producto != null)
                        {
                            producto.Stock += detalle.Cantidad;
                        }
                    }

                    // Eliminar detalles existentes
                    _context.VentaDetalles.RemoveRange(ventaExistente.VentaDetalles);

                    // Agregar nuevos detalles
                    if (productos != null && productos.Length > 0)
                    {
                        for (int i = 0; i < productos.Length; i++)
                        {
                            if (productos[i] > 0 && cantidades[i] > 0)
                            {
                                var detalle = new VentaDetalle
                                {
                                    IdVenta = ventaExistente.IdVenta,
                                    IdProducto = productos[i],
                                    Cantidad = cantidades[i],
                                    PrecioUnitario = precios[i]
                                };
                                _context.VentaDetalles.Add(detalle);

                                // Actualizar stock
                                var producto = await _context.Productos.FindAsync(productos[i]);
                                if (producto != null)
                                {
                                    if (producto.Stock >= cantidades[i])
                                    {
                                        producto.Stock -= cantidades[i];
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", $"Stock insuficiente para el producto: {producto.NombreProducto}");
                                        await RecargarDatosViewBag();
                                        return View(venta);
                                    }
                                }
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

            await RecargarDatosViewBag();
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
                .Include(v => v.VentaDetalles)
                    .ThenInclude(vd => vd.IdProductoNavigation)
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
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.VentaDetalles)
                    .FirstOrDefaultAsync(v => v.IdVenta == id);

                if (venta != null)
                {
                    // Restaurar stock antes de eliminar
                    foreach (var detalle in venta.VentaDetalles)
                    {
                        var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                        if (producto != null)
                        {
                            producto.Stock += detalle.Cantidad;
                        }
                    }

                    // Eliminar detalles primero
                    _context.VentaDetalles.RemoveRange(venta.VentaDetalles);
                    _context.Ventas.Remove(venta);

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venta eliminada exitosamente";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar la venta: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.IdVenta == id);
        }

        // Método auxiliar para recargar los datos del ViewBag
        private async Task RecargarDatosViewBag()
        {
            var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
            var productos = await _context.Productos.Where(p => p.Stock > 0).ToListAsync();

            ViewBag.Clientes = clientes;
            ViewBag.Productos = productos;
        }

        // Método para obtener información de producto (útil para AJAX)
        [HttpGet]
        public async Task<JsonResult> GetProductoInfo(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                nombre = producto.NombreProducto,
                precio = producto.Precio,
                stock = producto.Stock
            });
        }
    }
}