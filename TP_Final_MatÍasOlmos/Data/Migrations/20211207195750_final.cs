using Microsoft.EntityFrameworkCore.Migrations;

namespace TP_Final_MatÍasOlmos.Data.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "proveedorId",
                table: "productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_productos_proveedorId",
                table: "productos",
                column: "proveedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_proveedores_proveedorId",
                table: "productos",
                column: "proveedorId",
                principalTable: "proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_productos_proveedores_proveedorId",
            //    table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_proveedorId",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "proveedorId",
                table: "productos");

            migrationBuilder.CreateTable(
                name: "ProductoProveedor",
                columns: table => new
                {
                    productosId = table.Column<int>(type: "int", nullable: false),
                    proveedoresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoProveedor", x => new { x.productosId, x.proveedoresId });
                    table.ForeignKey(
                        name: "FK_ProductoProveedor_productos_productosId",
                        column: x => x.productosId,
                        principalTable: "productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoProveedor_proveedores_proveedoresId",
                        column: x => x.proveedoresId,
                        principalTable: "proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoProveedor_proveedoresId",
                table: "ProductoProveedor",
                column: "proveedoresId");
        }
    }
}
