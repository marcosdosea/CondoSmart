using AutoMapper;
using CondosmartWeb.Models;
using Core.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace CondosmartWeb.Controllers
{
    public class CondominioController : Controller
    {
        private readonly ICondominioService _service;
        private readonly IMapper _mapper;

        public CondominioController(ICondominioService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lista = _service.GetAll();
            var vms = _mapper.Map<List<CondominioViewModel>>(lista);
            return View(vms);
        }

        public IActionResult Details(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();
            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CondominioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = _mapper.Map<Condominio>(vm);

            // Validar CNPJ localmente e avisar se inválido
            if (!string.IsNullOrWhiteSpace(entity.Cnpj) && !ValidarCnpjLocal(entity.Cnpj))
            {
                ModelState.AddModelError("Cnpj", "Aviso: O CNPJ informado pode ser inválido. Verifique e corrija se necessário.");
                return View(vm);
            }

            _service.Create(entity);
            return RedirectToAction(nameof(Index));
        }

        private bool ValidarCnpjLocal(string cnpj)
        {
            string cnpjLimpo = System.Text.RegularExpressions.Regex.Replace(cnpj, @"\D", "");

            if (cnpjLimpo.Length != 14)
                return false;

            if (!System.Text.RegularExpressions.Regex.IsMatch(cnpjLimpo, @"^\d{14}$"))
                return false;

            if (cnpjLimpo == new string(cnpjLimpo[0], cnpjLimpo.Length))
                return false;

            int[] mult1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string temp = cnpjLimpo.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(temp[i].ToString()) * mult1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            if (resto != int.Parse(cnpjLimpo[12].ToString()))
                return false;

            temp = cnpjLimpo.Substring(0, 13);
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(temp[i].ToString()) * mult2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            if (resto != int.Parse(cnpjLimpo[13].ToString()))
                return false;

            return true;
        }

        public IActionResult Edit(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CondominioViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Validar CNPJ localmente e avisar se inválido
            if (!string.IsNullOrWhiteSpace(vm.Cnpj) && !ValidarCnpjLocal(vm.Cnpj))
            {
                ModelState.AddModelError("Cnpj", "Aviso: O CNPJ informado pode ser inválido. Verifique e corrija se necessário.");
                return View(vm);
            }

            _service.Edit(_mapper.Map<Condominio>(vm));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var entity = _service.GetById(id);
            return View(_mapper.Map<CondominioViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetEndereco(int id)
        {
            var entity = _service.GetById(id);
            if (entity == null) return NotFound();

            var endereco = new
            {
                rua = entity.Rua,
                numero = entity.Numero,
                bairro = entity.Bairro,
                complemento = entity.Complemento,
                cidade = entity.Cidade,
                uf = entity.Uf,
                cep = entity.Cep
            };

            return Json(endereco);
        }
    }
}
