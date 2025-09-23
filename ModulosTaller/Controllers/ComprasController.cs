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

        // GET: Compras
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




        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["Proveedores"] = _context.Proveedores.ToList();
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


        // POST: Compras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CompraViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Crear la compra
                var compra = new Compra
                {
                    FechaCompra = DateTime.Now,
                    IdProveedor = model.IdProveedor
                };

                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();

                foreach (var prod in model.Productos)
                {
                    // Buscar o crear categoría
                    var categoria = await _context.CategoriaProductos
                        .FirstOrDefaultAsync(c => c.NombreCategoria == prod.NombreCategoria);

                    if (categoria == null)
                    {
                        categoria = new CategoriaProducto
                        {
                            NombreCategoria = prod.NombreCategoria
                        };
                        _context.CategoriaProductos.Add(categoria);
                        await _context.SaveChangesAsync();
                    }

                    // Crear producto
                    var producto = new Producto
                    {
                        NombreProducto = prod.NombreProducto,
                        Precio = prod.Precio,
                        Stock = prod.Cantidad,
                        IdCategoria = categoria.IdCategoria
                    };
                    _context.Productos.Add(producto);
                    await _context.SaveChangesAsync();

                    // Crear detalle de compra
                    var detalle = new CompraDetalle
                    {
                        IdCompra = compra.IdCompra,
                        IdProducto = producto.IdProducto,
                        Cantidad = prod.Cantidad,
                        PrecioUnitario = prod.Precio
                    };
                    _context.CompraDetalles.Add(detalle);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Proveedores"] = _context.Proveedores.ToList();
            return View(model);
        }
    }

    // ViewModel para la creación de compra
    public class CompraViewModel
    {
        public int IdProveedor { get; set; }
        public List<ProductoViewModel> Productos { get; set; } = new();
    }

    public class ProductoViewModel
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
