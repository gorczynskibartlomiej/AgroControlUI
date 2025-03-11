
using AgroControlUI.Constants;
using AgroControlUI.DTOs.UserManagement;
using AgroControlUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AgroControlUI.Controllers
{
    public class InvitationController : Controller
    {
        private readonly HttpClient _client;

        public InvitationController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }
        [Authorize]
        [HttpGet("Invitation/Accept/{invitationId}")]
        public IActionResult Accept(int invitationId)
        {
            return View(invitationId);
        }
        [Authorize]
        [HttpPost("Accept/{invitationId}")]
        public async Task<IActionResult> AcceptInvitation(int invitationId)
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync($"/api/invitations/accept/{invitationId}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Zaproszenie zaakceptowane!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Błąd akceptacji. Sprawdź ważność zaproszenia lub zaloguj się na odpowiednie konto.";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
