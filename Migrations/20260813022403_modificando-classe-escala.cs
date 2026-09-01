using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace f_backend_gestafe.Migrations
{
    /// <inheritdoc />
    public partial class modificandoclasseescala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_escala_cargo_CargoId",
                table: "escala");

            migrationBuilder.DropColumn(
                name: "hora_fim",
                table: "escala");

            migrationBuilder.RenameColumn(
                name: "hora_inicio",
                table: "escala",
                newName: "hora_salvamento");

            migrationBuilder.RenameColumn(
                name: "data",
                table: "escala",
                newName: "data_salvamento");

            migrationBuilder.RenameColumn(
                name: "CargoId",
                table: "escala",
                newName: "igreja_id");

            migrationBuilder.RenameIndex(
                name: "IX_escala_CargoId",
                table: "escala",
                newName: "IX_escala_igreja_id");

            migrationBuilder.AddColumn<int>(
                name: "IgrejaId1",
                table: "escala",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "configuracoes",
                table: "escala",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_escala_IgrejaId1",
                table: "escala",
                column: "IgrejaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_escala_igreja_IgrejaId1",
                table: "escala",
                column: "IgrejaId1",
                principalTable: "igreja",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_escala_igreja_igreja_id",
                table: "escala",
                column: "igreja_id",
                principalTable: "igreja",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_escala_igreja_IgrejaId1",
                table: "escala");

            migrationBuilder.DropForeignKey(
                name: "FK_escala_igreja_igreja_id",
                table: "escala");

            migrationBuilder.DropIndex(
                name: "IX_escala_IgrejaId1",
                table: "escala");

            migrationBuilder.DropColumn(
                name: "IgrejaId1",
                table: "escala");

            migrationBuilder.DropColumn(
                name: "configuracoes",
                table: "escala");

            migrationBuilder.RenameColumn(
                name: "igreja_id",
                table: "escala",
                newName: "CargoId");

            migrationBuilder.RenameColumn(
                name: "hora_salvamento",
                table: "escala",
                newName: "hora_inicio");

            migrationBuilder.RenameColumn(
                name: "data_salvamento",
                table: "escala",
                newName: "data");

            migrationBuilder.RenameIndex(
                name: "IX_escala_igreja_id",
                table: "escala",
                newName: "IX_escala_CargoId");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "hora_fim",
                table: "escala",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddForeignKey(
                name: "FK_escala_cargo_CargoId",
                table: "escala",
                column: "CargoId",
                principalTable: "cargo",
                principalColumn: "id_cargo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
