namespace AgroControlUI.Controllers.FarmData
{
    using AgroControlUI.Constants;
    using AgroControlUI.DTOs.FarmData;
    using AgroControlUI.DTOs.ReferenceData;
    using AgroControlUI.DTOs.UserManagement;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class FarmController : Controller
    {
        private readonly HttpClient _client;
        public FarmController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Select()
        {
            var endpoint = "/api/farms/userFarms/";
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var farms = JsonConvert.DeserializeObject<List<AgroControlUserFarmDto>>(content);

            return View(farms);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetCurrentFarm(int farmId)
        {
            var endpoint = "/api/farms/setCurrentFarm";
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = JsonConvert.SerializeObject(farmId);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(endpoint, stringContent).Result;
            response.EnsureSuccessStatusCode();

            endpoint = $"/api/farms/userRolesInFarm/{farmId}";
            response = _client.GetAsync(endpoint).Result;
            response.EnsureSuccessStatusCode();
            content = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<AgroControlUserFarmDto>>(content);

            var currentUser = HttpContext.User;

            if (currentUser.Identity.IsAuthenticated)
            {
                var existingClaims = currentUser.Claims.ToList();
                existingClaims.RemoveAll(c => c.Type == ClaimTypes.Role || c.Type == "FarmId"||c.Type=="FarmName");
                foreach (var role in roles)
                {
                    existingClaims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                    existingClaims.Add(new Claim("FarmName", role.FarmName));
                }
                existingClaims.Add(new Claim("FarmId", farmId.ToString())); 

                var claimsIdentity = new ClaimsIdentity(existingClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.Now.AddSeconds(1000)
                };
            }
            return RedirectToAction("Index", "Home");
        }
        //Get
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/farms";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            var farm = JsonConvert.DeserializeObject<FarmDto>(content);
            return View(farm);
        }
        // Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(FarmDto farmDto)
        {
            if (!ModelState.IsValid)
            {
                return View(farmDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = "/api/farms";
                var content = JsonConvert.SerializeObject(farmDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();
                TempData["successMessage"] = "Nowe gospodarstwo zostało pomyślnie dodane!";
                return RedirectToAction("select");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Gospodarstwo o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(farmDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(farmDto);
            }
        }

        // Delete

        [Authorize(Policy = "Owner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/farms/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var currentUser = HttpContext.User;
                if (currentUser.Identity.IsAuthenticated)
                {
                    var existingClaims = currentUser.Claims.ToList();
                    existingClaims.RemoveAll(c => c.Type == "FarmId" || c.Type == "FarmName");

                    var claimsIdentity = new ClaimsIdentity(existingClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    );
                }
                TempData["successMessage"] = "Gospodarstwo zostało pomyślnie usunięte!";

                return RedirectToAction("select", "Farm");
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
                return RedirectToAction("index", "Farm");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return RedirectToAction("index", "Farm");
            }
        }
        [Authorize(Policy = "OwnerOrWorker")]
        [HttpPost]
        public async Task<IActionResult> LeaveFarm()
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/farms/leaveFarm";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var currentUser = HttpContext.User;
                if (currentUser.Identity.IsAuthenticated)
                {
                    var existingClaims = currentUser.Claims.ToList();
                    existingClaims.RemoveAll(c => c.Type == "FarmId" || c.Type == "FarmName");

                    var claimsIdentity = new ClaimsIdentity(existingClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    );
                }
                TempData["successMessage"] = "Pomyślnie opuszczono gospodarstwo!";

                return RedirectToAction("select", "Farm");
            }
            catch (HttpRequestException ex)
            {
                TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                return RedirectToAction("index", "Farm");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return RedirectToAction("index", "Farm");
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

                var endpoint = $"/api/farms";
                var response = await _client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var farm = JsonConvert.DeserializeObject<FarmDto>(content);
                return View(farm);
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
        public async Task<IActionResult> Edit(FarmDto farmDto)
        {
            if (!ModelState.IsValid)
            {
                return View(farmDto);
            }

            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/farms/{farmDto.Id}";
                var content = JsonConvert.SerializeObject(farmDto);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();

                TempData["successMessage"] = "Gospodarswto zostało pomyślnie zaaktualizowane!";
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["errorMessage"] = "Gospodarstwo o tej nazwie już istnieje!";
                }
                else
                {
                    TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
                }
                return View(farmDto);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie później. ";
                return View(farmDto);
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
