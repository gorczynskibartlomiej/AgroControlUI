using AgroControlUI.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AgroControlUI.Models.FarmData;

namespace AgroControlUI.Middleware
{
    public class TokenManagementMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _client;
        public TokenManagementMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }

        public async Task InvokeAsync(HttpContext context)
        
         {
            string accessToken = context.Request.Cookies["token"];
            string refreshToken = context.Request.Cookies["refreshToken"];

            if (refreshToken != null && accessToken == null)
            {
                var existingClaims = context.User.Claims.ToList();
                var newTokens = await RefreshTokens(refreshToken);
                if(newTokens.newAccessToken == null)
                {
                    // Jeśli refresh token jest nieważny, wyloguj użytkownika
                    context.Response.Cookies.Delete("token");
                    context.Response.Cookies.Delete("refreshToken");
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/Account/Login");
                    return;
                }
                ///1
                //var cookieOptions = new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    SameSite = SameSiteMode.None,
                //    Expires = DateTimeOffset.Now.AddSeconds(newTokens.expiresIn)
                //};
                //context.Response.Cookies.Append("token", newTokens.newAccessToken, cookieOptions);
                //cookieOptions.Expires = DateTimeOffset.Now.AddSeconds(newTokens.expiresIn * 20);
                //context.Response.Cookies.Append("refreshToken", newTokens.newRefreshToken, cookieOptions);

                //context.Response.Headers["token"] = newTokens.newAccessToken;
                //context.Request.Headers["token"] = newTokens.newAccessToken;
                //cookieOptions.Expires = DateTimeOffset.Now.AddSeconds(newTokens.expiresIn);
                //context.Request.Headers["Authorization"] = $"Bearer {newTokens.newAccessToken}";

                //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.newAccessToken);


                //2
                //context.Request.Headers["Authorization"] = $"Bearer {newTokens.newAccessToken}";

                //// Wymuś zapisanie nowego tokena przez przeglądarkę
                //context.Response.Headers.Append("Set-Cookie",
                //    $"token={newTokens.newAccessToken}; HttpOnly; Secure; SameSite=None; Path=/; Expires={DateTime.UtcNow.AddSeconds(newTokens.expiresIn):R}");

                //context.Response.Headers.Append("Set-Cookie",
                //    $"refreshToken={newTokens.newRefreshToken}; HttpOnly; Secure; SameSite=None; Path=/; Expires={DateTime.UtcNow.AddSeconds(newTokens.expiresIn * 20):R}");

                //_client.DefaultRequestHeaders.Authorization = null;
                //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.newAccessToken);
                //await Task.Delay(5000);           
                //var claimsIdentity = new ClaimsIdentity(existingClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                //// Podmieniamy w kontekście użytkownika
                //await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.Now.AddSeconds(newTokens.expiresIn)
                };
                context.Response.Cookies.Append("token", newTokens.newAccessToken, cookieOptions);
                cookieOptions.Expires = DateTimeOffset.Now.AddSeconds(newTokens.expiresIn * 20);
                context.Response.Cookies.Append("refreshToken", newTokens.newRefreshToken, cookieOptions);

                // Ustawienie nagłówka Authorization z nowym tokenem
                context.Request.Headers["Authorization"] = $"{newTokens.newAccessToken}";

                // Opcjonalnie: Wymuś zapisanie nowego tokena w przeglądarkach
                //context.Response.Headers.Append("Set-Cookie",
                //    $"token={newTokens.newAccessToken}; HttpOnly; Secure; SameSite=None; Path=/; Expires={DateTime.Now.AddSeconds(newTokens.expiresIn):R}");
                //context.Response.Headers.Append("Set-Cookie",
                //    $"refreshToken={newTokens.newRefreshToken}; HttpOnly; Secure; SameSite=None; Path=/; Expires={DateTime.Now.AddSeconds(newTokens.expiresIn * 20):R}");
                await Task.Delay(500);
            }

            await _next(context);


        }


        private async Task<(string newAccessToken, string newRefreshToken, int expiresIn)> RefreshTokens(string refreshToken)
        {
                var endpoint = "/refresh";
                var content = JsonConvert.SerializeObject(new { refreshToken = refreshToken });
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(endpoint, stringContent);
                response.EnsureSuccessStatusCode();
                var json = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                var newAccessToken = (string)json.accessToken;
                var newRefreshToken = (string)json.refreshToken;
                var expiresIn = (int)json.expiresIn;
                return (newAccessToken, newRefreshToken, expiresIn);
        }
    }
}
