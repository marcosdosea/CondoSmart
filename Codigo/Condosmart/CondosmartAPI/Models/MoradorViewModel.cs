using System.ComponentModel.DataAnnotations;

namespace CondosmartAPI.Models;

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
    [Display(Name = "CPF")]
    public string Cpf { get; set; } = null!;

    [StringLength(12)]
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
    [Display(Name = "CEP")]
    public string? Cep { get; set; }

    [StringLength(60)]
    [Display(Name = "Cidade")]
    public string? Cidade { get; set; }

    [StringLength(2)]
    [Display(Name = "UF")]
    public string? Uf { get; set; }

    [StringLength(80)]
    [Display(Name = "E-mail")]
    public string? Email { get; set; }

    [StringLength(11)]
    [Display(Name = "Telefone")]
    public string? Telefone { get; set; }

    [Display(Name = "Condomínio")]
    public int? CondominioId { get; set; }
}
