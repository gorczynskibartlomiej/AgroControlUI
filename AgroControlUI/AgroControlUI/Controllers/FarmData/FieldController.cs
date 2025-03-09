using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.FarmData
{
    public class FieldController : Controller
    {
        private readonly HttpClient _client;

        public FieldController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        // GetAll
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/fields";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var fields = JsonConvert.DeserializeObject<List<FieldDto>>(content);
            return View(fields);
        }

        // Create
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Pobieranie typów gleby
                 var soilResponse = await _client.GetAsync("/api/soiltypes");
                soilResponse.EnsureSuccessStatusCode();
                var soilContent = await soilResponse.Content.ReadAsStringAsync();
                var soilTypes = JsonConvert.DeserializeObject<List<SoilTypeDto>>(soilContent);
                ViewBag.SoilTypes = soilTypes?.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<SelectListItem>();

                // Pobieranie kodu pocztowego
                var postalCodeResponse = await _client.GetAsync("/api/farms/postalCode");
                postalCodeResponse.EnsureSuccessStatusCode();
                var postalCode = await postalCodeResponse.Content.ReadAsStringAsync();

                ViewBag.PostalCode = postalCode;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Nie udało się załadować danych.";
            }

            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateFieldDto fieldDto)
        {
            if (!ModelState.IsValid)
            {
                var soilResponse = await _client.GetAsync("/api/soiltypes");
                soilResponse.EnsureSuccessStatusCode();
                var soilContent = await soilResponse.Content.ReadAsStringAsync();
                var soilTypes = JsonConvert.DeserializeObject<List<SoilTypeDto>>(soilContent);
                ViewBag.SoilTypes = soilTypes?.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<SelectListItem>();

                // Pobieranie kodu pocztowego
                var postalCodeResponse = await _client.GetAsync("/api/farms/postalCode");
                postalCodeResponse.EnsureSuccessStatusCode();
                var postalCode = await postalCodeResponse.Content.ReadAsStringAsync();

                ViewBag.PostalCode = postalCode;
                return View(fieldDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/fields";
                var content = JsonConvert.SerializeObject(fieldDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowe pole zostało pomyślnie dodane!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(fieldDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(fieldDto);
            }
        }

        // Delete

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fields/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Pole zostało pomyślnie usunięte!";
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

                var soilResponse = await _client.GetAsync("/api/soiltypes");
                soilResponse.EnsureSuccessStatusCode();
                var soilContent = await soilResponse.Content.ReadAsStringAsync();
                var soilTypes = JsonConvert.DeserializeObject<List<SoilTypeDto>>(soilContent);
                ViewBag.SoilTypes = soilTypes?.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<SelectListItem>();

                var postalCodeResponse = await _client.GetAsync("/api/farms/postalCode");
                postalCodeResponse.EnsureSuccessStatusCode();
                var postalCode = await postalCodeResponse.Content.ReadAsStringAsync();

                ViewBag.PostalCode = postalCode;

                var endpoint = $"/api/fields/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();

                var fieldDetails = JsonConvert.DeserializeObject<FieldDetailsDto>(content);

                var createFieldDto = new CreateFieldDto
                {
                    Id = fieldDetails.Id,
                    Name = fieldDetails.Name,
                    Description = fieldDetails.Description,
                    FieldBorder = fieldDetails.FieldBorder,
                    Area = fieldDetails.Area,
                    SoilTypesIds = fieldDetails.SoilTypes.Select(st => st.Id).ToList()
                };

                return View(createFieldDto);
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
        public async Task<IActionResult> Edit(CreateFieldDto fieldDto)
        {
            if (!ModelState.IsValid)
            {
                var soilResponse = await _client.GetAsync("/api/soiltypes");
                soilResponse.EnsureSuccessStatusCode();
                var soilContent = await soilResponse.Content.ReadAsStringAsync();
                var soilTypes = JsonConvert.DeserializeObject<List<SoilTypeDto>>(soilContent);
                ViewBag.SoilTypes = soilTypes?.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList() ?? new List<SelectListItem>();

                var postalCodeResponse = await _client.GetAsync("/api/farms/postalCode");
                postalCodeResponse.EnsureSuccessStatusCode();
                var postalCode = await postalCodeResponse.Content.ReadAsStringAsync();

                ViewBag.PostalCode = postalCode;
                return View(fieldDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fields/{fieldDto.Id}";
                var content = JsonConvert.SerializeObject(fieldDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Pole zostało pomyślnie zaaktualizowane!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(fieldDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(fieldDto);
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/fields/{id}";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var field = JsonConvert.DeserializeObject<FieldDetailsDto>(content);
            return View(field);
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
