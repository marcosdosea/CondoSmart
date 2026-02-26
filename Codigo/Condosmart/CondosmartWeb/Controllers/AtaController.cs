using CondosmartWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = "Admin,Sindico,Morador")]
    public class AtaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AtaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("CondoSmartAPI");

        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/atas");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Logout", "Account", new { area = "Identity" });

            response.EnsureSuccessStatusCode();

            var atas = await response.Content.ReadFromJsonAsync<List<AtaViewModel>>();
            return View(atas ?? new List<AtaViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/atas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var ata = await response.Content.ReadFromJsonAsync<AtaViewModel>();
            return View(ata);
        }

        public async Task<IActionResult> Create()
        {
            await CarregarListas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AtaViewModel vm)
        {
            if (vm.CondominioId == 0) vm.CondominioId = 1;

            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var response = await client.PostAsJsonAsync("api/atas", vm);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var erro = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao criar ata: {erro}");
            }

            await CarregarListas();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/atas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var ata = await response.Content.ReadFromJsonAsync<AtaViewModel>();
            await CarregarListas();
            return View(ata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AtaViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (vm.CondominioId == 0) vm.CondominioId = 1;

            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var response = await client.PutAsJsonAsync($"api/atas/{id}", vm);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var erro = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao editar ata: {erro}");
            }

            await CarregarListas();
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/atas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var ata = await response.Content.ReadFromJsonAsync<AtaViewModel>();
            return View(ata);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClient();
            await client.DeleteAsync($"api/atas/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarListas()
        {
            var client = CreateClient();

            var condominios = await client.GetFromJsonAsync<List<dynamic>>("api/condominios")
                              ?? new List<dynamic>();

            var sindicos = await client.GetFromJsonAsync<List<dynamic>>("api/sindicos")
                           ?? new List<dynamic>();

            ViewBag.CondominioId = new SelectList(condominios, "id", "nome");
            ViewBag.SindicoId = new SelectList(sindicos, "id", "nome");
        }
    }
}
