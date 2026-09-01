using System.Text.Json;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class EscalaDTO
    {
        public int Id { get; set; }
        public int IgrejaId { get; set; }
        public DateOnly DataSalvamento { get; set; }
        public TimeOnly HoraSalvamento { get; set; }
        public string EscalaJson { get; set; }
    }
}