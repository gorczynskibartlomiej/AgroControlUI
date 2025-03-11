using AgroControlUI.DTOs.Account;
using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using AgroControlUI.Constants;
using System.Net.Http.Headers;
using System.Net.Http;

public class AccountController : Controller
{
    private readonly HttpClient _client;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri(Options.ApiUrl);
    }
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            // Jeśli ReturnUrl wskazuje na zaproszenie do farmy, przekieruj użytkownika do akcji dołączenia do farmy
            if (returnUrl.Contains("Invitation"))
            {
                var decodedReturnUrl = Uri.UnescapeDataString(returnUrl);
                var invitationId = decodedReturnUrl.Split('/').Last();
                return RedirectToAction("Accept", "Invitation", new { invitationId });
            }
        }
        return RedirectToAction("Select", "Farm"); // Domyślnie przekierowuje na stronę główną
    }
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        // Sprawdzamy, czy mamy ReturnUrl, jeśli nie, ustawiamy domyślną stronę
        ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/"); // Default to home page if ReturnUrl is null
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginModelDto login, string returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            var endpoint = "/login";
            var content = JsonConvert.SerializeObject(login);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(endpoint, stringContent).Result;
            response.EnsureSuccessStatusCode();

            var json = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            var token = (string)json.accessToken;
            var refreshToken = (string)json.refreshToken;
            var expiresIn = (int)json.expiresIn;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var isAdminResponse = await _client.GetAsync("/api/Account/isAdmin");
            var isAdminJson = JsonConvert.DeserializeObject<dynamic>(await isAdminResponse.Content.ReadAsStringAsync());
            bool isAdmin = (bool)isAdminJson.isAdmin;

            var claims = new List<Claim>();
            
            claims.Add(new Claim(ClaimTypes.Name, login.Email));
            

            if (isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
            );
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddSeconds(expiresIn)
            };

            HttpContext.Response.Cookies.Append("token", token, cookieOptions);
            cookieOptions.Expires = DateTimeOffset.Now.AddSeconds(expiresIn*20);
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            TempData["successMessage"] = "Zalogowano pomyślnie.";
            if (isAdmin)
            {
                return RedirectToAction("Index", "Home");
            }
                return RedirectToLocal(returnUrl);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                TempData["errorMessage"] = "Błędne dane logowania.";
            }
            else
            {
                TempData["errorMessage"] = "Błąd logowania. Spróbuj ponownie później.";
            }
            return View();
        }
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterModelDto register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        try
        {
            var endpoint = "/api/Account/register";
            var content = JsonConvert.SerializeObject(register);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var loginModel = new LoginModelDto { Email = register.Email, Password = register.Password};
                LoginAsync(loginModel,null);
                return RedirectToAction("Select", "Farm");
            }

            TempData["errorMessage"] = "Wystąpił błąd podczas rejestracji użytkownika. Spróbuj ponownie później.";
            return View(register);
        }
        catch (HttpRequestException)
        {
            TempData["errorMessage"] = "Wystąpił błąd podczas rejestracji użytkownika. Spróbuj ponownie później.";
            return View(register);
        }
    }

    public IActionResult AccessDenied()
    {
        TempData["errorMessage"] = "Nie masz uprawnień do tej strony. Zaloguj się na odpowiednie konto.";
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("token");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
