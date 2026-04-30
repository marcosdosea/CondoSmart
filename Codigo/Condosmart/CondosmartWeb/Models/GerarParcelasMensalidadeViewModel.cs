using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class GerarParcelasMensalidadeViewModel
{
    [Required(ErrorMessage = "Selecione um condominio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um condominio valido.")]
    public int CondominioId { get; set; }

    [Required(ErrorMessage = "Informe o ano de referencia.")]
    [Range(2000, 2100, ErrorMessage = "Informe um ano de referencia valido.")]
    public int AnoReferencia { get; set; } = DateTime.Today.Year;

    [Required(ErrorMessage = "Informe a quantidade de parcelas.")]
    [Range(1, 12, ErrorMessage = "A quantidade de parcelas deve ficar entre 1 e 12.")]
    public int QuantidadeParcelas { get; set; } = 12;
}
