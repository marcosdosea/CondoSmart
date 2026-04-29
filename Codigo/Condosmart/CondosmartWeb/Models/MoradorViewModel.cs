using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class MoradorViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome não pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CPF deve conter somente números (11 dígitos).")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = null!;

        [StringLength(9, ErrorMessage = "RG deve ter no máximo 9 caracteres (somente números).")]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "RG deve conter somente números.")]
        [Display(Name = "RG")]
        public string? Rg { get; set; }

        [StringLength(80)]
        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [StringLength(60)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(10)]
        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [StringLength(40)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [StringLength(8)]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "CEP deve conter somente números (8 dígitos).")]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [StringLength(60)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [StringLength(2)]
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "UF deve conter somente letras.")]
        [Display(Name = "UF")]
        public string? Uf { get; set; }

        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(80)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(11, MinimumLength = 8, ErrorMessage = "Telefone deve ter entre 8 e 11 dígitos.")]
        [RegularExpression("^[0-9]{8,11}$", ErrorMessage = "Telefone deve conter somente números (8 a 11 dígitos).")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O condomínio é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condomínio válido")]
        [Display(Name = "Condomínio")]
        public int? CondominioId { get; set; }
    }
}
