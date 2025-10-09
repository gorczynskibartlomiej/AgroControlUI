using AgroControlUI.Constants;
using AgroControlUI.DTOs.CropProtectionProducts;
using AgroControlUI.DTOs.Fertilizers;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.Fertilizers
{
    public class FertilizerController : Controller
    {
        private readonly HttpClient _client;

        public FertilizerController(IHttpClientFactory httpClientFactory)
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

            var endpoint = "/api/fertilizers";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var fertilizers = JsonConvert.DeserializeObject<List<FertilizerDto>>(content);
            return View(fertilizers);
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
        public async Task<IActionResult> Create(CreateFertilizerDto fertilizerDto)
        { 

            if (!ModelState.IsValid)
            {
                var componentsErrors = ModelState
            .Where(m => m.Key.StartsWith("FertilizerComponents") && m.Value.Errors.Any())
            .SelectMany(m => m.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                // Jeżeli są błędy, przekazujemy je do TempData
                if (componentsErrors.Any())
                {
                    TempData["ComponentsErrors"] = "Musisz wybrać składnik nawozu. Ilość musi być liczbą większa od 0 i mniejszą od 100 oraz mieć dokładnośc do cześci setnych (np. 12,55). Suma zawartości składników nie może przekraczać 100%.";
                }
                await LoadSelectLists();
                return View(fertilizerDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/fertilizers";
                var content = JsonConvert.SerializeObject(fertilizerDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowy nawóz został pomyślnie dodany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Nawóz o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(fertilizerDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(fertilizerDto);
            }
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

                var endpoint = $"/api/fertilizers/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nawóz został pomyślnie usunięty!";
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
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
        }

        // Edit
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fertilizers/{id}";
                var response = await _client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["errorMessage"] = "Nie udało się pobrać danych nawozu.";
                    return RedirectToAction("Index");
                }

                string content = await response.Content.ReadAsStringAsync();
                var fertilizer = JsonConvert.DeserializeObject<FertilizerDto>(content);

                var creteFertilizer = new CreateFertilizerDto
                {
                    Id = fertilizer.Id,
                    Name = fertilizer.Name,
                    ProducerId = fertilizer.Producer.Id,
                    CategoryId = fertilizer.FertilizerCategory.Id,
                    Description = fertilizer.Description,
                    FertilizerComponents = fertilizer.FertilizerComponents.Select(component => new CreateFertilizerComponentDto
                    {
                        ElementPercentage = component.ElementPercentage.ToString(CultureInfo.InvariantCulture),
                        ChemicalElementId = component.ChemicalElement.Id
                    }).ToList()
                };
                 await LoadSelectLists();
                return View(creteFertilizer);
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

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateFertilizerDto fertilizerDto)
        {
            if (!ModelState.IsValid)
            {
                var componentsErrors = ModelState
            .Where(m => m.Key.StartsWith("FertilizerComponents") && m.Value.Errors.Any())
            .SelectMany(m => m.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                // Jeżeli są błędy, przekazujemy je do TempData
                if (componentsErrors.Any())
                {
                    TempData["ComponentsErrors"] = "Musisz wybrać składnik nawozu. Ilość musi być liczbą większa od 0 i mniejszą od 100 oraz mieć dokładnośc do cześci setnych (np. 12,55). Suma zawartości składników nie może przekraczać 100%.";
                }
                await LoadSelectLists();
                return View(fertilizerDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fertilizers/{fertilizerDto.Id}";
                var content = JsonConvert.SerializeObject(fertilizerDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nawóz został pomyślnie zmodyfikowany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Nawóz o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                await LoadSelectLists();
                return View(fertilizerDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                await LoadSelectLists();
                return View(fertilizerDto);
            }
        }
        private async Task LoadSelectLists()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Producers = await FetchProducers();
                ViewBag.Categories = await FetchCategories();
                ViewBag.ChemicalElements = await FetchChemicalElements();
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Nie udało się załadować list. Spróbuj ponownie.";
            }
        }

        private async Task<List<SelectListItem>> FetchProducers()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) { token = Request.Headers["Authorization"]; }
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

        private async Task<List<SelectListItem>> FetchCategories()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/fertilizerCategories");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<FertilizerCategoryDto>>(content);

            return categories?.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList() ?? new List<SelectListItem>();
        }
        private async Task<List<SelectListItem>> FetchChemicalElements()
        {
            var token = HttpContext.Request.Cookies["token"];
            if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/chemicalElements");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var elements = JsonConvert.DeserializeObject<List<ChemicalElementDto>>(content);

            return elements?.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToList() ?? new List<SelectListItem>();
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
