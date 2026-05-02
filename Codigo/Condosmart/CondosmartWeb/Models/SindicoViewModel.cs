using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class SindicoViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome e obrigatorio")]
        [StringLength(80, ErrorMessage = "O nome nao pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF e obrigatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve conter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CPF deve conter somente numeros (11 digitos).")]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone deve ter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Telefone deve conter exatamente 11 numeros.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
        [StringLength(80)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(80)]
        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [StringLength(60)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(10)]
        [Display(Name = "Numero")]
        public string? Numero { get; set; }

        [StringLength(40)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [StringLength(8)]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "CEP deve conter somente numeros (8 digitos).")]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [StringLength(60)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [StringLength(2)]
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "UF deve conter somente letras.")]
        [Display(Name = "UF")]
        public string? Uf { get; set; }
    }
}
