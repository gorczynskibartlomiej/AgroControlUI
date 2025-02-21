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

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginModelDto login)
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

            var claims = new[]
            {
                  new Claim(ClaimTypes.Name, login.Email)
            };

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
                Expires = DateTimeOffset.UtcNow.AddSeconds(expiresIn)
            };
            HttpContext.Response.Cookies.Append("token", token, cookieOptions);
            cookieOptions.Expires = DateTimeOffset.UtcNow.AddSeconds(expiresIn);
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            TempData["successMessage"] = "Zalogowano pomyślnie.";
            return RedirectToAction("Select", "Farm");
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
                LoginAsync(loginModel);
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
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("token");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
