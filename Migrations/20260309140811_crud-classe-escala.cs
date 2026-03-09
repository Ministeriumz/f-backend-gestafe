using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace f_backend_gestafe.Migrations
{
    /// <inheritdoc />
    public partial class crudclasseescala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargo",
                columns: table => new
                {
                    id_cargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargo", x => x.id_cargo);
                });

            migrationBuilder.CreateTable(
                name: "eventos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    resumo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    data = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    hora_fim = table.Column<TimeOnly>(type: "time without time zone", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventos", x => x.id);
                });

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
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hora = table.Column<TimeSpan>(type: "interval", nullable: false),
                    acao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_usuario = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ministerio",
                columns: table => new
                {
                    id_ministerio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tamanho_max = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ministerio", x => x.id_ministerio);
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
                name: "escala",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    hora_fim = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CargoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_escala", x => x.id);
                    table.ForeignKey(
                        name: "FK_escala_cargo_CargoId",
                        column: x => x.CargoId,
                        principalTable: "cargo",
                        principalColumn: "id_cargo",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "financeiro",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    valor = table.Column<decimal>(type: "numeric", nullable: false),
                    acao = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    IgrejaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financeiro", x => x.id);
                    table.ForeignKey(
                        name: "FK_financeiro_igreja_IgrejaId",
                        column: x => x.IgrejaId,
                        principalTable: "igreja",
                        principalColumn: "id");
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

            migrationBuilder.CreateTable(
                name: "cargos_usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdCargo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargos_usuario", x => new { x.IdUsuario, x.IdCargo });
                    table.ForeignKey(
                        name: "FK_cargos_usuario_cargo_IdCargo",
                        column: x => x.IdCargo,
                        principalTable: "cargo",
                        principalColumn: "id_cargo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cargos_usuario_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cargos_usuario_IdCargo",
                table: "cargos_usuario",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_escala_CargoId",
                table: "escala",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_financeiro_IgrejaId",
                table: "financeiro",
                column: "IgrejaId");

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
                name: "cargos_usuario");

            migrationBuilder.DropTable(
                name: "configuracoes");

            migrationBuilder.DropTable(
                name: "escala");

            migrationBuilder.DropTable(
                name: "eventos");

            migrationBuilder.DropTable(
                name: "financeiro");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "ministerio");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "cargo");

            migrationBuilder.DropTable(
                name: "igreja");

            migrationBuilder.DropTable(
                name: "tipo_usuario");
        }
    }
}
