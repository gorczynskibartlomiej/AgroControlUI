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
    public class CropRotationPlannerController : Controller
    {
        private readonly HttpClient _client;

        public CropRotationPlannerController(IHttpClientFactory httpClientFactory)
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
            var fieldsResponse = await _client.GetAsync(endpoint);
            fieldsResponse.EnsureSuccessStatusCode();
            var fieldsContent = await fieldsResponse.Content.ReadAsStringAsync();
            var fields = JsonConvert.DeserializeObject<List<FieldDto>>(fieldsContent);
            ViewBag.Fields = fields?.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList() ?? new List<SelectListItem>();


            endpoint = "/api/cropRotationPlanners";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var plans = JsonConvert.DeserializeObject<List<CropRotationPlannerDto>>(content);
            return View(plans);
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

                var endpoint = "/api/crops";
                var cropsResponse = await _client.GetAsync(endpoint);
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Nie udało się załadować danych.";
            }

            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Create(CropRotationPlannerDto planDto)
        {
            if (!ModelState.IsValid)
            {
                var endpoint = "/api/fields";
                var fieldsResponse = await _client.GetAsync(endpoint);
                fieldsResponse.EnsureSuccessStatusCode();
                var fieldsContent = await fieldsResponse.Content.ReadAsStringAsync();
                var fields = JsonConvert.DeserializeObject<List<FieldDto>>(fieldsContent);
                ViewBag.Fields = fields?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();
                return View(planDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/fields";
                var fieldsResponse = await _client.GetAsync(endpoint);
                fieldsResponse.EnsureSuccessStatusCode();
                var fieldsContent = await fieldsResponse.Content.ReadAsStringAsync();
                var fields = JsonConvert.DeserializeObject<List<FieldDto>>(fieldsContent);
                ViewBag.Fields = fields?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();

                endpoint = "/api/cropRotationPlaners";
                var content = JsonConvert.SerializeObject(planDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(planDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(planDto);
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

                var endpoint = $"/api/cropRotationPlans/{id}";
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

                var endpoint = $"/api/cropRotationPlans/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var plan = JsonConvert.DeserializeObject<CropRotationPlannerDto>(content);
                return View(plan);
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
        public async Task<IActionResult> Edit(CropRotationPlannerDto planDto)
        {
            if (!ModelState.IsValid)
            {
                return View(planDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/cropRotationPlans/{planDto.Id}";
                var content = JsonConvert.SerializeObject(planDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Zmodyfikowano pomyślnie";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(planDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(planDto);
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
