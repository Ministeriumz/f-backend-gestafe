namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class EscalaDTO
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
        public int CargoId { get; set; }
    }
}
