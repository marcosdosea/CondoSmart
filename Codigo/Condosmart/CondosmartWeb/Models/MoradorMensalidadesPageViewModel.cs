namespace CondosmartWeb.Models;

public class MoradorMensalidadesPageViewModel
{
    public string NomeMorador { get; set; } = string.Empty;

    public string Condominio { get; set; } = string.Empty;

    public string Unidade { get; set; } = string.Empty;

    public List<MensalidadeViewModel> Mensalidades { get; set; } = new();
}
