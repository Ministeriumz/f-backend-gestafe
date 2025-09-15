using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace f_backend_gestafe.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "igreja",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    rua = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_igreja", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_usuario",
                columns: table => new
                {
                    id_tipo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_usuario", x => x.id_tipo);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sobrenome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telefone = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    senha = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_igreja = table.Column<int>(type: "integer", nullable: false),
                    id_tipo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_usuario_igreja_id_igreja",
                        column: x => x.id_igreja,
                        principalTable: "igreja",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_tipo_usuario_id_tipo",
                        column: x => x.id_tipo,
                        principalTable: "tipo_usuario",
                        principalColumn: "id_tipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_usuario_id_igreja",
                table: "usuario",
                column: "id_igreja");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_id_tipo",
                table: "usuario",
                column: "id_tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "igreja");

            migrationBuilder.DropTable(
                name: "tipo_usuario");
        }
    }
}
