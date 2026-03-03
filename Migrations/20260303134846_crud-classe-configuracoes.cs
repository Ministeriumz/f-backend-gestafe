using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace f_backend_gestafe.Migrations
{
    /// <inheritdoc />
    public partial class crudclasseconfiguracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuracoes",
                columns: table => new
                {
                    igreja_id = table.Column<int>(type: "integer", nullable: false),
                    configuracoes = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuracoes", x => x.igreja_id);
                    table.ForeignKey(
                        name: "FK_configuracoes_igreja_igreja_id",
                        column: x => x.igreja_id,
                        principalTable: "igreja",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuracoes");
        }
    }
}
