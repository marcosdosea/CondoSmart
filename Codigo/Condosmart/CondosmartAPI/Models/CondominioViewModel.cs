using System.ComponentModel.DataAnnotations;

namespace CondosmartAPI.Models;

public class CondominioViewModel
{
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(80)]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo CNPJ é obrigatório")]
    [StringLength(14)]
    [Display(Name = "CNPJ")]
    public string Cnpj { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "Telefone")]
    [Phone]
    public string? Telefone { get; set; }

    [Display(Name = "Rua")]
    [StringLength(80)]
    public string? Rua { get; set; }

    [Display(Name = "Número")]
    [StringLength(10)]
    public string? Numero { get; set; }

    [Display(Name = "Bairro")]
    [StringLength(60)]
    public string? Bairro { get; set; }

    [Display(Name = "Cidade")]
    [StringLength(60)]
    public string? Cidade { get; set; }

    [Display(Name = "UF")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [Display(Name = "CEP")]
    [StringLength(8)]
    public string? Cep { get; set; }

    [Display(Name = "Síndico")]
    public int? SindicoId { get; set; }

    [Display(Name = "Criado em")]
    public DateTime? CreatedAt { get; set; }
}
