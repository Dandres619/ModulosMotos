using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModulosTaller.Migrations
{
    /// <inheritdoc />
    public partial class ajustemodeloVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Ventas");
        }
    }
}
