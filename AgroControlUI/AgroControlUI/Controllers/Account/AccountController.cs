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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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

            if (returnUrl.Contains("Invitation"))
            {
                var decodedReturnUrl = Uri.UnescapeDataString(returnUrl);
                var invitationId = decodedReturnUrl.Split('/').Last();
                return RedirectToAction("Accept", "Invitation", new { invitationId });
            }
        }
        return RedirectToAction("Select", "Farm"); 
    }
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        if (HttpContext.Items.ContainsKey("LogoutMessage"))
        {
            TempData["errorMessage"] = HttpContext.Items["LogoutMessage"];
        }
        ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
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
            var endpointreg = "/api/Account/register";
            var contentreg = JsonConvert.SerializeObject(register);
            var stringContentreg = new StringContent(contentreg, Encoding.UTF8, "application/json");

            var responsereg = await _client.PostAsync(endpointreg, stringContentreg);

            if (responsereg.IsSuccessStatusCode)
            {
                var login = new LoginModelDto { Email = register.Email, Password = register.Password};
                TempData["successMessage"] = "Twoje konto zostało pomyślnie utworzone.";
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
                    cookieOptions.Expires = DateTimeOffset.Now.AddSeconds(expiresIn * 20);
                    HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
                
                return RedirectToAction("Select", "Farm");
            }
            else if (responsereg.StatusCode == HttpStatusCode.Conflict)
            {
                TempData["errorMessage"] = "Użytkownik z tym adresem e-mail już istnieje.";
                return View(register);
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

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        try
        {
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var endpoint = "/api/account/changePassword";
            var content = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, stringContent);
            response.EnsureSuccessStatusCode();

            HttpContext.Response.Cookies.Delete("token");
            HttpContext.Response.Cookies.Delete("refreshToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["successMessage"] = "Hasło zostało zmienione. Zaloguj się ponownie.";
            return RedirectToAction("Login", "Account");
            
        }
        catch (HttpRequestException ex)
        {
            TempData["errorMessage"] = "Podaj poprawne hasło!";
            return View(model);
        }
    }
    [HttpGet]
    public IActionResult Details()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Delete()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed()
    {
        try
        {
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync("/api/account/delete");
            response.EnsureSuccessStatusCode();

            response.EnsureSuccessStatusCode();
            HttpContext.Response.Cookies.Delete("token");
            HttpContext.Response.Cookies.Delete("refreshToken");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["successMessage"] = "Konto zostało usunięte!";
            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException ex)
        {
            TempData["errorMessage"] = "Błąd serwera, spróbuj ponownie później.";
            return View();
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Wystąpił nieoczekiwany błąd.";
            return View();
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
        HttpContext.Response.Cookies.Delete("refreshToken");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
