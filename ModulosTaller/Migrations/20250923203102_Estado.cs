using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModulosTaller.Migrations
{
    /// <inheritdoc />
    public partial class Estado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaProductos",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__A3C02A1030306DE5", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__D594664212B0B659", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    IdHorario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaSemana = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: true),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Horarios__1539229B0F26AE6A", x => x.IdHorario);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePermiso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permisos__0D626EC88C70CD31", x => x.IdPermiso);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__E8B631AF3DEC2399", x => x.IdProveedor);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__2A49584C9AE85A0A", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__09889210FAD937BE", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK__Productos__IdCat__45F365D3",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriaProductos",
                        principalColumn: "IdCategoria");
                });

            migrationBuilder.CreateTable(
                name: "Motocicletas",
                columns: table => new
                {
                    IdMoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Motocicl__33CED5FBEDA1A059", x => x.IdMoto);
                    table.ForeignKey(
                        name: "FK__Motocicle__IdCli__5535A963",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCompra = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdProveedor = table.Column<int>(type: "int", nullable: true),
                    EstaAnulada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Compras__0A5CDB5C716AAA18", x => x.IdCompra);
                    table.ForeignKey(
                        name: "FK__Compras__IdProve__4BAC3F29",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RolPermi__BA9F7EA063D4DD87", x => new { x.IdRol, x.IdPermiso });
                    table.ForeignKey(
                        name: "FK__RolPermis__IdPer__3C69FB99",
                        column: x => x.IdPermiso,
                        principalTable: "Permisos",
                        principalColumn: "IdPermiso");
                    table.ForeignKey(
                        name: "FK__RolPermis__IdRol__3B75D760",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__5B65BF97AE0A9458", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK__Usuarios__IdRol__403A8C7D",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateTable(
                name: "Agendamientos",
                columns: table => new
                {
                    IdAgendamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    IdMoto = table.Column<int>(type: "int", nullable: true),
                    IdHorario = table.Column<int>(type: "int", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Agendami__8516393DC4B8E90B", x => x.IdAgendamiento);
                    table.ForeignKey(
                        name: "FK__Agendamie__IdCli__59FA5E80",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK__Agendamie__IdHor__5BE2A6F2",
                        column: x => x.IdHorario,
                        principalTable: "Horarios",
                        principalColumn: "IdHorario");
                    table.ForeignKey(
                        name: "FK__Agendamie__IdMot__5AEE82B9",
                        column: x => x.IdMoto,
                        principalTable: "Motocicletas",
                        principalColumn: "IdMoto");
                });

            migrationBuilder.CreateTable(
                name: "CompraDetalle",
                columns: table => new
                {
                    IdCompraDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompra = table.Column<int>(type: "int", nullable: true),
                    IdProducto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CompraDe__A1B840C5C2BC30EF", x => x.IdCompraDetalle);
                    table.ForeignKey(
                        name: "FK__CompraDet__IdCom__4E88ABD4",
                        column: x => x.IdCompra,
                        principalTable: "Compras",
                        principalColumn: "IdCompra");
                    table.ForeignKey(
                        name: "FK__CompraDet__IdPro__4F7CD00D",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaVenta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    IdCompra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas__BC1240BDFC61093A", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK__Ventas__IdClient__5FB337D6",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK__Ventas__IdCompra__60A75C0F",
                        column: x => x.IdCompra,
                        principalTable: "Compras",
                        principalColumn: "IdCompra");
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalle",
                columns: table => new
                {
                    IdVentaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<int>(type: "int", nullable: true),
                    IdProducto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VentaDet__2787211D41EF6E96", x => x.IdVentaDetalle);
                    table.ForeignKey(
                        name: "FK__VentaDeta__IdPro__6477ECF3",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto");
                    table.ForeignKey(
                        name: "FK__VentaDeta__IdVen__6383C8BA",
                        column: x => x.IdVenta,
                        principalTable: "Ventas",
                        principalColumn: "IdVenta");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamientos_IdCliente",
                table: "Agendamientos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamientos_IdHorario",
                table: "Agendamientos",
                column: "IdHorario");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamientos_IdMoto",
                table: "Agendamientos",
                column: "IdMoto");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDetalle_IdCompra",
                table: "CompraDetalle",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDetalle_IdProducto",
                table: "CompraDetalle",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_IdProveedor",
                table: "Compras",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Motocicletas_IdCliente",
                table: "Motocicletas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "UQ__Motocicl__8310F99D2178C770",
                table: "Motocicletas",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdCategoria",
                table: "Productos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_IdPermiso",
                table: "RolPermiso",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__60695A19A7DCDF4A",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalle_IdProducto",
                table: "VentaDetalle",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalle_IdVenta",
                table: "VentaDetalle",
                column: "IdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdCliente",
                table: "Ventas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdCompra",
                table: "Ventas",
                column: "IdCompra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendamientos");

            migrationBuilder.DropTable(
                name: "CompraDetalle");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "VentaDetalle");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Motocicletas");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "CategoriaProductos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Proveedores");
        }
    }
}
