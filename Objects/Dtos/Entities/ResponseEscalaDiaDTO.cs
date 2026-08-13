namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class ResponseEscalaDiaDTO
    {
        public DateTime Data { get; set; }
        public string DiaDaSemana { get; set; }
        public List<AlocacaoDTO> Alocacoes { get; set; } = new List<AlocacaoDTO>();
    }
}
