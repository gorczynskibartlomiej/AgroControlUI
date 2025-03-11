using AgroControlUI.Constants;
using AgroControlUI.DTOs.ReferenceData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.Crop
{
    public class CropController:Controller
    {
        private readonly HttpClient _client;
        public CropController(IHttpClientFactory httpClientFactory)
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

            var endpoint = "/api/crops";
            var result = await _client.GetAsync(endpoint);

            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var crops = JsonConvert.DeserializeObject<List<CropDto>>(content);
            return View(crops);           
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
        public IActionResult Create(CropDto cropDto)
        {
            if (!ModelState.IsValid)
            {
                return View(cropDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/crops";
                var content = JsonConvert.SerializeObject(cropDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Nowy rodzaj uprawy został pomyślnie dodany!";
                return RedirectToAction("Index");

            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(cropDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(cropDto);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/crops/{id}";
                var result = await _client.DeleteAsync(endpoint);
                result.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Rodzaj uprawy został pomyślnie usunięty!";
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

                var endpoint = $"/api/crops/{id}";
                var response = _client.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                var crop = JsonConvert.DeserializeObject<CropDto>(content);
                return View(crop);
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
        public IActionResult Edit(CropDto cropDto)
        {
            if (!ModelState.IsValid)
            {
                return View(cropDto);
            }
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/crops/{cropDto.Id}";
                var content = JsonConvert.SerializeObject(cropDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = _client.PutAsync(endpoint, stringContent).Result;
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Rodzaj uprawy został pomyślnie zaaktualizowany!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(cropDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(cropDto);
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
