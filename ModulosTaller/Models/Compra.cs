using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public DateTime FechaCompra { get; set; }

    public int? IdProveedor { get; set; }

    public bool EstaAnulada { get; set; } = false;


    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual Proveedore? IdProveedorNavigation { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
