namespace f_backend_gestafe.src.Objects.Dtos.Entities
{
    public class UsuarioPatchDTO
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }

        // Chaves estrangeiras
        public int? IdIgreja { get; set; }
        public int? IdTipoUsuario { get; set; }
    }
}
