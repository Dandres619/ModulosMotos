using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int? IdCategoria { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual CategoriaProducto? IdCategoriaNavigation { get; set; }

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
