using AgroControlUI.Constants;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.Fuel
{
    public class FuelController : Controller
    {
        private readonly HttpClient _client;
        public FuelController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }
        //GetAll
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/fuels";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var fuels = JsonConvert.DeserializeObject<List<FuelDto>>(content);
            return View(fuels);
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
        public IActionResult Create(FuelDto fuelDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fuelDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/fuels";
                var content = JsonConvert.SerializeObject(fuelDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Nowy rodzaj paliwa został pomyślnie dodany!";
                return RedirectToAction("Index");

            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(fuelDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(fuelDto);
            }
        }

        //Delete
        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fuels/{id}";
                var result = _client.DeleteAsync(endpoint).Result;
                result.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Rodzaj paliwa został pomyślnie usunięty!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return RedirectToAction("Index");
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

                var endpoint = $"/api/fuels/{id}";
                var response = _client.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                var fuel = JsonConvert.DeserializeObject<FuelDto>(content);
                return View(fuel);
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
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public IActionResult Edit(FuelDto fuelDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fuelDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/fuels/{fuelDto.Id}";
                var content = JsonConvert.SerializeObject(fuelDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PutAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Rodzaj paliwa został pomyślnie zaaktualizowany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(fuelDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(fuelDto);
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
