using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.FarmData
{
    public class AgriculturalEquipmentController : Controller
    {
        private readonly HttpClient _client;

        public AgriculturalEquipmentController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        // GetAll
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/agriculturalEquipment";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var equipmentList = JsonConvert.DeserializeObject<List<AgriculturalEquipmentDto>>(content);
            return View(equipmentList);
        }

        // Create
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Create(AgriculturalEquipmentDto equipmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(equipmentDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/agriculturalEquipment";
                var content = JsonConvert.SerializeObject(equipmentDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(equipmentDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(equipmentDto);
            }
        }

        // Delete
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var equipment = JsonConvert.DeserializeObject<AgriculturalEquipmentDto>(content);
                return View(equipment);
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

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

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
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var equipment = JsonConvert.DeserializeObject<AgriculturalEquipmentDto>(content);
                return View(equipment);
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

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Edit(AgriculturalEquipmentDto equipmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(equipmentDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{equipmentDto.Id}";
                var content = JsonConvert.SerializeObject(equipmentDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Zmodyfikowano pomyślnie";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(equipmentDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(equipmentDto);
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
