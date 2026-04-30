using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models;

public class FiltroMensalidadeViewModel
{
    public int? CondominioId { get; set; }

    public int? UnidadeId { get; set; }

    public string? Status { get; set; }

    [Range(1, 12, ErrorMessage = "Informe um mes valido.")]
    public int? MesCompetencia { get; set; }

    [Range(2000, 2100, ErrorMessage = "Informe um ano valido.")]
    public int? AnoCompetencia { get; set; }

    [Range(1, 9999, ErrorMessage = "Informe uma pagina valida.")]
    public int Page { get; set; } = 1;

    [Range(5, 100, ErrorMessage = "O tamanho da pagina deve ficar entre 5 e 100.")]
    public int PageSize { get; set; } = 10;
}
