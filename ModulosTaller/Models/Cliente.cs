using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string TipoDocumento { get; set; } = null!; // Nuevo campo: "CC", "CE", "NIT", etc.

    public string Documento { get; set; } = null!; // Nuevo campo: número de documento

    public string PrimerNombre { get; set; } = null!; // Nuevo campo

    public string? SegundoNombre { get; set; } // Nuevo campo (opcional)

    public string PrimerApellido { get; set; } = null!; // Nuevo campo

    public string? SegundoApellido { get; set; } // Nuevo campo (opcional)

    public string Direccion { get; set; } = null!; // Nuevo campo

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now; // Nuevo campo

    public bool Estado { get; set; } = true; // Nuevo campo: activo/inactivo

    public virtual ICollection<Agendamiento> Agendamientos { get; set; } = new List<Agendamiento>();

    public virtual ICollection<Motocicleta> Motocicleta { get; set; } = new List<Motocicleta>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    // Propiedad de solo lectura para nombre completo
    public string NombreCompleto => $"{PrimerNombre} {SegundoNombre ?? ""} {PrimerApellido} {SegundoApellido ?? ""}".Trim().Replace("  ", " ");
}