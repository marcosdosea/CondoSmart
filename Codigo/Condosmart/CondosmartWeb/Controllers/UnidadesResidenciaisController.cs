using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CondosmartWeb.Controllers
{
    public class UnidadesResidenciaisController : Controller
    {
        private readonly IUnidadesResidenciaisService _service;
        private readonly ICondominioService _condominioService;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory? _httpFactory;

        public UnidadesResidenciaisController(
            IUnidadesResidenciaisService service,
            ICondominioService condominioService,
            IMapper mapper,
            IHttpClientFactory? httpFactory = null)
        {
            _service = service;
            _condominioService = condominioService;
            _mapper = mapper;
            _httpFactory = httpFactory;
        }

        private bool ValidateCep(UnidadeResidencialViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Cep))
            {
                ModelState.AddModelError(nameof(vm.Cep), "CEP é obrigatório.");
                return false;
            }

            var digits = new string(vm.Cep.Where(char.IsDigit).ToArray());
            if (digits.Length != 8)
            {
                ModelState.AddModelError(nameof(vm.Cep), "CEP deve conter 8 dígitos.");
                return false;
            }
            // se IHttpClientFactory não disponível (testes), apenas validar formato
            if (_httpFactory == null) return true;

            var client = _httpFactory.CreateClient();
            try
            {
                var resp = client.GetAsync($"https://viacep.com.br/ws/{digits}/json/").Result;
                if (!resp.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(nameof(vm.Cep), "Não foi possível validar o CEP no serviço externo.");
                    return false;
                }

                var content = resp.Content.ReadAsStringAsync().Result;
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;
                if (root.TryGetProperty("erro", out var err) && err.GetBoolean())
                {
                    ModelState.AddModelError(nameof(vm.Cep), "CEP não encontrado.");
                    return false;
                }

                return true;
            }
            catch
            {
                ModelState.AddModelError(nameof(vm.Cep), "Erro ao validar CEP (serviço indisponível).");
                return false;
            }
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<UnidadeResidencialViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        public IActionResult Create()
        {
            PopularDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            // valida CEP via ViaCEP (serviço interno)
            if (!ValidateCep(vm))
            {
                PopularDropdowns();
                return View(vm);
            }

            try
            {
                var entity = _mapper.Map<UnidadesResidenciais>(vm);
                _service.Create(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.Identificador), ex.Message);
                PopularDropdowns();
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            PopularDropdowns();
            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UnidadeResidencialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopularDropdowns();
                return View(vm);
            }

            if (!ValidateCep(vm))
            {
                PopularDropdowns();
                return View(vm);
            }

            try
            {
                var entity = _mapper.Map<UnidadesResidenciais>(vm);
                _service.Edit(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(vm.Identificador), ex.Message);
                PopularDropdowns();
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null)
                return NotFound();

            return View(_mapper.Map<UnidadeResidencialViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopularDropdowns()
        {
            var condominios = _condominioService.GetAll();
            ViewBag.Condominios = new SelectList(condominios, "Id", "Nome");

            // Embutir dados de endereço dos condomínios para evitar chamadas AJAX
            var dados = condominios.Select(c => new
            {
                id = c.Id,
                rua = c.Rua,
                numero = c.Numero,
                bairro = c.Bairro,
                complemento = c.Complemento,
                cidade = c.Cidade,
                uf = c.Uf,
                cep = c.Cep
            }).ToList();

            ViewBag.CondominiosData = System.Text.Json.JsonSerializer.Serialize(dados);
        }
    }
}

