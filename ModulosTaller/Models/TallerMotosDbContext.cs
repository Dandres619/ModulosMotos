using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ModulosTaller.Models;

public partial class TallerMotosDbContext : DbContext
{
    public TallerMotosDbContext()
    {
    }

    public TallerMotosDbContext(DbContextOptions<TallerMotosDbContext> options)
        : base(options)
    {
    }

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
            entity.HasKey(e => e.IdAgendamiento).HasName("PK__Agendami__8516393DC4B8E90B");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Fecha).HasColumnType("datetime");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Agendamientos)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Agendamie__IdCli__59FA5E80");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Agendamientos)
                .HasForeignKey(d => d.IdHorario)
                .HasConstraintName("FK__Agendamie__IdHor__5BE2A6F2");

            entity.HasOne(d => d.IdMotoNavigation).WithMany(p => p.Agendamientos)
                .HasForeignKey(d => d.IdMoto)
                .HasConstraintName("FK__Agendamie__IdMot__5AEE82B9");
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A1030306DE5");
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D594664212B0B659");

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
            entity.HasKey(e => e.IdCompra).HasName("PK__Compras__0A5CDB5C716AAA18");
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__Compras__IdProve__4BAC3F29");
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCompraDetalle).HasName("PK__CompraDe__A1B840C5C2BC30EF");
            entity.ToTable("CompraDetalle");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__CompraDet__IdCom__4E88ABD4");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__CompraDet__IdPro__4F7CD00D");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__1539229B0F26AE6A");
            entity.Property(e => e.DiaSemana).HasMaxLength(20);
        });

        modelBuilder.Entity<Motocicleta>(entity =>
        {
            entity.HasKey(e => e.IdMoto).HasName("PK__Motocicl__33CED5FBEDA1A059");
            entity.HasIndex(e => e.Placa, "UQ__Motocicl__8310F99D2178C770").IsUnique();
            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Placa).HasMaxLength(20);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Motocicleta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Motocicle__IdCli__5535A963");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permisos__0D626EC88C70CD31");
            entity.Property(e => e.NombrePermiso).HasMaxLength(100);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210FAD937BE");
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK__Productos__IdCat__45F365D3");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF3DEC2399");
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.NombreProveedor).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C9AE85A0A");
            entity.Property(e => e.NombreRol).HasMaxLength(50);

            entity.HasMany(d => d.IdPermisos).WithMany(p => p.IdRols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("IdPermiso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__IdPer__3C69FB99"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__IdRol__3B75D760"),
                    j =>
                    {
                        j.HasKey("IdRol", "IdPermiso").HasName("PK__RolPermi__BA9F7EA063D4DD87");
                        j.ToTable("RolPermiso");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97AE0A9458");
            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19A7DCDF4A").IsUnique();
            entity.Property(e => e.Clave).HasMaxLength(200);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__403A8C7D");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Ventas__BC1240BDFC61093A");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Ventas__IdClient__5FB337D6");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__Ventas__IdCompra__60A75C0F");
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdVentaDetalle).HasName("PK__VentaDet__2787211D41EF6E96");
            entity.ToTable("VentaDetalle");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__VentaDeta__IdPro__6477ECF3");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__VentaDeta__IdVen__6383C8BA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}