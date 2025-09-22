using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Agendamiento> Agendamientos { get; set; } = new List<Agendamiento>();

    public virtual ICollection<Motocicleta> Motocicleta { get; set; } = new List<Motocicleta>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
