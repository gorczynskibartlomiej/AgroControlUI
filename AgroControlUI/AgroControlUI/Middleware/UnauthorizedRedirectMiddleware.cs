using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AgroControlUI.Middleware
{
    public class UnauthorizedRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnauthorizedRedirectMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            var responseStatusCode = context.Response.StatusCode;

            if (responseStatusCode == 401)
            {
                // Jeśli odpowiedź to 401, przekieruj użytkownika na stronę logowania
                context.Response.Redirect("/Account/Login");
                return;
            }

            // Jeśli odpowiedź nie jest 401, przejdź do następnego middleware
            await _next(context);
        }
    }

}
