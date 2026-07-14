namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int IdIgreja { get; set; }
        public int IdTipoUsuario { get; set; }
    }
}