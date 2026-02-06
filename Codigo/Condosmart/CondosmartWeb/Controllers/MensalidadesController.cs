using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class MensalidadesController : Controller
    {
        private readonly IMensalidadeService _mensalidadeService;
        private readonly IPagamentoService _pagamentoService;
        private readonly IMapper _mapper;

        public MensalidadesController(
            IMensalidadeService mensalidadeService,
            IPagamentoService pagamentoService,
            IMapper mapper)
        {
            _mensalidadeService = mensalidadeService;
            _pagamentoService = pagamentoService;
            _mapper = mapper;
        }

        // GET: Mensalidades
        public ActionResult Index(string? status)
        {
            var lista = _mensalidadeService.GetAll();

            // Aplicar filtro de status se fornecido
            if (!string.IsNullOrEmpty(status))
            {
                lista = lista.Where(m => m.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var listaVm = _mapper.Map<List<MensalidadeViewModel>>(lista);
            return View(listaVm);
        }

        // GET: Mensalidades/Details/5
        public ActionResult Details(int id)
        {
            var mensalidade = _mensalidadeService.GetById(id);
            if (mensalidade == null) return NotFound();

            var mensalidadeVm = _mapper.Map<MensalidadeViewModel>(mensalidade);
            return View(mensalidadeVm);
        }

        // GET: Mensalidades/Pagar/5
        public ActionResult Pagar(int id)
        {
            var mensalidade = _mensalidadeService.GetById(id);
            if (mensalidade == null) return NotFound();

            // Verificar se pode pagar
            if (mensalidade.PagamentoId.HasValue || 
                (mensalidade.Status != "pendente" && mensalidade.Status != "vencida"))
            {
                TempData["Erro"] = "Esta mensalidade não pode ser paga.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var pagarVm = new PagarMensalidadeViewModel
            {
                MensalidadeId = mensalidade.Id,
                Valor = mensalidade.Valor,
                Competencia = mensalidade.Competencia,
                Vencimento = mensalidade.Vencimento,
                CondominioNome = mensalidade.Condominio?.Nome,
                UnidadeIdentificador = mensalidade.Unidade?.Identificador,
                MoradorNome = mensalidade.Morador?.Nome
            };

            return View(pagarVm);
        }

        // POST: Mensalidades/Pagar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pagar(PagarMensalidadeViewModel pagarVm)
        {
            if (!ModelState.IsValid)
            {
                return View(pagarVm);
            }

            var mensalidade = _mensalidadeService.GetById(pagarVm.MensalidadeId);
            if (mensalidade == null) return NotFound();

            // Verificar novamente se pode pagar
            if (mensalidade.PagamentoId.HasValue || 
                (mensalidade.Status != "pendente" && mensalidade.Status != "vencida"))
            {
                TempData["Erro"] = "Esta mensalidade não pode ser paga.";
                return RedirectToAction(nameof(Details), new { id = pagarVm.MensalidadeId });
            }

            // Criar o pagamento
            var pagamento = new Pagamento
            {
                MoradorId = mensalidade.MoradorId,
                UnidadeId = mensalidade.UnidadeId,
                CondominioId = mensalidade.CondominioId,
                FormaPagamento = pagarVm.FormaPagamento,
                Status = "pendente",
                Valor = pagarVm.Valor,
                DataPagamento = DateTime.Now
            };

            _pagamentoService.Create(pagamento);

            // Atualizar a mensalidade
            mensalidade.PagamentoId = pagamento.Id;
            mensalidade.Status = "pago";
            _mensalidadeService.Edit(mensalidade);

            TempData["Sucesso"] = "Pagamento registrado com sucesso!";
            return RedirectToAction(nameof(Comprovante), new { id = pagarVm.MensalidadeId });
        }

        // GET: Mensalidades/Comprovante/5
        public ActionResult Comprovante(int id)
        {
            var mensalidade = _mensalidadeService.GetById(id);
            if (mensalidade == null) return NotFound();

            if (!mensalidade.PagamentoId.HasValue)
            {
                TempData["Erro"] = "Esta mensalidade ainda não possui pagamento registrado.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var pagamento = _pagamentoService.GetById(mensalidade.PagamentoId.Value);
            if (pagamento == null) return NotFound();

            var comprovanteVm = new ComprovantePagamentoViewModel
            {
                MensalidadeId = mensalidade.Id,
                Competencia = mensalidade.Competencia,
                Vencimento = mensalidade.Vencimento,
                ValorMensalidade = mensalidade.Valor,
                CondominioNome = mensalidade.Condominio?.Nome,
                UnidadeIdentificador = mensalidade.Unidade?.Identificador,
                MoradorNome = mensalidade.Morador?.Nome,
                PagamentoId = pagamento.Id,
                FormaPagamento = pagamento.FormaPagamento,
                StatusPagamento = pagamento.Status,
                ValorPago = pagamento.Valor,
                DataPagamento = pagamento.DataPagamento,
                DataRegistro = pagamento.CreatedAt
            };

            return View(comprovanteVm);
        }
    }
}
