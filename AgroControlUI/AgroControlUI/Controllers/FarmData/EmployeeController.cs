using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.FarmData
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _client;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        // GetAll
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/employees";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(content);

            return View(employees);
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
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/employees";
                var content = JsonConvert.SerializeObject(employeeDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Nowy pracownik został pomyślnie dodany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(employeeDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(employeeDto);
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

                var endpoint = $"/api/employees/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Pracownik został pomyślnie usunięty!";
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

                var endpoint = $"/api/employees/{id}";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<EmployeeDto>(content);
                return View(employee);
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View();
            }
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/employees/{employeeDto.Id}";
                var content = JsonConvert.SerializeObject(employeeDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Pracownik został pomyślnie zaaktualizowany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(employeeDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(employeeDto);
            }
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

                var endpoint = $"/api/employees/{id}/updateIsActive";

                var content = JsonConvert.SerializeObject(isActive);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await _client.PatchAsync(endpoint, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Stan aktywności pracownika został zaktualizowany!";
                }
                else
                {
                    TempData["errorMessage"] = "Wystąpił problem z aktualizacją stanu pracownika.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później.";
            }

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
