namespace CondosmartWeb.Models;

public class MoradorDashboardViewModel
{
    public int MoradorId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefone { get; set; }

    public string Condominio { get; set; } = "-";

    public string Unidade { get; set; } = "-";

    public int MensalidadesPendentes { get; set; }

    public List<MoradorMensalidadeResumoViewModel> Mensalidades { get; set; } = new();

    public List<MoradorComunicadoResumoViewModel> Comunicados { get; set; } = new();
}

public class MoradorMensalidadeResumoViewModel
{
    public string Competencia { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;

    public string Vencimento { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public class MoradorComunicadoResumoViewModel
{
    public string Titulo { get; set; } = string.Empty;

    public string Data { get; set; } = string.Empty;

    public string Resumo { get; set; } = string.Empty;
}
