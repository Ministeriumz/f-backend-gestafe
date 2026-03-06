namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class LogDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Acao { get; set; }
        public int? IdUsuario { get; set; }
    }
}
