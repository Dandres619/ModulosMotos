namespace ModulosTaller.Models;

public class RolPermiso
{
    public int IdRol { get; set; }
    public int IdPermiso { get; set; }

    public Role Rol { get; set; }
    public Permiso Permiso { get; set; }
}