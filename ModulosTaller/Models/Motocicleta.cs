using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Motocicleta
{
    public int IdMoto { get; set; }

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public string Placa { get; set; } = null!;

    public int? IdCliente { get; set; }

    public virtual ICollection<Agendamiento> Agendamientos { get; set; } = new List<Agendamiento>();

    public virtual Cliente? IdClienteNavigation { get; set; }
}
