using f_backend_gestafe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace f_backend_gestafe.Migrations;

/// <summary>
/// Aligns the database column used by Escala.CargoId with the application's
/// snake_case schema convention.
/// </summary>
[DbContext(typeof(AppDbContext))]
[Migration("20260831130000_padronizar-coluna-cargo-escala")]
public partial class PadronizarColunaCargoEscala : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Older installations can have either column name. The conditional
        // migration preserves the existing id_cargo column when it is already
        // correct and renames the legacy quoted CargoId column when necessary.
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = current_schema()
                      AND table_name = 'escala'
                      AND column_name = 'CargoId'
                ) AND NOT EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = current_schema()
                      AND table_name = 'escala'
                      AND column_name = 'id_cargo'
                ) THEN
                    ALTER TABLE escala RENAME COLUMN "CargoId" TO id_cargo;
                END IF;
            END $$;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = current_schema()
                      AND table_name = 'escala'
                      AND column_name = 'id_cargo'
                ) AND NOT EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = current_schema()
                      AND table_name = 'escala'
                      AND column_name = 'CargoId'
                ) THEN
                    ALTER TABLE escala RENAME COLUMN id_cargo TO "CargoId";
                END IF;
            END $$;
            """);
    }
}
