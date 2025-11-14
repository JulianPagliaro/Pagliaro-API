using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class migracion131122hs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editores_Paises_PaisId",
                table: "Editores");

            migrationBuilder.DropForeignKey(
                name: "FK_Estudios_Paises_PaisId",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Estudios_PaisId",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Editores_PaisId",
                table: "Editores");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Estudios");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Editores");

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_IdPais",
                table: "Estudios",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_Editores_IdPais",
                table: "Editores",
                column: "IdPais");

            migrationBuilder.AddForeignKey(
                name: "FK_Editores_Paises_IdPais",
                table: "Editores",
                column: "IdPais",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudios_Paises_IdPais",
                table: "Estudios",
                column: "IdPais",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editores_Paises_IdPais",
                table: "Editores");

            migrationBuilder.DropForeignKey(
                name: "FK_Estudios_Paises_IdPais",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Estudios_IdPais",
                table: "Estudios");

            migrationBuilder.DropIndex(
                name: "IX_Editores_IdPais",
                table: "Editores");

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Estudios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaisId",
                table: "Editores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Estudios_PaisId",
                table: "Estudios",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Editores_PaisId",
                table: "Editores",
                column: "PaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Editores_Paises_PaisId",
                table: "Editores",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudios_Paises_PaisId",
                table: "Estudios",
                column: "PaisId",
                principalTable: "Paises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
