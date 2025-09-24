using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModulosTaller.Models;

public partial class Role
{
    public int IdRol { get; set; }

    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre del rol no puede superar los 100 caracteres.")]
    public string NombreRol { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [MaxLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    // ✅ Relación muchos-a-muchos con permisos
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}