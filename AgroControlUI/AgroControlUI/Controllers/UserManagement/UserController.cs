using AgroControlUI.Constants;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AgroControlUI.Controllers.UserManagement
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri(Options.ApiUrl);
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public IActionResult Select()
        {
            return View();
        }
        // GetAll
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/farms/users";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();
            var usersContent = await result.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<AgroControlUserDto>>(usersContent);


            return View(users);
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/role/toAssign";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<AgroControlRoleDto>>(content)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,   
                    Value = r.Id.ToString()
                }).ToList();

            ViewBag.Roles = roles;
            return View();
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Add(InviteUserDto model)
        {
            if (!ModelState.IsValid)
            {
                var apitoken = HttpContext.Request.Cookies["token"]; if (apitoken == null) { apitoken = Request.Headers["Authorization"]; }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apitoken);

                var endpoint = "/api/role/toAssign";
                var result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();
                var contentRoles = await result.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<AgroControlRoleDto>>(contentRoles)
                .Select(r => new SelectListItem
                {
                    Text = r.Name, 
                    Value = r.Id.ToString()
                }).ToList();

                ViewBag.Roles = roles;
                return View(model);

            }
            var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/invitations/send", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Zaproszenie wysłane pomyślnie!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Błąd wysyłania zaproszenia. Konto o podanym adresie email może nie istnieć lub należy już do tego gospodarstwa.";
                var apitoken = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apitoken);

                var endpoint = "/api/role/toAssign";
                var result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();
                var contentRoles = await result.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<AgroControlRoleDto>>(contentRoles)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();

                ViewBag.Roles = roles;
            }

            return View();
        }
        //[Authorize(Policy = "OwnerOrCoOwner")]
        //[HttpPost]
        //public async Task<IActionResult> Add(string email)
        //{

        //}

        // Delete
        //[Authorize(Policy = "OwnerOrCoOwner")]
        //[HttpGet]
        //public async Task<IActionResult> Delete(int id)
        //{

        //}

        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = HttpContext.Request.Cookies["token"];if(token==null){token = Request.Headers["Authorization"];}
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var endpoint = $"/api/AgroControlUserRole/{id}";
                var response = await _client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
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
        //Edit
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = "/api/role/toAssign";
            var result = await _client.GetAsync(endpoint);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<AgroControlRoleDto>>(content)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();

            ViewBag.Roles = roles;

            AgroControlUserRoleDto agroControlUserRoleDto = new AgroControlUserRoleDto();
            agroControlUserRoleDto.UserId = id;
            return View(agroControlUserRoleDto);
        }
        [Authorize(Policy = "OwnerOrCoOwner")]
        [HttpPost]
        public async Task<IActionResult> Edit(AgroControlUserRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                var apitoken = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apitoken);

                var endpoint = "/api/role/toAssign";
                var result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();
                var contentRoles = await result.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<AgroControlRoleDto>>(contentRoles)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();

                ViewBag.Roles = roles;
                return View(model);

            }
            var token = HttpContext.Request.Cookies["token"]; if (token == null) { token = Request.Headers["Authorization"]; }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/AgroControlUserRole/change", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Rola użytkownika została pomyślnie zaktualizowana!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Wystąpił problem przy zmianie roli. Spróbój ponownie później.";
                var apitoken = HttpContext.Request.Cookies["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apitoken);

                var endpoint = "/api/role/toAssign";
                var result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();
                var contentRoles = await result.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<AgroControlRoleDto>>(contentRoles)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();

                ViewBag.Roles = roles;
            }

            return View();
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
