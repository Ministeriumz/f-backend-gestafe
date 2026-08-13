namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class RequestEscalaDTO
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        // Lista de dias permitidos (0 = Domingo, 1 = Segunda, ..., 6 = Sábado)
        // Se vier vazio, considera todos os dias entre DataInicio e DataFim.
        public List<DayOfWeek> DiasDaSemana { get; set; } = new List<DayOfWeek>();
    }
}
