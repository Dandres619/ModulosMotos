using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;
using System.Drawing.Printing;

namespace ModulosTaller.Controllers
{
    public class ComprasController : Controller
    {
        private readonly TallerMotosDbContext _context;
        private const int Paginacion = 2; 

        public ComprasController(TallerMotosDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var comprasQuery = _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.IdProductoNavigation)
                        .ThenInclude(p => p.IdCategoriaNavigation)
                .OrderByDescending(c => c.FechaCompra);

            int totalCompras = await comprasQuery.CountAsync();
            var compras = await comprasQuery
                .Skip((page - 1) * Paginacion)
                .Take(Paginacion)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCompras / (double)Paginacion);

            return View(compras);
        }

        [HttpPost]
        public IActionResult Anular(int id)
        {
            var compra = _context.Compras.Find(id);
            if (compra == null)
                return NotFound();

            compra.EstaAnulada = true;
            _context.Update(compra);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewData["Proveedores"] = _context.Proveedores.ToList();
            ViewData["Categorias"] = _context.CategoriaProductos.ToList();

            return View();
        }


        public async Task<IActionResult> Details(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.CompraDetalles)
                    .ThenInclude(cd => cd.IdProductoNavigation)
                        .ThenInclude(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(c => c.IdCompra == id);

            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraViewModel model)
        {
            if (!ModelState.IsValid || model.Productos == null || !model.Productos.Any())
            {
                ViewData["Proveedores"] = _context.Proveedores.ToList();
                ViewData["Categorias"] = _context.CategoriaProductos.ToList();
                ModelState.AddModelError("", "Debe agregar al menos un producto.");
                return View(model);
            }

            foreach (var p in model.Productos)
            {
                if ((p.IdCategoria == null || p.IdCategoria == 0) && string.IsNullOrWhiteSpace(p.NombreCategoria))
                {
                    ModelState.AddModelError("", "Debe seleccionar o escribir una categoría para cada producto.");
                    ViewData["Proveedores"] = _context.Proveedores.ToList();
                    ViewData["Categorias"] = _context.CategoriaProductos.ToList();
                    return View(model);
                }
            }

            var compra = new Compra
            {
                FechaCompra = DateTime.Now,
                IdProveedor = model.IdProveedor,
                EstaAnulada = false
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            foreach (var p in model.Productos)
            {
                int categoriaId = 0;
                if (p.IdCategoria.HasValue && p.IdCategoria.Value > 0)
                {
                    // Buscar la categoría existente por Id
                    var categoriaExistente = await _context.CategoriaProductos
                        .FirstOrDefaultAsync(c => c.IdCategoria == p.IdCategoria.Value);

                    if (categoriaExistente == null)
                    {
                        ModelState.AddModelError("", "La categoría seleccionada no existe.");
                        ViewData["Proveedores"] = _context.Proveedores.ToList();
                        ViewData["Categorias"] = _context.CategoriaProductos.ToList();
                        return View(model);
                    }
                    categoriaId = categoriaExistente.IdCategoria;
                }
                else
                {
                    // Si se escribe una nueva categoría, buscar si ya existe por nombre
                    var nombreCat = p.NombreCategoria.Trim();
                    var existente = await _context.CategoriaProductos
                        .FirstOrDefaultAsync(c => c.NombreCategoria == nombreCat);

                    if (existente != null)
                    {
                        categoriaId = existente.IdCategoria;
                    }
                    else
                    {
                        var nuevaCat = new CategoriaProducto
                        {
                            NombreCategoria = nombreCat
                        };
                        _context.CategoriaProductos.Add(nuevaCat);
                        await _context.SaveChangesAsync();
                        categoriaId = nuevaCat.IdCategoria;
                    }
                }

                var producto = new Producto
                {
                    NombreProducto = p.NombreProducto,
                    Precio = p.Precio,
                    Stock = p.Cantidad,
                    IdCategoria = categoriaId
                };
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                var detalle = new CompraDetalle
                {
                    IdCompra = compra.IdCompra,
                    IdProducto = producto.IdProducto,
                    Cantidad = p.Cantidad,
                    PrecioUnitario = p.Precio
                };
                _context.CompraDetalles.Add(detalle);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

    public class CompraViewModel
    {
        public int IdProveedor { get; set; }
        public List<ProductoViewModel> Productos { get; set; } = new();
    }

    public class ProductoViewModel
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public int? IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }

}
