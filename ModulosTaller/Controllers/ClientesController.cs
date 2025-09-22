using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;

namespace ModulosTaller.Controllers
{
    public class ClientesController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public ClientesController(TallerMotosDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.Where(c => c.Estado).ToListAsync();
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewBag.TiposDocumento = new List<SelectListItem>
            {
                new SelectListItem { Value = "CC", Text = "Cédula de Ciudadanía" },
                new SelectListItem { Value = "CE", Text = "Cédula de Extranjería" },
                new SelectListItem { Value = "TI", Text = "Tarjeta de Identidad" },
                new SelectListItem { Value = "PAS", Text = "Pasaporte" },
                new SelectListItem { Value = "NIT", Text = "NIT" }
            };

            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // La fecha de registro se asignará automáticamente por el defaultValue en la BD
                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cliente creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Error al crear el cliente: " + ex.Message);
                }
            }

            ViewBag.TiposDocumento = new List<SelectListItem>
            {
                new SelectListItem { Value = "CC", Text = "Cédula de Ciudadanía" },
                new SelectListItem { Value = "CE", Text = "Cédula de Extranjería" },
                new SelectListItem { Value = "TI", Text = "Tarjeta de Identidad" },
                new SelectListItem { Value = "PAS", Text = "Pasaporte" },
                new SelectListItem { Value = "NIT", Text = "NIT" }
            };

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewBag.TiposDocumento = new List<SelectListItem>
            {
                new SelectListItem { Value = "CC", Text = "Cédula de Ciudadanía", Selected = cliente.TipoDocumento == "CC" },
                new SelectListItem { Value = "CE", Text = "Cédula de Extranjería", Selected = cliente.TipoDocumento == "CE" },
                new SelectListItem { Value = "TI", Text = "Tarjeta de Identidad", Selected = cliente.TipoDocumento == "TI" },
                new SelectListItem { Value = "PAS", Text = "Pasaporte", Selected = cliente.TipoDocumento == "PAS" },
                new SelectListItem { Value = "NIT", Text = "NIT", Selected = cliente.TipoDocumento == "NIT" }
            };

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.TiposDocumento = new List<SelectListItem>
            {
                new SelectListItem { Value = "CC", Text = "Cédula de Ciudadanía", Selected = cliente.TipoDocumento == "CC" },
                new SelectListItem { Value = "CE", Text = "Cédula de Extranjería", Selected = cliente.TipoDocumento == "CE" },
                new SelectListItem { Value = "TI", Text = "Tarjeta de Identidad", Selected = cliente.TipoDocumento == "TI" },
                new SelectListItem { Value = "PAS", Text = "Pasaporte", Selected = cliente.TipoDocumento == "PAS" },
                new SelectListItem { Value = "NIT", Text = "NIT", Selected = cliente.TipoDocumento == "NIT" }
            };

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                cliente.Estado = false;
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cliente eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Exportar clientes a Excel
        [HttpPost]
        public async Task<IActionResult> ExportarExcel(string filtroBusqueda, string filtroEstado, string filtroTipoDocumento)
        {
            try
            {
                // Obtener los clientes filtrados
                var query = _context.Clientes.AsQueryable();

                // Aplicar filtros si se proporcionaron
                if (!string.IsNullOrEmpty(filtroBusqueda))
                {
                    query = query.Where(c =>
                        c.PrimerNombre.Contains(filtroBusqueda) ||
                        c.SegundoNombre.Contains(filtroBusqueda) ||
                        c.PrimerApellido.Contains(filtroBusqueda) ||
                        c.SegundoApellido.Contains(filtroBusqueda) ||
                        c.Documento.Contains(filtroBusqueda) ||
                        c.Correo.Contains(filtroBusqueda) ||
                        c.Telefono.Contains(filtroBusqueda));
                }

                if (filtroEstado != "all")
                {
                    bool estadoFiltro = filtroEstado == "active";
                    query = query.Where(c => c.Estado == estadoFiltro);
                }

                if (filtroTipoDocumento != "all")
                {
                    query = query.Where(c => c.TipoDocumento == filtroTipoDocumento);
                }

                var clientes = await query.ToListAsync();

                // Crear el libro de Excel
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Clientes");

                    // Título del reporte
                    worksheet.Cell(1, 1).Value = "Reporte de Clientes - Rafa Motos";
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Range(1, 1, 1, 8).Merge();

                    // Fecha de generación
                    worksheet.Cell(2, 1).Value = $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}";
                    worksheet.Cell(2, 1).Style.Font.Italic = true;
                    worksheet.Range(2, 1, 2, 8).Merge();

                    // Encabezados de columnas
                    var headers = new string[] {
                        "Tipo Documento", "Número Documento", "Primer Nombre", "Segundo Nombre",
                        "Primer Apellido", "Segundo Apellido", "Teléfono", "Correo",
                        "Dirección", "Fecha Registro", "Estado"
                    };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(4, i + 1).Value = headers[i];
                        worksheet.Cell(4, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        worksheet.Cell(4, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    // Datos de clientes
                    int row = 5;
                    foreach (var cliente in clientes)
                    {
                        worksheet.Cell(row, 1).Value = cliente.TipoDocumento;
                        worksheet.Cell(row, 2).Value = cliente.Documento;
                        worksheet.Cell(row, 3).Value = cliente.PrimerNombre;
                        worksheet.Cell(row, 4).Value = cliente.SegundoNombre ?? "";
                        worksheet.Cell(row, 5).Value = cliente.PrimerApellido;
                        worksheet.Cell(row, 6).Value = cliente.SegundoApellido ?? "";
                        worksheet.Cell(row, 7).Value = cliente.Telefono;
                        worksheet.Cell(row, 8).Value = cliente.Correo;
                        worksheet.Cell(row, 9).Value = cliente.Direccion;
                        worksheet.Cell(row, 10).Value = cliente.FechaRegistro;
                        worksheet.Cell(row, 10).Style.NumberFormat.Format = "dd/mm/yyyy";
                        worksheet.Cell(row, 11).Value = cliente.Estado ? "ACTIVO" : "INACTIVO";
                        worksheet.Cell(row, 11).Style.Font.FontColor = cliente.Estado ? XLColor.Green : XLColor.Red;
                        worksheet.Cell(row, 11).Style.Font.Bold = true;

                        row++;
                    }

                    // Ajustar ancho de columnas automáticamente
                    worksheet.Columns().AdjustToContents();

                    // Agregar bordes a toda la tabla de datos
                    var dataRange = worksheet.Range(4, 1, row - 1, headers.Length);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Preparar la respuesta
                    var memoryStream = new MemoryStream();
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;

                    var fileName = $"Clientes_RafaMotos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(memoryStream,
                               "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                               fileName);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al exportar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }
    }
}