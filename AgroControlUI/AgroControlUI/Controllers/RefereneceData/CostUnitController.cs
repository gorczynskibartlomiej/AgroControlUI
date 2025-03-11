using AgroControlUI.Constants;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.RefereneceData
{
    public class CostUnitController : Controller
    {
        private readonly HttpClient _client;
        public CostUnitController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }
        //GetAll
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/costUnits";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var costUnits = JsonConvert.DeserializeObject<List<CostUnitDto>>(content);
            return View(costUnits);
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
        public IActionResult Create(CostUnitDto costUnitDto)
        {
            if (!ModelState.IsValid)
            {
                return View(costUnitDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/costUnits";
                var content = JsonConvert.SerializeObject(costUnitDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowa jednostka kosztów została pomyślnie dodana!";
                return RedirectToAction("Index");

            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(costUnitDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(costUnitDto);
            }
        }

        //Delete
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/costUnits/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Jednostka kosztów została pomyślnie usunięta!";
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
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/costUnits/{id}";
                var response = _client.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                var costUnit = JsonConvert.DeserializeObject<CostUnitDto>(content);
                return View(costUnit);
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
        public IActionResult Edit(CostUnitDto costUnitDto)
        {
            if (!ModelState.IsValid)
            {
                return View(costUnitDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/costUnits/{costUnitDto.Id}";
                var content = JsonConvert.SerializeObject(costUnitDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PutAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Jednostka kosztów została pomyślnie zaaktualizowana!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd żądania HTTP: " + ex.Message;
                return View(costUnitDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd: " + ex.Message;
                return View(costUnitDto);
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
