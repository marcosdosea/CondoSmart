using Core;
using Core.Data;
using Core.Models;
using Core.Service;
using Core.DTO; 
using Microsoft.EntityFrameworkCore;
using System; 
using System.Linq; 

namespace Service
{
    /// <summary>
    /// Serviço para gerenciar os pagamentos de condomínio
    /// </summary>
    public class PagamentoService : IPagamentoService
    {
        private readonly CondosmartContext context;

        public PagamentoService(CondosmartContext context)
        {
            this.context = context;
        }

        // =======================================================================
        // INÍCIO DA NOVA FUNCIONALIDADE (NÃO-CRUD) - Exigência do Professor
        // =======================================================================

        /// <summary>
        /// Processa a liquidação de uma mensalidade, calculando juros e multas por atraso.
        /// </summary>
        /// <param name="dto">Dados seguros vindos da requisição</param>
        public void LiquidarMensalidade(LiquidarMensalidadeDTO dto)
        {
            // 1. Busca a mensalidade no banco de dados
            var mensalidade = context.Mensalidades
                .FirstOrDefault(m => m.Id == dto.MensalidadeId);

            // 2. Validações Iniciais (Tratamento de Exceções)
            if (mensalidade == null)
                throw new ArgumentException("Erro: Mensalidade não encontrada no sistema.");

            if (mensalidade.Status?.ToUpper() == "PAGO")
                throw new ArgumentException("Aviso: Esta mensalidade já encontra-se quitada.");

            // 3. O Cálculo "Não-CRUD" (Multa e Juros)
            decimal valorExigido = mensalidade.Valor;

            if (dto.DataPagamento.Date > mensalidade.Vencimento.Date)
            {
                int diasAtraso = (dto.DataPagamento.Date - mensalidade.Vencimento.Date).Days;

                decimal multa = mensalidade.Valor * 0.02m; // Multa fixa de 2%
                decimal juros = mensalidade.Valor * 0.00033m * diasAtraso; // Juros de aprox 1% ao mês

                valorExigido += (multa + juros);
                valorExigido = Math.Round(valorExigido, 2); // Evita dízimas financeiras
            }

            // 4. Validação da Regra de Negócio
            if (dto.ValorPago < valorExigido)
            {
                throw new ArgumentException($"Valor insuficiente. O total atualizado com os encargos é de {valorExigido:C2}.");
            }

            // 5. Criação do Registro de Pagamento
            var pagamento = new Pagamento
            {
                CondominioId = mensalidade.CondominioId,
                UnidadeId = mensalidade.UnidadeId,
                MoradorId = mensalidade.MoradorId,
                FormaPagamento = dto.FormaPagamento,
                Valor = dto.ValorPago,
                DataPagamento = dto.DataPagamento,
                Status = "CONCLUIDO",
                CreatedAt = DateTime.Now
            };

            // 6. Atualização de Status e Persistência
            mensalidade.Status = "PAGO";
            context.Pagamentos.Add(pagamento);

            context.SaveChanges();

            // Vincula o pagamento à mensalidade e salva novamente
            mensalidade.PagamentoId = pagamento.Id;
            context.SaveChanges();
        }

        // =======================================================================
        // MÉTODOS CRUD ORIGINAIS (Mantidos intactos)
        // =======================================================================

        /// <summary>
        /// Criar um novo pagamento na base de dados
        /// </summary>
        /// <param name="pagamento">dados do pagamento</param>
        /// <returns>id do pagamento</returns>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Pagamento pagamento)
        {
            ValidarPagamento(pagamento);

            context.Add(pagamento);
            context.SaveChanges();
            return pagamento.Id;
        }

        /// <summary>
        /// Editar dados do pagamento na base de dados
        /// </summary>
        /// <param name="pagamento">dados do pagamento</param>
        /// <exception cref="ArgumentException"></exception>
        public void Edit(Pagamento pagamento)
        {
            ValidarPagamento(pagamento);

            context.Update(pagamento);
            context.SaveChanges();
        }

        /// <summary>
        /// Remover o pagamento da base de dados
        /// </summary>
        /// <param name="id">identificador do pagamento</param>
        public void Delete(int id)
        {
            var pagamento = GetById(id);
            if (pagamento != null)
            {
                context.Remove(pagamento);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Obter um pagamento pelo seu identificador
        /// </summary>
        /// <param name="id">identificador do pagamento</param>
        /// <returns>dados do pagamento ou nulo</returns>
        public Pagamento? GetById(int id)
        {
            return context.Pagamentos?
                .Include(p => p.Condominio)
                .Include(p => p.Morador)
                .Include(p => p.Unidade)
                .FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Obter a lista com todos os pagamentos
        /// </summary>
        /// <returns>lista de pagamentos</returns>
        public List<Pagamento> GetAll()
        {
            return context.Pagamentos?
                .Include(p => p.Condominio)
                .Include(p => p.Morador)
                .Include(p => p.Unidade)
                .ToList() ?? new List<Pagamento>();
        }

        /// <summary>
        /// Validar os dados do pagamento
        /// </summary>
        /// <param name="pagamento">dados do pagamento</param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidarPagamento(Pagamento pagamento)
        {
            if (pagamento == null)
                throw new ArgumentException("O pagamento não pode ser nulo");

            if (string.IsNullOrWhiteSpace(pagamento.FormaPagamento))
                throw new ArgumentException("A forma de pagamento é obrigatória");

            if (string.IsNullOrWhiteSpace(pagamento.Status))
                throw new ArgumentException("O status é obrigatório");

            if (pagamento.Valor <= 0)
                throw new ArgumentException("O valor do pagamento deve ser maior que zero");

            if (pagamento.CondominioId <= 0)
                throw new ArgumentException("O identificador do condomínio é obrigatório");
        }
    }
}