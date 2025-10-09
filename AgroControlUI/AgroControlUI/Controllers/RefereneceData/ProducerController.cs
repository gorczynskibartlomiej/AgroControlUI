using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
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

            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/producers";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var producers = JsonConvert.DeserializeObject<List<ProducerDto>>(content); 
            return View(producers);
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
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/producers";
                var content = JsonConvert.SerializeObject(producerDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var result = await _client.PostAsync(endpoint, stringContent);
                result.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowy producent został pomyślnie dodany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Producent o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(producerDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(producerDto);
            }
        }

        // Edit Producer
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {

                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/producers/{id}";
                var result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();
                var producer = JsonConvert.DeserializeObject<ProducerDto>(content);
                return View(producer);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View();
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProducerDto producerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(producerDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/producers/{producerDto.Id}";
                var content = JsonConvert.SerializeObject(producerDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var result = await _client.PutAsync(endpoint, stringContent);
                result.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Producent został pomyślnie zaaktualizowany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Producent o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(producerDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(producerDto);
            }
        }

        // Delete Producer
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/producers/{id}";
                var result = await _client.DeleteAsync(endpoint);
                result.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Producent został pomyślnie usunięty!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Nie można usunąć tego obiektu, ponieważ jest powiązany z innymi danymi.";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return RedirectToAction("Index");
            }
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
