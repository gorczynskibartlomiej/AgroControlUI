using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.ReferenceData;
using AgroControlUI.Models.FarmData;
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
        public async Task<IActionResult> Index(int? fieldId)
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Fetching fields for the dropdown list
            var endpoint = "/api/fields";
            var fieldsResponse = await _client.GetAsync(endpoint);
            fieldsResponse.EnsureSuccessStatusCode();
            var fieldsContent = await fieldsResponse.Content.ReadAsStringAsync();
            var fields = JsonConvert.DeserializeObject<List<FieldDto>>(fieldsContent);

            // Create the dropdown list with fields
            ViewBag.Fields = fields?.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList() ?? new List<SelectListItem>();

            // Fetch crop rotation plans, with filtering by fieldId if provided
            endpoint = fieldId.HasValue ? $"/api/cropRotationPlanners?fieldId={fieldId}" : "/api/cropRotationPlanners";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var plans = JsonConvert.DeserializeObject<List<CropRotationPlannerDto>>(content);

            // Return the View with the filtered or all crop rotation plans
            return View(plans);
        }


        // Create
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Create(int? fieldId)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Fetch list of crops
                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();

                var model = new CreateCropRotationPlannerDto();
                if (fieldId.HasValue)
                {
                    model.FieldId = fieldId.Value;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Błąd przy ładowaniu danych: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCropRotationPlannerDto planDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();

                if (planDto.FieldId != 0)
                {
                    var fieldsResponse = await _client.GetAsync("/api/fields");
                    fieldsResponse.EnsureSuccessStatusCode();
                    var fieldsContent = await fieldsResponse.Content.ReadAsStringAsync();
                    var fields = JsonConvert.DeserializeObject<List<FieldDto>>(fieldsContent);
                    var selectedField = fields.FirstOrDefault(f => f.Id == planDto.FieldId);
                    ViewBag.SelectedFieldName = selectedField != null ? selectedField.Name : "Nie znaleziono pola";
                }
                else
                {
                    ViewBag.SelectedFieldName = "";
                }
                return View(planDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/cropRotationPlanners";
                var content = JsonConvert.SerializeObject(planDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowe zmianowanie upraw zostało pomyślnie dodane!";
                return RedirectToAction("Index", new { fieldId = planDto.FieldId });
            }
            catch (HttpRequestException ex)
            {
                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(planDto);
            }
            catch (Exception ex)
            {
                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);
                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(planDto);
            }
        }




        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, int fieldId)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/cropRotationPlanners/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Zmianowanie upraw zostało pomyślnie usunięte!";
                return RedirectToAction("Index", new { fieldId = fieldId });
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
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
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
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Pobranie szczegółów istniejącego planu
                var planResponse = await _client.GetAsync($"/api/cropRotationPlanners/{id}");
                if (!planResponse.IsSuccessStatusCode)
                {
                    TempData["errorMessage"] = "Nie znaleziono planu zmianowania.";
                    return RedirectToAction("Index");
                }
                var planContent = await planResponse.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CropRotationPlannerDto>(planContent);

                // Pobranie listy upraw
                var cropsResponse = await _client.GetAsync("/api/crops");
                cropsResponse.EnsureSuccessStatusCode();
                var cropsContent = await cropsResponse.Content.ReadAsStringAsync();
                var crops = JsonConvert.DeserializeObject<List<CropDto>>(cropsContent);

                ViewBag.Crops = crops?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>();

                var crop = crops?.FirstOrDefault(c => c.Name == model.CropName);
                int cropId = crop?.Id ?? 0;

                var cropRotationPlanner = new CreateCropRotationPlannerDto
                {
                    Id = model.Id,
                    FieldId = model.FieldId,
                    CropId = cropId,
                    Year = model.Year
                };

                return View(cropRotationPlanner);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Błąd przy ładowaniu danych: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateCropRotationPlannerDto planDto)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var cropsResponse = await _client.GetAsync("/api/crops");
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
                    TempData["errorMessage"] = "Błąd przy ładowaniu upraw: " + ex.Message;
                }
                return View(planDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/cropRotationPlanners/{planDto.Id}";
                var content = JsonConvert.SerializeObject(planDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Zmianowanie upraw zostało pomyślnie zaaktualizowane!";

                // Można dodać przekierowanie do pola, jeśli masz FieldId
                return RedirectToAction("Index", new { fieldId = planDto.FieldId });
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(planDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
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
