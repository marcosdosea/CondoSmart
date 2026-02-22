using CondosmartWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace CondosmartWeb.Controllers
{
    public class MoradorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public MoradorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CondosmartApi");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/moradores");
            if (!response.IsSuccessStatusCode)
                return View(new List<MoradorViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var lista = JsonSerializer.Deserialize<List<MoradorViewModel>>(json, _jsonOptions);
            return View(lista);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/moradores/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var vm = JsonSerializer.Deserialize<MoradorViewModel>(json, _jsonOptions);
            return View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoradorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var json = JsonSerializer.Serialize(vm);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/moradores", content);
            if (!response.IsSuccessStatusCode)
                return View(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/moradores/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var vm = JsonSerializer.Deserialize<MoradorViewModel>(json, _jsonOptions);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MoradorViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var json = JsonSerializer.Serialize(vm);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/moradores/{vm.Id}", content);
            if (!response.IsSuccessStatusCode)
                return View(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"api/moradores/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var vm = JsonSerializer.Deserialize<MoradorViewModel>(json, _jsonOptions);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/moradores/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
