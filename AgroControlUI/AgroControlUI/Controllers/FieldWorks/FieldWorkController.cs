using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.Fertilizers;
using AgroControlUI.DTOs.FieldWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Index(string filterType, bool? isPlanned)
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/fieldworks";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var fieldWorks = JsonConvert.DeserializeObject<List<FieldWorkDto>>(content);

            // Filtrowanie
            if (!string.IsNullOrEmpty(filterType))
            {
                //fieldWorks = fieldWorks.Where(fw => fw.Type == filterType).ToList();
            }

            if (isPlanned.HasValue)
            {
                fieldWorks = fieldWorks.Where(fw => fw.IsPlanned == isPlanned.Value).ToList();
            }

            return View(fieldWorks);
        }

        // Create
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> CreateFertilizingWork()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Pobieranie listy pól
            var fieldsResponse = await _client.GetAsync("/api/fields");
            fieldsResponse.EnsureSuccessStatusCode();
            var fields = JsonConvert.DeserializeObject<List<FieldDto>>(await fieldsResponse.Content.ReadAsStringAsync());

            ViewBag.Fields = fields.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList();

            // Pobieranie listy nawozów
            var fertilizersResponse = await _client.GetAsync("/api/fertilizers");
            fertilizersResponse.EnsureSuccessStatusCode();
            var fertilizers = JsonConvert.DeserializeObject<List<FertilizerDto>>(await fertilizersResponse.Content.ReadAsStringAsync());

            ViewBag.Fertilizers = fertilizers.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            }).ToList();

            return View();
        }


        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> CreateFertilizingWork(int FieldId, string Description, List<int> SelectedFertilizerIds)
        {
            if (FieldId == 0)
            {
                ModelState.AddModelError("FieldId", "Pole jest wymagane.");
            }

            if (!ModelState.IsValid)
            {
                // Pobieranie listy pól i nawozów na wypadek błędów walidacji
                var fieldsResponse = await _client.GetAsync("/api/fields");
                fieldsResponse.EnsureSuccessStatusCode();
                var fields = JsonConvert.DeserializeObject<List<FieldDto>>(await fieldsResponse.Content.ReadAsStringAsync());
                ViewBag.Fields = fields.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList();

                var fertilizersResponse = await _client.GetAsync("/api/fertilizers");
                fertilizersResponse.EnsureSuccessStatusCode();
                var fertilizers = JsonConvert.DeserializeObject<List<FertilizerDto>>(await fertilizersResponse.Content.ReadAsStringAsync());
                ViewBag.Fertilizers = fertilizers.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Name
                }).ToList();

                return View();
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Przygotowanie danych do wysłania
                var payload = new
                {
                    FieldId,
                    Description,
                    SelectedFertilizerIds
                };

                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/fertilizingworks", content);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Dodano pracę nawożenia!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił błąd: " + ex.Message;
                return View();
            }
        }



        // Tworzenie - Opryski
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public IActionResult CreateSprayingWork()
        {
            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> CreateSprayingWork(SprayingWorkDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/sprayingWorks/create";
                var content = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Dodano nowy oprysk!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(model);
            }
        }

        // Tworzenie - Zbiory
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public IActionResult CreateHarvestingWork()
        {
            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> CreateHarvestingWork(HarvestingWorkDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/harvestingWorks/create";
                var content = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Dodano nowe zbiory!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(model);
            }
        }

        // Tworzenie - Inne
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public IActionResult CreateOtherWork()
        {
            return View();
        }

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> CreateOtherWork(OtherWorkDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/otherWorks/create";
                var content = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Dodano nową pracę!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(model);
            }
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
