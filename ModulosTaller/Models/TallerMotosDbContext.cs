using Microsoft.EntityFrameworkCore;

namespace ModulosTaller.Models;

public partial class TallerMotosDbContext : DbContext
{
    public TallerMotosDbContext() { }

    public TallerMotosDbContext(DbContextOptions<TallerMotosDbContext> options)
        : base(options) { }

    public virtual DbSet<Agendamiento> Agendamientos { get; set; }
    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Compra> Compras { get; set; }
    public virtual DbSet<CompraDetalle> CompraDetalles { get; set; }
    public virtual DbSet<Horario> Horarios { get; set; }
    public virtual DbSet<Motocicleta> Motocicletas { get; set; }
    public virtual DbSet<Permiso> Permisos { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    public virtual DbSet<Proveedore> Proveedores { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Venta> Ventas { get; set; }
    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }
    public virtual DbSet<RolPermiso> RolPermisos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamiento>(entity =>
        {
            entity.HasKey(e => e.IdAgendamiento);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra);
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCompraDetalle);
            entity.Property(e => e.PrecioUnitario).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);
        });

        modelBuilder.Entity<Motocicleta>(entity =>
        {
            entity.HasKey(e => e.IdMoto);
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso);
            entity.Property(e => e.NombrePermiso)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);
            entity.Property(e => e.Precio).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);
            entity.Property(e => e.NombreRol)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300);
            entity.Property(e => e.Activo)
                .HasDefaultValue(true);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdVentaDetalle);
            entity.Property(e => e.PrecioUnitario).HasPrecision(10, 2);
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => new { e.IdRol, e.IdPermiso });
            entity.ToTable("RolPermiso");

            entity.HasOne(e => e.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(e => e.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(e => e.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(e => e.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}