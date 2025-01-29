using AgroControlUI.Constants;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.RefereneceData
{
    public class UnitController : Controller
    {
        private readonly HttpClient _client;
        public UnitController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }
        //GetAll
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/units";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var units = JsonConvert.DeserializeObject<List<UnitDto>>(content);
            return View(units);
        }

        //Create
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Create(UnitDto unitDto)
        {
            if (!ModelState.IsValid)
            {
                return View(unitDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/units";
                var content = JsonConvert.SerializeObject(unitDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();
               
                return RedirectToAction("Index");

            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(unitDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(unitDto);
            }
        }

        //Delete
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/units/{id}";
                var response = _client.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                var unit = JsonConvert.DeserializeObject<UnitDto>(content);
                return View(unit);
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
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/units/{id}";
                var result = _client.DeleteAsync(endpoint).Result;
                result.EnsureSuccessStatusCode();
                //TempData["successMessage"] = "Uprawa została usunięta";
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

        //Edit
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/units/{id}";
                var response = _client.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                var unit = JsonConvert.DeserializeObject<UnitDto>(content);
                return View(unit);
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
        public IActionResult Edit(UnitDto unitDto)
        {
            if (!ModelState.IsValid)
            {
                return View(unitDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/units/{unitDto.Id}";
                var content = JsonConvert.SerializeObject(unitDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PutAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Zmodyfikowano pomyślnie";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(unitDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(unitDto);
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
