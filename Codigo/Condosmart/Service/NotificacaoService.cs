using Core.Data;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly CondosmartContext _context;

        public NotificacaoService(CondosmartContext context)
        {
            _context = context;
        }

        public void Criar(string usuarioEmail, string usuarioNome, string titulo, string mensagem, string tipo = "info", int? condominioId = null, string? urlDestino = null)
        {
            var notificacao = new NotificacaoSistema
            {
                UsuarioEmail = usuarioEmail,
                UsuarioNome = usuarioNome,
                Titulo = titulo,
                Mensagem = mensagem,
                Tipo = tipo,
                CondominioId = condominioId,
                UrlDestino = urlDestino
            };

            _context.NotificacoesSistema.Add(notificacao);
            _context.SaveChanges();
        }

        public List<NotificacaoSistema> ListarRecentes(int? condominioId, int take = 15)
        {
            var query = _context.NotificacoesSistema
                .AsNoTracking()
                .Include(n => n.Condominio)
                .OrderByDescending(n => n.CreatedAt)
                .AsQueryable();

            if (condominioId.HasValue)
                query = query.Where(n => n.CondominioId == condominioId.Value);

            return query.Take(take).ToList();
        }

        public int Contar(int? condominioId)
        {
            var query = _context.NotificacoesSistema.AsNoTracking().AsQueryable();
            if (condominioId.HasValue)
                query = query.Where(n => n.CondominioId == condominioId.Value);

            return query.Count();
        }

        public void Remover(int id)
        {
            var entidade = _context.NotificacoesSistema.Find(id);
            if (entidade is null)
                return;

            _context.NotificacoesSistema.Remove(entidade);
            _context.SaveChanges();
        }

        public void LimparPorCondominio(int? condominioId)
        {
            var query = _context.NotificacoesSistema.AsQueryable();
            if (condominioId.HasValue)
                query = query.Where(n => n.CondominioId == condominioId.Value);

            var itens = query.ToList();
            if (itens.Count == 0)
                return;

            _context.NotificacoesSistema.RemoveRange(itens);
            _context.SaveChanges();
        }
    }
}
