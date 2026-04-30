using Core.Data;
using Core.DTO;
using Core.Models;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class MensalidadeService : IMensalidadeService
    {
        private readonly CondosmartContext context;

        public MensalidadeService(CondosmartContext context)
        {
            this.context = context;
        }

        public int Create(Mensalidade mensalidade)
        {
            PrepararMensalidade(mensalidade);
            ValidarMensalidade(mensalidade);

            context.Add(mensalidade);
            context.SaveChanges();
            return mensalidade.Id;
        }

        public void Edit(Mensalidade mensalidade)
        {
            PrepararMensalidade(mensalidade);
            ValidarMensalidade(mensalidade);

            context.Update(mensalidade);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var mensalidade = GetById(id);
            if (mensalidade != null)
            {
                context.Remove(mensalidade);
                context.SaveChanges();
            }
        }

        public Mensalidade? GetById(int id)
        {
            AtualizarStatusAutomaticamente();

            return BaseQuery()
                .FirstOrDefault(m => m.Id == id);
        }

        public List<Mensalidade> GetAll()
        {
            AtualizarStatusAutomaticamente();
            return BaseQuery().ToList();
        }

        public List<Mensalidade> GetByMorador(int moradorId)
        {
            AtualizarStatusAutomaticamente();

            return BaseQuery()
                .Where(m => m.MoradorId == moradorId)
                .OrderByDescending(m => m.Competencia)
                .ToList();
        }

        public List<Mensalidade> Filtrar(int? condominioId, int? unidadeId, string? status, int? mesCompetencia, int? anoCompetencia)
        {
            AtualizarStatusAutomaticamente();

            var query = BaseQuery().AsQueryable();

            if (condominioId.HasValue && condominioId.Value > 0)
                query = query.Where(m => m.CondominioId == condominioId.Value);

            if (unidadeId.HasValue && unidadeId.Value > 0)
                query = query.Where(m => m.UnidadeId == unidadeId.Value);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(m => m.Status == status);

            if (mesCompetencia.HasValue && mesCompetencia.Value > 0)
                query = query.Where(m => m.Competencia.Month == mesCompetencia.Value);

            if (anoCompetencia.HasValue && anoCompetencia.Value > 0)
                query = query.Where(m => m.Competencia.Year == anoCompetencia.Value);

            return query
                .OrderByDescending(m => m.Competencia)
                .ThenBy(m => m.Unidade.Identificador)
                .ToList();
        }

        public List<ConfiguracaoMensalidade> GetConfiguracoes()
        {
            return context.ConfiguracaoMensalidades
                .Include(c => c.Condominio)
                .OrderBy(c => c.Condominio.Nome)
                .ToList();
        }

        public ConfiguracaoMensalidade? GetConfiguracaoAtiva(int condominioId)
        {
            return context.ConfiguracaoMensalidades
                .Include(c => c.Condominio)
                .FirstOrDefault(c => c.CondominioId == condominioId && c.Ativa);
        }

        public void SalvarConfiguracao(ConfiguracaoMensalidade configuracao)
        {
            ValidarConfiguracao(configuracao);

            var existente = context.ConfiguracaoMensalidades
                .FirstOrDefault(c => c.CondominioId == configuracao.CondominioId);

            if (existente is null)
            {
                context.ConfiguracaoMensalidades.Add(configuracao);
            }
            else
            {
                existente.ValorMensalidade = configuracao.ValorMensalidade;
                existente.DiaVencimento = configuracao.DiaVencimento;
                existente.QuantidadeParcelasPadrao = configuracao.QuantidadeParcelasPadrao;
                existente.Ativa = configuracao.Ativa;
            }

            context.SaveChanges();
        }

        public GeracaoParcelasResultDTO GerarParcelasEmLote(int condominioId, int anoReferencia, int quantidadeParcelas)
        {
            if (anoReferencia < 2000 || anoReferencia > 2100)
                throw new ArgumentException("Informe um ano de referencia valido.");

            if (quantidadeParcelas < 1 || quantidadeParcelas > 12)
                throw new ArgumentException("A quantidade de parcelas deve ficar entre 1 e 12.");

            var configuracao = GetConfiguracaoAtiva(condominioId);
            if (configuracao is null)
                throw new ArgumentException("Cadastre a configuracao de mensalidade do condominio antes de gerar parcelas.");

            var unidades = context.UnidadesResidenciais
                .Where(u => u.CondominioId == condominioId && u.MoradorId.HasValue)
                .OrderBy(u => u.Identificador)
                .ToList();

            if (unidades.Count == 0)
                throw new ArgumentException("Nao existem unidades com morador vinculado neste condominio.");

            var parcelasGeradas = 0;
            var parcelasIgnoradas = 0;

            for (var mes = 1; mes <= quantidadeParcelas; mes++)
            {
                var competencia = new DateTime(anoReferencia, mes, 1);
                var diaVencimento = Math.Min(configuracao.DiaVencimento, DateTime.DaysInMonth(anoReferencia, mes));
                var vencimento = new DateTime(anoReferencia, mes, diaVencimento);

                foreach (var unidade in unidades)
                {
                    var existe = context.Mensalidades.Any(m =>
                        m.UnidadeId == unidade.Id &&
                        m.CondominioId == condominioId &&
                        m.Competencia == competencia);

                    if (existe)
                    {
                        parcelasIgnoradas++;
                        continue;
                    }

                    var status = vencimento.Date < DateTime.Today ? "atrasado" : "pendente";
                    var mensalidade = new Mensalidade
                    {
                        UnidadeId = unidade.Id,
                        MoradorId = unidade.MoradorId,
                        CondominioId = condominioId,
                        Competencia = competencia,
                        Vencimento = vencimento,
                        ValorOriginal = configuracao.ValorMensalidade,
                        ValorFinal = configuracao.ValorMensalidade,
                        Valor = configuracao.ValorMensalidade,
                        Status = status
                    };

                    context.Mensalidades.Add(mensalidade);
                    parcelasGeradas++;
                }
            }

            context.SaveChanges();

            return new GeracaoParcelasResultDTO
            {
                CondominioId = condominioId,
                AnoReferencia = anoReferencia,
                QuantidadeParcelasSolicitada = quantidadeParcelas,
                ParcelasGeradas = parcelasGeradas,
                ParcelasIgnoradas = parcelasIgnoradas
            };
        }

        private IQueryable<Mensalidade> BaseQuery()
        {
            return context.Mensalidades
                .Include(m => m.Condominio)
                .Include(m => m.Morador)
                .Include(m => m.Unidade)
                .Include(m => m.Pagamento)
                .AsNoTracking();
        }

        private void AtualizarStatusAutomaticamente()
        {
            var pendentesVencidas = context.Mensalidades
                .Where(m => (m.Status == "pendente" || m.Status == "vencida") && m.Vencimento < DateTime.Today)
                .ToList();

            if (pendentesVencidas.Count == 0)
                return;

            foreach (var mensalidade in pendentesVencidas)
                mensalidade.Status = "atrasado";

            context.SaveChanges();
        }

        private static void PrepararMensalidade(Mensalidade mensalidade)
        {
            if (mensalidade.ValorOriginal <= 0 && mensalidade.Valor > 0)
                mensalidade.ValorOriginal = mensalidade.Valor;

            if (mensalidade.ValorFinal <= 0 && mensalidade.ValorOriginal > 0)
                mensalidade.ValorFinal = mensalidade.ValorOriginal;

            mensalidade.Valor = mensalidade.ValorFinal;

            if (string.IsNullOrWhiteSpace(mensalidade.Status))
                mensalidade.Status = "pendente";
        }

        private static void ValidarMensalidade(Mensalidade mensalidade)
        {
            if (mensalidade == null)
                throw new ArgumentException("A mensalidade nao pode ser nula.");

            if (mensalidade.ValorOriginal <= 0)
                throw new ArgumentException("O valor original da mensalidade deve ser maior que zero.");

            if (mensalidade.ValorFinal <= 0)
                throw new ArgumentException("O valor final da mensalidade deve ser maior que zero.");

            if (mensalidade.UnidadeId <= 0)
                throw new ArgumentException("O identificador da unidade e obrigatorio.");

            if (mensalidade.CondominioId <= 0)
                throw new ArgumentException("O identificador do condominio e obrigatorio.");

            if (string.IsNullOrWhiteSpace(mensalidade.Status))
                throw new ArgumentException("O status e obrigatorio.");

            if (mensalidade.Competencia == default)
                throw new ArgumentException("A competencia e obrigatoria.");

            if (mensalidade.Vencimento == default)
                throw new ArgumentException("A data de vencimento e obrigatoria.");
        }

        private static void ValidarConfiguracao(ConfiguracaoMensalidade configuracao)
        {
            if (configuracao.CondominioId <= 0)
                throw new ArgumentException("Selecione um condominio valido.");

            if (configuracao.ValorMensalidade <= 0)
                throw new ArgumentException("O valor da mensalidade deve ser maior que zero.");

            if (configuracao.DiaVencimento < 1 || configuracao.DiaVencimento > 28)
                throw new ArgumentException("O dia de vencimento deve ficar entre 1 e 28.");

            if (configuracao.QuantidadeParcelasPadrao < 1 || configuracao.QuantidadeParcelasPadrao > 12)
                throw new ArgumentException("A quantidade padrao de parcelas deve ficar entre 1 e 12.");
        }
    }
}
