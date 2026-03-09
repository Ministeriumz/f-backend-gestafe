namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
        public int UsuarioId { get; set; }
        public string Email { get; set; }
    }
}
