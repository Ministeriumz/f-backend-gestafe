namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class EventosDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Resumo { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora_inicio { get; set; }
        public TimeOnly Hora_fim { get; set; }
    }
}
