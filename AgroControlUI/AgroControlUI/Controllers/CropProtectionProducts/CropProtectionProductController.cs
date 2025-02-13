using AgroControlUI.Constants;
using AgroControlUI.DTOs.CropProtectionProducts;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.CropProtectionProducts
{
    public class CropProtectionProductController : Controller
    {
        private readonly HttpClient _client;

        public CropProtectionProductController(IHttpClientFactory httpClientFactory)
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

            var endpoint = "/api/cropProtectionProducts";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<CropProtectionProductDto>>(content);
            return View(products);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        // POST: Dodanie nowego środka ochrony roślin
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCropProtectionProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return View(productDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/cropProtectionProducts";
                var content = JsonConvert.SerializeObject(productDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
            }

            await LoadSelectLists();
            return View(productDto);
        }

        private async Task LoadSelectLists()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Producers = await FetchProducers();
                ViewBag.Crops = await FetchCrops();
                ViewBag.Components = await FetchActiveIngredients();
                ViewBag.Categories = await FetchCategories();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Nie udało się załadować list: ";
            }
        }

        private async Task<List<SelectListItem>> FetchProducers()
        {
            var response = await _client.GetAsync("/api/producers");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var producers = JsonConvert.DeserializeObject<List<ProducerDto>>(content);

            return producers?.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList() ?? new List<SelectListItem>();
        }

        private async Task<List<SelectListItem>> FetchCrops()
        {
            var response = await _client.GetAsync("/api/crops");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var crops = JsonConvert.DeserializeObject<List<CropDto>>(content);

            return crops?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList() ?? new List<SelectListItem>();
        }

        private async Task<List<SelectListItem>> FetchActiveIngredients()
        {
            var response = await _client.GetAsync("/api/activeIngredients");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var ingredients = JsonConvert.DeserializeObject<List<ActiveIngredientDto>>(content);

            return ingredients?.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            }).ToList() ?? new List<SelectListItem>();
        }

        private async Task<List<SelectListItem>> FetchCategories()
        {
            var response = await _client.GetAsync("/api/cropProtectionProductCategories");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CropProtectionProductCategoryDto>>(content);

            return categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList() ?? new List<SelectListItem>();
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

                var endpoint = $"/api/cropProtectionProducts/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<CropProtectionProductDto>(content);
                return View(product);
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

                var endpoint = $"/api/cropProtectionProducts/{id}";
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
