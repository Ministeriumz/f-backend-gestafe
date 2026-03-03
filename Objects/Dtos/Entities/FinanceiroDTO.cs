using f_backend_gestafe.Objects.Enums;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class FinanceiroDTO
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string Acao { get; set; }
        public DateTime Data { get; set; }
        public StatusFinanceiro Status { get; set; }
        public int IgrejaId { get; set; }
    }
}
