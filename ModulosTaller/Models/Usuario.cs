using System;

namespace ModulosTaller.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string TipoDocumento { get; set; } = string.Empty;

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string Barrio { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public string Clave { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public int? IdRol { get; set; }

    public bool Activo { get; set; } = true;
    public virtual Role? IdRolNavigation { get; set; }
}