using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class MoradorViewModel
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome e obrigatorio")]
        [StringLength(80, ErrorMessage = "O nome nao pode exceder 80 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O CPF e obrigatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "CPF deve conter somente numeros (11 digitos).")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = null!;

        [StringLength(9, ErrorMessage = "RG deve ter no maximo 9 caracteres (somente numeros).")]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "RG deve conter somente numeros.")]
        [Display(Name = "RG")]
        public string? Rg { get; set; }

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

        [Required(ErrorMessage = "O e-mail e obrigatorio para criar o acesso do morador.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
        [StringLength(80)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefone deve ter 11 digitos.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Telefone deve conter exatamente 11 numeros.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O condominio e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido")]
        [Display(Name = "Condominio")]
        public int? CondominioId { get; set; }

        [Required(ErrorMessage = "Selecione a unidade do morador.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma unidade valida.")]
        [Display(Name = "Unidade")]
        public int? UnidadeId { get; set; }
    }
}
