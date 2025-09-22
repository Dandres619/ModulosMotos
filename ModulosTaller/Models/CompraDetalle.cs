using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class CompraDetalle
{
    public int IdCompraDetalle { get; set; }

    public int? IdCompra { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
