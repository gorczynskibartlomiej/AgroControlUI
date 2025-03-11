using AgroControlUI.Constants;
using AgroControlUI.DTOs.CropProtectionProducts;
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
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
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
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var fuelResponse = await _client.GetAsync("/api/fuels");
            fuelResponse.EnsureSuccessStatusCode();
            var fuelContent = await fuelResponse.Content.ReadAsStringAsync();
            var fuels = JsonConvert.DeserializeObject<List<FuelDto>>(fuelContent);

            ViewBag.Fuels = fuels?.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),  
                Text = f.Name             
            }).ToList() ?? new List<SelectListItem>();

            var agriEqTypesResponse = await _client.GetAsync("/api/AgriculturalEquipmentTypes");
            agriEqTypesResponse.EnsureSuccessStatusCode();
            var agriEqTypesContent = await agriEqTypesResponse.Content.ReadAsStringAsync();
            var agriEqTypes = JsonConvert.DeserializeObject<List<AgriculturalEquipmentTypeDto>>(agriEqTypesContent);

            ViewBag.AgriculturalEquipmentTypes = agriEqTypes?.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList() ?? new List<SelectListItem>();
            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAgriculturalEquipmentDto equipmentDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var fuelResponse = await _client.GetAsync("/api/fuels");
                fuelResponse.EnsureSuccessStatusCode();
                var fuelContent = await fuelResponse.Content.ReadAsStringAsync();
                var fuels = JsonConvert.DeserializeObject<List<FuelDto>>(fuelContent);

                ViewBag.Fuels = fuels?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                var agriEqTypesResponse = await _client.GetAsync("/api/agriculturalEquipmentTypes");
                agriEqTypesResponse.EnsureSuccessStatusCode();
                var agriEqTypesContent = await agriEqTypesResponse.Content.ReadAsStringAsync();
                var agriEqTypes = JsonConvert.DeserializeObject<List<AgriculturalEquipmentTypeDto>>(agriEqTypesContent);

                ViewBag.AgriculturalEquipmentTypes = agriEqTypes?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();
                return View(equipmentDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/agriculturalEquipment";
                var content = JsonConvert.SerializeObject(equipmentDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Nowa maszyna została pomyślnie dodana!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Maszyna o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(equipmentDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(equipmentDto);
            }
        }

        // Delete

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Maszyna została pomyślnie usunięta!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return RedirectToAction();
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

                var fuelResponse = await _client.GetAsync("/api/fuels");
                fuelResponse.EnsureSuccessStatusCode();
                var fuelContent = await fuelResponse.Content.ReadAsStringAsync();
                var fuels = JsonConvert.DeserializeObject<List<FuelDto>>(fuelContent);

                ViewBag.Fuels = fuels?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                var agriEqTypesResponse = await _client.GetAsync("/api/AgriculturalEquipmentTypes");
                agriEqTypesResponse.EnsureSuccessStatusCode();
                var agriEqTypesContent = await agriEqTypesResponse.Content.ReadAsStringAsync();
                var agriEqTypes = JsonConvert.DeserializeObject<List<AgriculturalEquipmentTypeDto>>(agriEqTypesContent);

                ViewBag.AgriculturalEquipmentTypes = agriEqTypes?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                var endpoint = $"/api/agriculturalEquipment/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var equipment = JsonConvert.DeserializeObject<AgriculturalEquipmentDetailsDto>(content);

                var createEquipment = new CreateAgriculturalEquipmentDto
                {
                    Name = equipment.Name,
                    Brand = equipment.Brand,
                    Description = equipment.Description,
                    IsActive = equipment.IsActive,
                    YearOfManufacture = equipment.YearOfManufacture,
                    FuelId = equipment.Fuel != null ? equipment.Fuel.Id : (int?)null,
                    FuelCapacity = equipment.FuelCapacity,
                    EnginePower = equipment.EnginePower,
                    Weight = equipment.Weight,
                    Width = equipment.Width,
                    Height = equipment.Height,
                    WorkingSpeed = equipment.WorkingSpeed,
                    TransportSpeed = equipment.TransportSpeed,
                    WorkingWidth = equipment.WorkingWidth,
                    LastServiceDate = equipment.LastServiceDate,
                    NextServiceDate = equipment.NextServiceDate,
                    AgriculturalEquipmentTypeId = equipment.AgriculturalEquipmentType.Id
                };
                return View(createEquipment);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return RedirectToAction();
            }
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateAgriculturalEquipmentDto equipmentDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var fuelResponse = await _client.GetAsync("/api/fuels");
                fuelResponse.EnsureSuccessStatusCode();
                var fuelContent = await fuelResponse.Content.ReadAsStringAsync();
                var fuels = JsonConvert.DeserializeObject<List<FuelDto>>(fuelContent);

                ViewBag.Fuels = fuels?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                var agriEqTypesResponse = await _client.GetAsync("/api/AgriculturalEquipmentTypes");
                agriEqTypesResponse.EnsureSuccessStatusCode();
                var agriEqTypesContent = await agriEqTypesResponse.Content.ReadAsStringAsync();
                var agriEqTypes = JsonConvert.DeserializeObject<List<AgriculturalEquipmentTypeDto>>(agriEqTypesContent);

                ViewBag.AgriculturalEquipmentTypes = agriEqTypes?.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList() ?? new List<SelectListItem>();

                return View(equipmentDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{equipmentDto.Id}";
                var content = JsonConvert.SerializeObject(equipmentDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Maszyna została pomyślnie zaaktualizowana!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Maszyna o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(equipmentDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(equipmentDto);
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"/api/agriculturalEquipment/{id}";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var agriculturalEquipment = JsonConvert.DeserializeObject<AgriculturalEquipmentDetailsDto>(content);
            return View(agriculturalEquipment);
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> UpdateIsActive(int id, bool isActive)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                if (token == null)
                {
                    token = Request.Headers["Authorization"];
                }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/agriculturalEquipment/{id}/updateIsActive";
 
                var content = JsonConvert.SerializeObject(isActive);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PatchAsync(endpoint, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Stan aktywności maszyny został zaktualizowany!";
                }
                else
                {
                    TempData["errorMessage"] = "Wystąpił problem z aktualizacją stanu maszyny.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
            }

            // Po zakończeniu aktualizacji, przekierowanie z powrotem na stronę indeksu
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
