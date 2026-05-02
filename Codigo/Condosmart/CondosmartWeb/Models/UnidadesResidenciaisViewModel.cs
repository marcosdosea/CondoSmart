using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class UnidadeResidencialViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O identificador e obrigatorio")]
        [StringLength(50, ErrorMessage = "O identificador nao pode exceder 50 caracteres.")]
        [Display(Name = "Identificador")]
        public string Identificador { get; set; } = null!;

        [Required(ErrorMessage = "A Rua e obrigatoria")]
        [StringLength(100)]
        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [Required(ErrorMessage = "O Numero e obrigatorio")]
        [StringLength(10)]
        [Display(Name = "Numero")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "O Bairro e obrigatorio")]
        [StringLength(100)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(100)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O CEP e obrigatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter 8 caracteres (somente numeros).")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "CEP deve conter somente numeros (8 digitos).")]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [Required(ErrorMessage = "A Cidade e obrigatoria")]
        [StringLength(100)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "UF e obrigatorio")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve possuir 2 caracteres.")]
        [Display(Name = "UF")]
        public string? Uf { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone residencial deve ter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Telefone residencial deve conter exatamente 11 numeros.")]
        [Display(Name = "Telefone Residencial")]
        public string? TelefoneResidencial { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone celular deve ter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Telefone celular deve conter exatamente 11 numeros.")]
        [Display(Name = "Telefone Celular")]
        public string? TelefoneCelular { get; set; }

        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Required(ErrorMessage = "O condominio e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido")]
        [Display(Name = "Condominio")]
        public int CondominioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um sindico valido")]
        [Display(Name = "Sindico")]
        public int? SindicoId { get; set; }

        public string? CondominioNome { get; set; }
        public string? MoradorNome { get; set; }
        public string? SindicoNome { get; set; }
    }
}
