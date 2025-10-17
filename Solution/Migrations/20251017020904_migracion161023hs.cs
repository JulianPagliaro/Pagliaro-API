using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class migracion161023hs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPlataforma",
                table: "Juegos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlataformaId",
                table: "Juegos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Juegos_PlataformaId",
                table: "Juegos",
                column: "PlataformaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Juegos_Plataformas_PlataformaId",
                table: "Juegos",
                column: "PlataformaId",
                principalTable: "Plataformas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Juegos_Plataformas_PlataformaId",
                table: "Juegos");

            migrationBuilder.DropIndex(
                name: "IX_Juegos_PlataformaId",
                table: "Juegos");

            migrationBuilder.DropColumn(
                name: "IdPlataforma",
                table: "Juegos");

            migrationBuilder.DropColumn(
                name: "PlataformaId",
                table: "Juegos");
        }
    }
}
