namespace Core.DTO
{
    public class ProvisionamentoMoradorResultDTO
    {
        public int MoradorId { get; set; }

        public string NomeMorador { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string SenhaTemporaria { get; set; } = string.Empty;

        public string Condominio { get; set; } = string.Empty;

        public string Unidade { get; set; } = string.Empty;

        public string UrlAcesso { get; set; } = string.Empty;

        public bool EmailEnviado { get; set; }

        public string? ObservacaoEmail { get; set; }
    }
}
