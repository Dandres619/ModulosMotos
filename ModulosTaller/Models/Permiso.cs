using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModulosTaller.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    [Required(ErrorMessage = "El nombre del permiso es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre del permiso no puede superar los 100 caracteres.")]
    public string NombrePermiso { get; set; } = string.Empty;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    // ✅ Relación explícita con RolPermiso
    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}