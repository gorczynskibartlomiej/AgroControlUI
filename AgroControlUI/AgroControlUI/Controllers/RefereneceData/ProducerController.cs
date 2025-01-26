using AgroControlUI.Constants;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.RefereneceData
{
    public class ProducerController : Controller
    {
        private readonly HttpClient _client;

        public ProducerController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        // GetAll Producers
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/producers";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var producers = JsonConvert.DeserializeObject<List<ProducerDto>>(content);
            return View(producers);
        }

        // Get Producer Details
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/producers/{id}";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var producer = JsonConvert.DeserializeObject<ProducerDto>(content);
            return View(producer);
        }

        // Create Producer
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(ProducerDto producerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(producerDto);
            }

            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/producers";
            var content = JsonConvert.SerializeObject(producerDto);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(endpoint, stringContent);
            result.EnsureSuccessStatusCode();

            TempData["successMessage"] = "Producent został dodany.";
            return RedirectToAction("Index");
        }

        // Edit Producer
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/producers/{id}";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var producer = JsonConvert.DeserializeObject<ProducerDto>(content);
            return View(producer);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProducerDto producerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(producerDto);
            }

            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/producers/{producerDto.Id}";
            var content = JsonConvert.SerializeObject(producerDto);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await _client.PutAsync(endpoint, stringContent);
            result.EnsureSuccessStatusCode();

            TempData["successMessage"] = "Producent został zaktualizowany.";
            return RedirectToAction("Index");
        }

        // Delete Producer
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/producers/{id}";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var producer = JsonConvert.DeserializeObject<ProducerDto>(content);
            return View(producer);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/producers/{id}";
            var result = await _client.DeleteAsync(endpoint);
            result.EnsureSuccessStatusCode();

            TempData["successMessage"] = "Producent został usunięty.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
