using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracion1510 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneroId",
                table: "Juegos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdGenero",
                table: "Juegos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Juegos_GeneroId",
                table: "Juegos",
                column: "GeneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Juegos_Generos_GeneroId",
                table: "Juegos",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Juegos_Generos_GeneroId",
                table: "Juegos");

            migrationBuilder.DropIndex(
                name: "IX_Juegos_GeneroId",
                table: "Juegos");

            migrationBuilder.DropColumn(
                name: "GeneroId",
                table: "Juegos");

            migrationBuilder.DropColumn(
                name: "IdGenero",
                table: "Juegos");
        }
    }
}
