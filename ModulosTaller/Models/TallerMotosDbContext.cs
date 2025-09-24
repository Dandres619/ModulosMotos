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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamiento>(entity =>
        {
            entity.HasKey(e => e.IdAgendamiento);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Descripcion).HasMaxLength(200);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Agendamientos)
                .HasForeignKey(d => d.IdCliente);

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Agendamientos)
                .HasForeignKey(d => d.IdHorario);

            entity.HasOne(d => d.IdMotoNavigation).WithMany(p => p.Agendamientos)
                .HasForeignKey(d => d.IdMoto);
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.SegundoNombre)
                .HasMaxLength(50);

            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(50);

            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(100);

            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Estado)
                .HasDefaultValue(true);

            // Índice único para documento
            entity.HasIndex(e => e.Documento, "UQ__Clientes__Documento")
                .IsUnique();
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra);
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor);
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCompraDetalle);
            entity.ToTable("CompraDetalle");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.IdCompra);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.IdProducto);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);
            entity.Property(e => e.DiaSemana).HasMaxLength(20);
        });

        modelBuilder.Entity<Motocicleta>(entity =>
        {
            entity.HasKey(e => e.IdMoto);
            entity.HasIndex(e => e.Placa).IsUnique();
            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Placa).HasMaxLength(20);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Motocicleta)
                .HasForeignKey(d => d.IdCliente);
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso);
            entity.Property(e => e.NombrePermiso).HasMaxLength(100);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria);
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);
            entity.Property(e => e.NombreProveedor).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);
            entity.Property(e => e.NombreRol).HasMaxLength(50);
            entity.Property(e => e.Activo).HasDefaultValue(true);

            // Relación muchos-a-muchos con Permisos
            entity.HasMany(r => r.Permisos)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany().HasForeignKey("IdPermiso").OnDelete(DeleteBehavior.ClientSetNull),
                    p => p.HasOne<Role>().WithMany().HasForeignKey("IdRol").OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("IdRol", "IdPermiso");
                        j.ToTable("RolPermiso");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.HasIndex(e => e.Correo, "UQ__Usuarios__Correo").IsUnique();
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Clave).HasMaxLength(200);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente);

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCompra);
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdVentaDetalle);
            entity.ToTable("VentaDetalle");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdProducto);

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdVenta);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
