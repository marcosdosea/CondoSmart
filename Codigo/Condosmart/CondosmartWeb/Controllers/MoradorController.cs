using CondosmartWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = "Admin,Sindico")]
    public class MoradorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MoradorController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("CondoSmartAPI");

        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var moradores = await client.GetFromJsonAsync<List<MoradorViewModel>>("api/moradores");
            return View(moradores ?? new List<MoradorViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/moradores/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var morador = await response.Content.ReadFromJsonAsync<MoradorViewModel>();
            return View(morador);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoradorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var client = CreateClient();
            var response = await client.PostAsJsonAsync("api/moradores", vm);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Erro ao criar morador.");
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/moradores/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var morador = await response.Content.ReadFromJsonAsync<MoradorViewModel>();
            return View(morador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MoradorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/moradores/{vm.Id}", vm);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Erro ao editar morador.");
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/moradores/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var morador = await response.Content.ReadFromJsonAsync<MoradorViewModel>();
            return View(morador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClient();
            await client.DeleteAsync($"api/moradores/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
