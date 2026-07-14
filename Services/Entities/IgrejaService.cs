using AutoMapper;
using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Middleware.Exceptions;
using f_backend_gestafe.Objects.Dtos.Entities;
using f_backend_gestafe.Objects.Models;
using f_backend_gestafe.Services.Interfaces;
using System.Text.RegularExpressions;

namespace f_backend_gestafe.Services.Entities
{
    public class IgrejaService : GenericService<Igreja, IgrejaDTO>, IIgrejaService
    {
        private readonly IIgrejaRepository _igrejaRepository;
        private readonly IMapper _mapper;

        // Lista de estados brasileiros (UF) para validação
        private readonly string[] _estadosValidos =
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
            "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
            "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        public IgrejaService(IIgrejaRepository igrejaRepository, IMapper mapper) : base(igrejaRepository, mapper)
        {
            _igrejaRepository = igrejaRepository;
            _mapper = mapper;
        }

        public override async Task Create(IgrejaDTO entityDTO)
        {
            var errors = new List<string>();

            if (entityDTO is null)
            {
                errors.Add("O objeto não pode ser nulo.");
            }
            else
            {
                // Validação do Nome
                if (string.IsNullOrWhiteSpace(entityDTO.Nome))
                {
                    errors.Add("O campo 'Nome' é obrigatório.");
                }
                else if (entityDTO.Nome.Length > 100)
                {
                    errors.Add("O campo 'Nome' deve ter no máximo 100 caracteres.");
                }

                // Validação do CNPJ
                if (string.IsNullOrWhiteSpace(entityDTO.Cnpj))
                {
                    errors.Add("O campo 'Cnpj' é obrigatório.");
                }
                else if (entityDTO.Cnpj.Length > 18)
                {
                    errors.Add("O campo 'Cnpj' deve ter no máximo 18 caracteres.");
                }
                else if (!ValidarCnpj(entityDTO.Cnpj))
                {
                    errors.Add("O 'Cnpj' informado é inválido.");
                }

                // Validação do Estado
                if (string.IsNullOrWhiteSpace(entityDTO.Estado))
                {
                    errors.Add("O campo 'Estado' é obrigatório.");
                }
                else if (entityDTO.Estado.Length != 2)
                {
                    errors.Add("O campo 'Estado' deve conter exatamente 2 caracteres (Sigla UF).");
                }
                else if (!_estadosValidos.Contains(entityDTO.Estado.ToUpper()))
                {
                    errors.Add("O 'Estado' informado não é uma UF válida no Brasil.");
                }

                // Validação da Rua
                if (string.IsNullOrWhiteSpace(entityDTO.Rua))
                {
                    errors.Add("O campo 'Rua' é obrigatório.");
                }
                else if (entityDTO.Rua.Length > 100)
                {
                    errors.Add("O campo 'Rua' deve ter no máximo 100 caracteres.");
                }

                // Validação do CEP
                if (string.IsNullOrWhiteSpace(entityDTO.Cep))
                {
                    errors.Add("O campo 'Cep' é obrigatório.");
                }
                else if (entityDTO.Cep.Length > 10)
                {
                    errors.Add("O campo 'Cep' deve ter no máximo 10 caracteres.");
                }
                else if (!Regex.IsMatch(entityDTO.Cep, @"^\d{5}-?\d{3}$"))
                {
                    errors.Add("O 'Cep' informado não possui um formato válido (Ex: 00000-000 ou 00000000).");
                }

                // Validação do Numero
                if (string.IsNullOrWhiteSpace(entityDTO.Numero))
                {
                    errors.Add("O campo 'Numero' é obrigatório.");
                }
                else if (entityDTO.Numero.Length > 20)
                {
                    errors.Add("O campo 'Numero' deve ter no máximo 20 caracteres.");
                }
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var entity = _mapper.Map<Igreja>(entityDTO);
            await _igrejaRepository.Add(entity);
        }

        // Método auxiliar para calcular e validar CNPJ
        private bool ValidarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;

            // Remove formatação
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14) return false;

            // Verifica se todos os dígitos são iguais (ex: 11.111.111/1111-11)
            if (new string(cnpj[0], 14) == cnpj) return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

    }
}
