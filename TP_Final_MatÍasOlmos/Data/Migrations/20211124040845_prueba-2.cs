using Microsoft.EntityFrameworkCore.Migrations;

namespace TP_Final_MatÍasOlmos.Data.Migrations
{
    public partial class prueba2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "marcaId",
                table: "productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_productos_categoriaId",
                table: "productos",
                column: "categoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_productos_marcaId",
                table: "productos",
                column: "marcaId");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_categorias_categoriaId",
                table: "productos",
                column: "categoriaId",
                principalTable: "categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productos_marcas_marcaId",
                table: "productos",
                column: "marcaId",
                principalTable: "marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_categorias_categoriaId",
                table: "productos");

            migrationBuilder.DropForeignKey(
                name: "FK_productos_marcas_marcaId",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_categoriaId",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_marcaId",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "marcaId",
                table: "productos");
        }
    }
}
