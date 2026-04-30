using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Models;

public class MensalidadesAdminPageViewModel
{
    public ConfiguracaoMensalidadeViewModel Configuracao { get; set; } = new();

    public GerarParcelasMensalidadeViewModel Geracao { get; set; } = new();

    public FiltroMensalidadeViewModel Filtro { get; set; } = new();

    public PagedListViewModel<MensalidadeViewModel> Mensalidades { get; set; } = PagedListViewModel<MensalidadeViewModel>.Create([], 1, 10);

    public List<ConfiguracaoMensalidadeResumoViewModel> Configuracoes { get; set; } = new();

    public List<SelectListItem> Condominios { get; set; } = new();

    public List<SelectListItem> Unidades { get; set; } = new();
}

public class ConfiguracaoMensalidadeResumoViewModel
{
    public string Condominio { get; set; } = string.Empty;

    public decimal ValorMensalidade { get; set; }

    public int DiaVencimento { get; set; }

    public int QuantidadeParcelasPadrao { get; set; }

    public bool Ativa { get; set; }
}
