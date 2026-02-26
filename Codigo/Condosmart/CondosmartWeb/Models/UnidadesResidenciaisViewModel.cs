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

        [StringLength(100)]
        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [StringLength(10)]
        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [StringLength(100)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(100)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [StringLength(100)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "UF")]
        public string? Uf { get; set; }

        [StringLength(11, MinimumLength = 8, ErrorMessage = "Telefone residencial deve ter entre 8 e 11 dígitos (somente números).")]
        [Display(Name = "Telefone Residencial")]
        public string? TelefoneResidencial { get; set; }

        [StringLength(11, MinimumLength = 8, ErrorMessage = "Telefone celular deve ter entre 8 e 11 dígitos (somente números).")]
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

