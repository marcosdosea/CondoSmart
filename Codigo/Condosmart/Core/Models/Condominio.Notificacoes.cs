namespace Core.Models;

public partial class Condominio
{
    public virtual ICollection<NotificacaoSistema> NotificacoesSistema { get; set; } = new List<NotificacaoSistema>();
}
