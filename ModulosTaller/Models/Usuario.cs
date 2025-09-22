using System;
using System.Collections.Generic;

namespace ModulosTaller.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public int? IdRol { get; set; }

    public virtual Role? IdRolNavigation { get; set; }
}
