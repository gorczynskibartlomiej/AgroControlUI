using AgroControlUI.Constants;
using AgroControlUI.DTOs.Fertilizers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.Fertilizers
{
    public class FertilizerCategoryController : Controller
    {
        private readonly HttpClient _client;

        public FertilizerCategoryController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        // GetAll
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/fertilizercategories";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var fertilizerCategories = JsonConvert.DeserializeObject<List<FertilizerCategoryDto>>(content);
            return View(fertilizerCategories);
        }

        // Create
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(FertilizerCategoryDto fertilizerCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fertilizerCategoryDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/fertilizercategories";
                var content = JsonConvert.SerializeObject(fertilizerCategoryDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(fertilizerCategoryDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(fertilizerCategoryDto);
            }
        }

        // Delete
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fertilizercategories/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var fertilizerCategory = JsonConvert.DeserializeObject<FertilizerCategoryDto>(content);
                return View(fertilizerCategory);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View();
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fertilizercategories/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Kategoria nawozów została pomyślnie usunięta.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View();
            }
        }

        // Edit
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fertilizercategories/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var fertilizerCategory = JsonConvert.DeserializeObject<FertilizerCategoryDto>(content);
                return View(fertilizerCategory);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View();
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(FertilizerCategoryDto fertilizerCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fertilizerCategoryDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fertilizercategories/{fertilizerCategoryDto.Id}";
                var content = JsonConvert.SerializeObject(fertilizerCategoryDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Zmodyfikowano pomyślnie";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(fertilizerCategoryDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(fertilizerCategoryDto);
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
