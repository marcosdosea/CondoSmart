using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class UnidadeResidencialViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O identificador é obrigatório")]
        [StringLength(50, ErrorMessage = "O identificador não pode exceder 50 caracteres.")]
        [Display(Name = "Identificador")]
        public string Identificador { get; set; } = null!;

        [Required(ErrorMessage = "A Rua é obrigatória")]
        [StringLength(100)]
        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [Required(ErrorMessage = "O Número é obrigatório")]
        [StringLength(10)]
        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "O Bairro é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(100)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter 8 caracteres (somente números).")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "CEP deve conter somente números (8 dígitos).")]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [Required(ErrorMessage = "A Cidade é obrigatória")]
        [StringLength(100)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "UF é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve possuir 2 caracteres.")]
        [Display(Name = "UF")]
        public string? Uf { get; set; }

        [StringLength(11, MinimumLength = 8, ErrorMessage = "Telefone residencial deve ter entre 8 e 11 dígitos (somente números).")]
        [RegularExpression("^[0-9]{8,11}$", ErrorMessage = "Telefone residencial deve conter somente números (8 a 11 dígitos).")]
        [Display(Name = "Telefone Residencial")]
        public string? TelefoneResidencial { get; set; }

        [StringLength(11, MinimumLength = 8, ErrorMessage = "Telefone celular deve ter entre 8 e 11 dígitos (somente números).")]
        [RegularExpression("^[0-9]{8,11}$", ErrorMessage = "Telefone celular deve conter somente números (8 a 11 dígitos).")]
        [Display(Name = "Telefone Celular")]
        public string? TelefoneCelular { get; set; }

        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Required(ErrorMessage = "O condomínio é obrigatório")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        // Propriedades de navegação para exibição
        public string? CondominioNome { get; set; }
        public string? MoradorNome { get; set; }
        public string? SindicoNome { get; set; }
    }
}

