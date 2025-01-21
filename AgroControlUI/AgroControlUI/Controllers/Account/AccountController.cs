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
    public async Task<IActionResult> LoginAsync(LoginModel login)
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
            endpoint = $"/api/Admin/{login.Email}";
            //var userData = _services.GetFromApi<List<string>>(endpoint, _client, token);

            //HttpContext.Session.SetString("token", token);

            //var firstLogin = Boolean.Parse(userData[1]);
            //if (firstLogin)
            //{
            //    TempData["UserName"] = login.Email;
            //    TempData["UserRole"] = userData[0];
            //    TempData["token"] = token;
            //    TempData["refreshToken"] = refreshToken;
            //    return RedirectToAction("ResetPassword", "Identity");
            //}
            //else
            //{
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

            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                TempData["loginError"] = "Błędne dane logowania";
            }
            else
            {
                TempData["loginError"] = "Błąd logowania";
            }
            return View();
        }
    }
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterModel register)
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
                return RedirectToAction("Login", "Account");
            }

            TempData["registerError"] = "Registration failed. Please try again.";
            return View(register);
        }
        catch (HttpRequestException)
        {
            TempData["registerError"] = "An error occurred while processing your request.";
            return View(register);
        }
    }

    public IActionResult AccessDenied()
    {
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
