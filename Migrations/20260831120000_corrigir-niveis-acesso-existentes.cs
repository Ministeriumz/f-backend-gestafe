using f_backend_gestafe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace f_backend_gestafe.Migrations;

/// <summary>
/// Corrects records created when nivel_acesso used 0 as its database default.
/// Unknown legacy names are intentionally assigned the least privileged level.
/// </summary>
[DbContext(typeof(AppDbContext))]
[Migration("20260831120000_corrigir-niveis-acesso-existentes")]
public partial class CorrigirNiveisAcessoExistentes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            UPDATE tipo_usuario
            SET nivel_acesso = CASE lower(trim(nome))
                WHEN 'superadministrador' THEN 0
                WHEN 'super administrador' THEN 0
                WHEN 'superadm' THEN 0
                WHEN 'administrador' THEN 0
                WHEN 'administrador da igreja' THEN 1
                WHEN 'adm igreja' THEN 1
                WHEN 'colaborador da igreja' THEN 2
                WHEN 'usuário' THEN 3
                WHEN 'usuario' THEN 3
                ELSE 3
            END;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // The previous migration used 0 as the default for all records. The
        // former values cannot be inferred safely, so this data correction is
        // intentionally not reversed.
    }
}
