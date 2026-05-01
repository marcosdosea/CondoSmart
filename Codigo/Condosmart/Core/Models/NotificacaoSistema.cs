namespace Core.Models;

public class NotificacaoSistema
{
    public int Id { get; set; }

    public int? CondominioId { get; set; }

    public string UsuarioEmail { get; set; } = string.Empty;

    public string UsuarioNome { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Mensagem { get; set; } = string.Empty;

    public string Tipo { get; set; } = "info";

    public string? UrlDestino { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio? Condominio { get; set; }
}
