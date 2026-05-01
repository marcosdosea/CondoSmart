using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class CondominioViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome e obrigatorio")]
        [StringLength(80, ErrorMessage = "O nome nao pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O CNPJ e obrigatorio")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "CNPJ deve ter 14 caracteres.")]
        [RegularExpression("^[0-9]{14}$", ErrorMessage = "CNPJ deve conter somente numeros (14 digitos).")]
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; } = null!;

        [Required(ErrorMessage = "A Rua e obrigatoria")]
        [StringLength(80)]
        [Display(Name = "Rua")]
        public string Rua { get; set; } = null!;

        [Required(ErrorMessage = "O Numero e obrigatorio")]
        [StringLength(10)]
        [Display(Name = "Numero")]
        public string Numero { get; set; } = null!;

        [Required(ErrorMessage = "O Bairro e obrigatorio")]
        [StringLength(60)]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; } = null!;

        [Required(ErrorMessage = "A Cidade e obrigatoria")]
        [StringLength(60)]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = null!;

        [Required(ErrorMessage = "UF e obrigatorio")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve possuir 2 caracteres.")]
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "UF deve conter somente letras.")]
        [Display(Name = "UF")]
        public string Uf { get; set; } = null!;

        [Required(ErrorMessage = "CEP e obrigatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter 8 caracteres.")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "CEP deve conter somente numeros (8 digitos).")]
        [Display(Name = "CEP")]
        public string Cep { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
        [StringLength(80)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone deve ter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Telefone deve conter exatamente 11 numeros.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [StringLength(40)]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Unidades e obrigatorio")]
        [Display(Name = "Unidades")]
        [Range(1, 100000, ErrorMessage = "Unidades deve ser maior que 0.")]
        public int Unidades { get; set; }
    }
}
