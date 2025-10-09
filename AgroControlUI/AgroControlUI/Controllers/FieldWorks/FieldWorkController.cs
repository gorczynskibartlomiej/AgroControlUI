using AgroControlUI.Constants;
using AgroControlUI.DTOs.CropProtectionProducts;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.Fertilizers;
using AgroControlUI.DTOs.FieldWorks;
using AgroControlUI.DTOs.ReferenceData;
using AgroControlUI.DTOs.UserManagement;
using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.Fertilizers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AgroControlUI.Controllers.FieldWorks
{
    public class FieldWorkController : Controller
    {
        private readonly HttpClient _client;

        public FieldWorkController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        // GetAll
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var fieldName = TempData["FieldName"] as string;
            ViewBag.SelectedFieldName = fieldName;
            var endpoint = "/api/fieldworks";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var fieldWorks = JsonConvert.DeserializeObject<List<FieldWorkDto>>(content);

            return View(fieldWorks);
        }

        // Create
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public async Task<IActionResult> CreateFertilizingWork()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                ViewBag.Fertilizers = await GetFertilizers();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.Units = await GetUnits();
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();

                return View();
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


        [Authorize(Policy = "OwnerOrWorker")]
        [HttpPost]
        public async Task<IActionResult> CreateFertilizingWork(CreateFertilizingWorkDto createFertilizingWorkDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                ViewBag.Fertilizers = await GetFertilizers();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.Units = await GetUnits();
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();

                    var activeIngredientErrors = ModelState
                .Where(m => m.Key.StartsWith("FertilizingWorkFertilizers") && m.Value.Errors.Any())
                .SelectMany(m => m.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                    if (activeIngredientErrors.Any())
                    {
                        TempData["ActiveIngredientErrors"] = "Musisz wybrać nawóz i jednostkę. Ilość musi być liczbą całkowitą większą od zera.";
                    }
                    return View(createFertilizingWorkDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(createFertilizingWorkDto), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/fertilizingWork", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowa praca została dodana pomyślnie.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(createFertilizingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(createFertilizingWorkDto);
            }
        }
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public async Task<IActionResult> CreateHarvestingWork()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                if(!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();

                return View();
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


        [Authorize(Policy = "OwnerOrWorker")]
        [HttpPost]
        public async Task<IActionResult> CreateHarvestingWork(CreateHarvestingWorkDto createHarvestingWorkDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();

                return View();
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(createHarvestingWorkDto), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/harvestingWork", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowa praca została dodana pomyślnie.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(createHarvestingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(createHarvestingWorkDto);
            }
        }
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public async Task<IActionResult> CreateSprayingWork()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.CropProtectionProducts = await GetCropProtectionProducts();
                ViewBag.Fields = await GetFields();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();
                ViewBag.Units = await GetUnits();

                return View();
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


        [Authorize(Policy = "OwnerOrWorker")]
        [HttpPost]
        public async Task<IActionResult> CreateSprayingWork(CreateSprayingWorkDto createSprayingWorkDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();
                ViewBag.Units = await GetUnits();
                ViewBag.CropProtectionProducts = await GetCropProtectionProducts();
                var activeIngredientErrors = ModelState
                .Where(m => m.Key.StartsWith("SprayingWorkCropProtectionProducts") && m.Value.Errors.Any())
                .SelectMany(m => m.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

                if (activeIngredientErrors.Any())
                {
                    TempData["ActiveIngredientErrors"] = "Musisz wybrać oprysk i jednostkę. Ilość musi być liczbą całkowitą większą od zera.";
                }
                return View(createSprayingWorkDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(createSprayingWorkDto), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/sprayingWork", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowa praca została dodana pomyślnie.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(createSprayingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(createSprayingWorkDto);
            }
        }
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public async Task<IActionResult> CreateOtherWork()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();

                return View();
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


        [Authorize(Policy = "OwnerOrWorker")]
        [HttpPost]
        public async Task<IActionResult> CreateOtherWork(CreateOtherWorkDto createOtherWorkDto)
        {
            if (!ModelState.IsValid)
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                ViewBag.Fields = await GetFields();
                if (!User.IsInRole("Pracownik"))
                {
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                }
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();

                return View();
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(createOtherWorkDto), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/otherWork", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowa praca została dodana pomyślnie.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(createOtherWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(createOtherWorkDto);
            }
        }

        [HttpPost]
        [Authorize(Policy = "OwnerOrWorker")]
        public async Task<IActionResult> FinishWork(int fieldWorkId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"] ?? Request.Headers["Authorization"].ToString();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var finishWorkDto = new FinishWorkDto
                {
                    FieldWorkId = fieldWorkId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var content = new StringContent(JsonConvert.SerializeObject(finishWorkDto), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("/api/fieldworks/finish", content);

                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Praca zakończona pomyślnie.";
                return RedirectToAction("Index"); 
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(); 
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd.";
                return View(); 
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fieldWorks/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Praca polowa została pomyślnie usunięta!";
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



        //Edit
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> EditFertilizingWork(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");
                response.EnsureSuccessStatusCode();

                var fertilizingWork = JsonConvert.DeserializeObject<FertilizingWorkDto>(await response.Content.ReadAsStringAsync());

                if (fertilizingWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                var createFertilizingWorkDto = new CreateFertilizingWorkDto
                {
                    FieldId = fertilizingWork.FieldId,
                    EmployeeId = fertilizingWork.EmployeeId,
                    StartTime = fertilizingWork.StartTime,
                    EndTime = fertilizingWork.EndTime,
                    AgroControlUserId = fertilizingWork.AgroControlUserId,
                    IsPlanned = fertilizingWork.IsPlanned,
                    Description = fertilizingWork.Description,
                    FieldWorkAgriculturalEquipmentIds = fertilizingWork.FieldWorkAgriculturalEquipment
                        .Select(equipment => equipment.AgriculturalEquipment.Id)
                        .ToList(),
                    FertilizingWorkFertilizers = fertilizingWork.FertilizingWorkFertilizers.Select(fertilizer => new CreateFertilizingWorkFertilizerDto
                    {
                        Quantity = fertilizer.Quantity,
                        FertilizerId = fertilizer.Fertilizer.Id,
                        UnitId = fertilizer.Unit.Id,
                    }).ToList()

                };

                ViewBag.Fields = await GetFields();
                ViewBag.Fertilizers = await GetFertilizers();
                ViewBag.Employees = await GetEmployees();
                ViewBag.Users = await GetUsers();
                ViewBag.Units = await GetUnits();
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();

                return View(createFertilizingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> EditFertilizingWork(int id, CreateFertilizingWorkDto fertilizingWorkDto)
        {
            if (!ModelState.IsValid)
            {

                try
                {
                    ViewBag.Fields = await GetFields();
                    ViewBag.Fertilizers = await GetFertilizers();
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                    ViewBag.Units = await GetUnits();
                    ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                    return RedirectToAction("Index");
                }
                return View(fertilizingWorkDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(fertilizingWorkDto), Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/api/fertilizingWork/{id}", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Praca została pomyślnie zaktualizowana.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(fertilizingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(fertilizingWorkDto);
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> EditHarvestingWork(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");
                response.EnsureSuccessStatusCode();

                var harvestingWork = JsonConvert.DeserializeObject<HarvestingWorkDto>(await response.Content.ReadAsStringAsync());

                if (harvestingWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                var createHarvestingWorkDto = new CreateHarvestingWorkDto
                {
                    FieldId = harvestingWork.FieldId,
                    EmployeeId = harvestingWork.EmployeeId,
                    StartTime = harvestingWork.StartTime,
                    EndTime = harvestingWork.EndTime,
                    AgroControlUserId = harvestingWork.AgroControlUserId,
                    IsPlanned = harvestingWork.IsPlanned,
                    Description = harvestingWork.Description,
                    FieldWorkAgriculturalEquipmentIds = harvestingWork.FieldWorkAgriculturalEquipment
                        .Select(equipment => equipment.AgriculturalEquipment.Id)
                        .ToList(),
                    TotalYield = harvestingWork.TotalYield,
                    Moisture = harvestingWork.Moisture,
                    CropId = harvestingWork.Crop.Id

                };

                ViewBag.Fields = await GetFields();
                ViewBag.Employees = await GetEmployees();
                ViewBag.Users = await GetUsers();
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();

                return View(createHarvestingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> EditHarvestingWork(int id, CreateHarvestingWorkDto harvestingWorkDto)
        {
            if (!ModelState.IsValid)
            {

                try
                {
                    ViewBag.Fields = await GetFields();
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                    ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                    ViewBag.Crops = await GetCrops();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                    return RedirectToAction("Index");
                }
                return View(harvestingWorkDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(harvestingWorkDto), Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/api/harvestingWork/{id}", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Praca została pomyślnie zaktualizowana.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(harvestingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(harvestingWorkDto);
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> EditOtherWork(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");
                response.EnsureSuccessStatusCode();

                var otherWork = JsonConvert.DeserializeObject<OtherWorkDto>(await response.Content.ReadAsStringAsync());

                if (otherWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                var createHarvestingWorkDto = new CreateOtherWorkDto
                {
                    Name = otherWork.Name,
                    FieldId = otherWork.FieldId,
                    EmployeeId = otherWork.EmployeeId,
                    StartTime = otherWork.StartTime,
                    EndTime = otherWork.EndTime,
                    AgroControlUserId = otherWork.AgroControlUserId,
                    IsPlanned = otherWork.IsPlanned,
                    Description = otherWork.Description,
                    FieldWorkAgriculturalEquipmentIds = otherWork.FieldWorkAgriculturalEquipment
                        .Select(equipment => equipment.AgriculturalEquipment.Id)
                        .ToList(),

                };

                ViewBag.Fields = await GetFields();
                ViewBag.Employees = await GetEmployees();
                ViewBag.Users = await GetUsers();
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                ViewBag.Crops = await GetCrops();

                return View(createHarvestingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> EditOtherWork(int id, CreateOtherWorkDto harvestingWorkDto)
        {
            if (!ModelState.IsValid)
            {

                try
                {
                    ViewBag.Fields = await GetFields();
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                    ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                    ViewBag.Crops = await GetCrops();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                    return RedirectToAction("Index");
                }
                return View(harvestingWorkDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(harvestingWorkDto), Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/api/otherWork/{id}", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Praca została pomyślnie zaktualizowana.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(harvestingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(harvestingWorkDto);
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> EditSprayingWork(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");
                response.EnsureSuccessStatusCode();

                var sprayingWork = JsonConvert.DeserializeObject<SprayingWorkDto>(await response.Content.ReadAsStringAsync());

                if (sprayingWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                var createSprayingWorkDto = new CreateSprayingWorkDto
                {
                    FieldId = sprayingWork.FieldId,
                    EmployeeId = sprayingWork.EmployeeId,
                    StartTime = sprayingWork.StartTime,
                    EndTime = sprayingWork.EndTime,
                    AgroControlUserId = sprayingWork.AgroControlUserId,
                    IsPlanned = sprayingWork.IsPlanned,
                    Description = sprayingWork.Description,
                    FieldWorkAgriculturalEquipmentIds = sprayingWork.FieldWorkAgriculturalEquipment
                        .Select(equipment => equipment.AgriculturalEquipment.Id)
                        .ToList(),
                    SprayingWorkCropProtectionProducts = sprayingWork.SprayingWorkCropProtectionProducts.Select(cpp => new CreateSprayingWorkCropProtectionProductDto
                    {
                        Quantity = cpp.Quantity,
                        CropProtectionProductId = cpp.CropProtectionProduct.Id,
                        UnitId = cpp.Unit.Id,
                    }).ToList()

                };

                ViewBag.Fields = await GetFields();
                ViewBag.CropProtectionProducts = await GetCropProtectionProducts();
                ViewBag.Employees = await GetEmployees();
                ViewBag.Users = await GetUsers();
                ViewBag.Units = await GetUnits();
                ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();

                return View(createSprayingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> EditSprayingWork(int id, CreateSprayingWorkDto sprayingWorkDto)
        {
            if (!ModelState.IsValid)
            {

                try
                {
                    ViewBag.Fields = await GetFields();
                    ViewBag.Employees = await GetEmployees();
                    ViewBag.Users = await GetUsers();
                    ViewBag.AgriculturalEquipment = await GetAgriculturalEquipment();
                    ViewBag.CropProtectionProducts = await GetCropProtectionProducts();
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = "Wystąpił błąd przy ładowaniu danych.";
                    return RedirectToAction("Index");
                }
                return View(sprayingWorkDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(JsonConvert.SerializeObject(sprayingWorkDto), Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/api/sprayingWork/{id}", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Praca została pomyślnie zaktualizowana.";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(sprayingWorkDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return View(sprayingWorkDto);
            }
        }








        //Details
        public async Task<IActionResult> FertilizingWorkDetails(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");

                response.EnsureSuccessStatusCode();

                var fertilizingWork = JsonConvert.DeserializeObject<FertilizingWorkDto>(await response.Content.ReadAsStringAsync());

                if (fertilizingWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy nawożenia o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                return View(fertilizingWork);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> HarvestingWorkDetails(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");

                response.EnsureSuccessStatusCode();

                var harvestingWork = JsonConvert.DeserializeObject<HarvestingWorkDto>(await response.Content.ReadAsStringAsync());

                if (harvestingWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono zbioru o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                return View(harvestingWork);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> SprayingWorkDetails(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");

                response.EnsureSuccessStatusCode();

                var sprayingWork = JsonConvert.DeserializeObject<SprayingWorkDto>(await response.Content.ReadAsStringAsync());

                if (sprayingWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy wykonywania oprysków o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                return View(sprayingWork);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> OtherWorkDetails(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"/api/fieldWorks/{id}");

                response.EnsureSuccessStatusCode();

                var otherWork = JsonConvert.DeserializeObject<OtherWorkDto>(await response.Content.ReadAsStringAsync());

                if (otherWork == null)
                {
                    TempData["errorMessage"] = "Nie znaleziono pracy polowej o podanym identyfikatorze.";
                    return RedirectToAction("Index");
                }
                return View(otherWork);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
        }
        private async Task<List<SelectListItem>> GetFields()
        {
            var response = await _client.GetAsync("/api/fields");
            response.EnsureSuccessStatusCode();
            var fields = JsonConvert.DeserializeObject<List<FieldDto>>(await response.Content.ReadAsStringAsync());

            return fields.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
        }

        private async Task<List<SelectListItem>> GetFertilizers()
        {
            var response = await _client.GetAsync("/api/fertilizers");
            response.EnsureSuccessStatusCode();
            var fertilizers = JsonConvert.DeserializeObject<List<FertilizerDto>>(await response.Content.ReadAsStringAsync());

            return fertilizers.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
        }
        private async Task<List<SelectListItem>> GetCropProtectionProducts()
        {
            var response = await _client.GetAsync("/api/cropProtectionProducts");
            response.EnsureSuccessStatusCode();
            var cpp = JsonConvert.DeserializeObject<List<CropProtectionProductDto>>(await response.Content.ReadAsStringAsync());

            return cpp.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
        }
        private async Task<List<SelectListItem>> GetEmployees()
        {
            var response = await _client.GetAsync("/api/employees");
            response.EnsureSuccessStatusCode();
            var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(await response.Content.ReadAsStringAsync());

            return employees.Where(e => e.IsActive).Select(e => new SelectListItem { Value = e.Id.ToString(), Text = $"{e.FirstName} {e.LastName}" }).ToList();
        }

        private async Task<List<SelectListItem>> GetUsers()
        {
            var response = await _client.GetAsync("/api/farms/users");
            response.EnsureSuccessStatusCode();
            var users = JsonConvert.DeserializeObject<List<AgroControlUserDto>>(await response.Content.ReadAsStringAsync());

            return users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.FirstName} {u.LastName} ({u.RoleName})" }).ToList();
        }

        private async Task<List<SelectListItem>> GetUnits()
        {
            var response = await _client.GetAsync("/api/units");
            response.EnsureSuccessStatusCode();
            var units = JsonConvert.DeserializeObject<List<UnitDto>>(await response.Content.ReadAsStringAsync());

            return units.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
        }

        private async Task<List<SelectListItem>> GetAgriculturalEquipment()
        {
            var response = await _client.GetAsync("/api/agriculturalEquipment");
            response.EnsureSuccessStatusCode();
            var equipment = JsonConvert.DeserializeObject<List<AgriculturalEquipmentDto>>(await response.Content.ReadAsStringAsync());

            return equipment.Where(e => e.IsActive).Select(e => new SelectListItem { Value = e.Id.ToString(), Text = $"{e.Name} ({e.AgriculturalEquipmentTypeName})" }).ToList();
        }
        private async Task<List<SelectListItem>> GetCrops()
        {
            var response = await _client.GetAsync("/api/crops");
            response.EnsureSuccessStatusCode();
            var crops = JsonConvert.DeserializeObject<List<CropDto>>(await response.Content.ReadAsStringAsync());

            return crops.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();
        }
        // Dispose
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
