using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public DateTime FechaVenta { get; set; }

    public int? IdCliente { get; set; }

    public int? IdCompra { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
