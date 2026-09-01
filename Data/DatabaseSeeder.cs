using System.Data;
using f_backend_gestafe.Objects.Enums;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Security;
using Microsoft.EntityFrameworkCore;

namespace f_backend_gestafe.Data;

public static class DatabaseSeeder
{
    public static async Task SeedIfEmptyAsync(
        AppDbContext context,
        IConfiguration configuration,
        ILogger logger)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        if (await HasApplicationDataAsync(context))
        {
            logger.LogInformation("Seed ignorado: o banco de dados já possui dados.");
            return;
        }

        var tiposUsuario = new[]
        {
            new TipoUsuario { Nome = "Superadministrador", NivelAcesso = NivelAcesso.SuperAdministrador },
            new TipoUsuario { Nome = "Administrador da Igreja", NivelAcesso = NivelAcesso.AdmIgreja },
            new TipoUsuario { Nome = "Colaborador da Igreja", NivelAcesso = NivelAcesso.ColaboradorIgreja },
            new TipoUsuario { Nome = "Usuário", NivelAcesso = NivelAcesso.Usuario }
        };

        var igreja = new Igreja
        {
            Nome = configuration["DatabaseSeed:Church:Name"] ?? "Igreja GestaFé",
            Cnpj = configuration["DatabaseSeed:Church:Cnpj"] ?? "00.000.000/0001-00",
            Estado = configuration["DatabaseSeed:Church:State"] ?? "SP",
            Rua = configuration["DatabaseSeed:Church:Street"] ?? "Rua Principal",
            Cep = configuration["DatabaseSeed:Church:ZipCode"] ?? "00000-000",
            Numero = configuration["DatabaseSeed:Church:Number"] ?? "S/N"
        };

        var usuarioRoot = new Usuario
        {
            Nome = configuration["DatabaseSeed:RootUser:FirstName"] ?? "Root",
            Sobrenome = configuration["DatabaseSeed:RootUser:LastName"] ?? "GestaFé",
            Telefone = configuration["DatabaseSeed:RootUser:Phone"] ?? "(00) 00000-0000",
            Email = configuration["DatabaseSeed:RootUser:Email"] ?? "root@gestafe.local",
            Senha = PasswordHasher.Hash(
                configuration["DatabaseSeed:RootUser:Password"] ?? "Root@123"),
            Igreja = igreja,
            TipoUsuario = tiposUsuario.Single(tipo => tipo.NivelAcesso == NivelAcesso.SuperAdministrador)
        };

        var configuracoes = new Configuracoes
        {
            Igreja = igreja,
            ConfiguracaoJson = "{}"
        };

        context.TipoUsuario.AddRange(tiposUsuario);
        context.Igrejas.Add(igreja);
        context.Configuracoes.Add(configuracoes);
        context.Usuario.Add(usuarioRoot);

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        logger.LogInformation(
            "Seed inicial concluído. Igreja: {Igreja}; usuário root: {Email}.",
            igreja.Nome,
            usuarioRoot.Email);
    }

    private static async Task<bool> HasApplicationDataAsync(AppDbContext context)
    {
        return await context.Igrejas.AsNoTracking().AnyAsync()
            || await context.TipoUsuario.AsNoTracking().AnyAsync()
            || await context.Usuario.AsNoTracking().AnyAsync()
            || await context.Configuracoes.AsNoTracking().AnyAsync()
            || await context.Cargo.AsNoTracking().AnyAsync()
            || await context.Ministerios.AsNoTracking().AnyAsync()
            || await context.Eventos.AsNoTracking().AnyAsync()
            || await context.Escala.AsNoTracking().AnyAsync()
            || await context.CargosUsuarios.AsNoTracking().AnyAsync()
            || await context.Log.AsNoTracking().AnyAsync();
    }
}
