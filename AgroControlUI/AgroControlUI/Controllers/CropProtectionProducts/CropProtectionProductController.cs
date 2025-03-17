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
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/cropProtectionProducts";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<CropProtectionProductDto>>(content);
            return View(products);
        }

        // Create
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCropProtectionProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var activeIngredientErrors = ModelState
            .Where(m => m.Key.StartsWith("ActiveIngredients") && m.Value.Errors.Any())
            .SelectMany(m => m.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                // Jeżeli są błędy, przekazujemy je do TempData
                if (activeIngredientErrors.Any())
                {
                    TempData["ActiveIngredientErrors"] = "Musisz wybrać składnik aktywny. Ilość składnika musi być liczbą całkowitą większą od zera.";
                }
                await LoadSelectLists();
                return View(productDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/cropProtectionProducts";
                var content = JsonConvert.SerializeObject(productDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowy środek ochrony roślin został pomyślnie dodany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Środek ochrony roślin o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                    await LoadSelectLists();
                }
                return View(productDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                await LoadSelectLists();
                return View(productDto);
            }
        }

        private async Task LoadSelectLists()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Producers = await FetchProducers();
                ViewBag.Crops = await FetchCrops();
                ViewBag.Components = await FetchActiveIngredients();
                ViewBag.Categories = await FetchCategories();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Nie udało się załadować list. Spróbuj ponownie.";
            }
        }

        private async Task<List<SelectListItem>> FetchProducers()
        {
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/cropProtectionProducts/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Środek ochrony roślin został pomyślnie usunięty!";
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
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View();
            }
        }

        // Edit - GET
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/cropProtectionProducts/{id}";
                var result = await _client.GetAsync(endpoint);

                if (!result.IsSuccessStatusCode)
                {
                    TempData["errorMessage"] = "Nie udało się pobrać danych środka ochrony roślin.";
                    return RedirectToAction("Index");
                }

                var content = await result.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<CropProtectionProductDto>(content);

                var createProduct = new CreateCropProtectionProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    ProducerId = product.Producer.Id,
                    CropIds = product.cropProtectionProductCrops?.Select(crop => crop.Id).ToList(),

                    ActiveIngredients = product.cropProtectionProductComponents?.Select(component => new CreateCropProtectionProductComponent
                    {
                        ActiveIngredientId = component.ComponentId,
                        Concentration = component.Concentration
                    }).ToList(),

                    CategoryIds = product.cropProtectionProductCategories?.Select(category => category.Id).ToList()
                };

                await LoadSelectLists();
                return View(createProduct);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View();
            }
        }

        // Edit - POST
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateCropProtectionProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                var activeIngredientErrors = ModelState
                   .Where(m => m.Key.StartsWith("ActiveIngredients") && m.Value.Errors.Any())
                   .SelectMany(m => m.Value.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();

                if (activeIngredientErrors.Any())
                {
                    TempData["ActiveIngredientErrors"] = "Musisz wybrać składnik aktywny. Ilość składnika musi być liczbą całkowitą większą od zera.";
                }
                await LoadSelectLists();
                return View(productDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/cropProtectionProducts/{productDto.Id}";
                var content = JsonConvert.SerializeObject(productDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync(endpoint, stringContent);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["errorMessage"] = "Błąd podczas aktualizacji środka ochrony roślin.";
                    await LoadSelectLists();
                    return View(productDto);
                }

                TempData["successMessage"] = "Środek ochrony roślin został pomyślnie zaktualizowany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Środek ochrony roślin o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
            }

            await LoadSelectLists();
            return View(productDto);
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
