using CondosmartWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace CondosmartWeb.Controllers
{
    [Authorize(Roles = "Admin,Sindico,Morador")]
    public class ReservaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("CondoSmartAPI");

        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/reservas");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Logout", "Account", new { area = "Identity" });

            response.EnsureSuccessStatusCode();

            var reservas = await response.Content.ReadFromJsonAsync<List<ReservaViewModel>>();
            return View(reservas ?? new List<ReservaViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/reservas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var reserva = await response.Content.ReadFromJsonAsync<ReservaViewModel>();
            return View(reserva);
        }

        public async Task<IActionResult> Create()
        {
            await CarregarListas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservaViewModel vm)
        {
            if (vm.CondominioId == 0) vm.CondominioId = 1;

            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var response = await client.PostAsJsonAsync("api/reservas", vm);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var erro = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao criar reserva: {erro}");
            }

            await CarregarListas();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/reservas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var reserva = await response.Content.ReadFromJsonAsync<ReservaViewModel>();
            await CarregarListas();
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservaViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (vm.CondominioId == 0) vm.CondominioId = 1;

            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var response = await client.PutAsJsonAsync($"api/reservas/{id}", vm);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var erro = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao editar reserva: {erro}");
            }

            await CarregarListas();
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/reservas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var reserva = await response.Content.ReadFromJsonAsync<ReservaViewModel>();
            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClient();
            await client.DeleteAsync($"api/reservas/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarListas()
        {
            var client = CreateClient();

            var areas = await client.GetFromJsonAsync<List<AreaDeLazerViewModel>>("api/areadelazer")
                        ?? new List<AreaDeLazerViewModel>();

            var moradores = await client.GetFromJsonAsync<List<MoradorViewModel>>("api/moradores")
                            ?? new List<MoradorViewModel>();

            ViewBag.AreaId = new SelectList(areas, "Id", "Nome");
            ViewBag.MoradorId = new SelectList(moradores, "Id", "Nome");
            ViewBag.Status = new SelectList(new[] { "pendente", "confirmado", "cancelado", "concluido" });
        }
    }
}