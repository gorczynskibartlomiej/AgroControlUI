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
            var token = HttpContext.Request.Cookies["token"];
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
            var token = HttpContext.Request.Cookies["token"];
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
                existingClaims.RemoveAll(c => c.Type == ClaimTypes.Role || c.Type == "FarmId");
                foreach (var role in roles)
                {
                    existingClaims.Add(new Claim(ClaimTypes.Role, role.RoleName));
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
                    Expires = DateTimeOffset.UtcNow.AddSeconds(1000)
                };
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
