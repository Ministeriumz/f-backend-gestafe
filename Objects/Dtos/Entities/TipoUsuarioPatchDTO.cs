using f_backend_gestafe.Objects.Enums;

namespace f_backend_gestafe.Objects.Dtos.Entities
{
    public class TipoUsuarioPatchDTO
    {
        public string? Nome { get; set; }
        public NivelAcesso? NivelAcesso { get; set; }
    }
}
