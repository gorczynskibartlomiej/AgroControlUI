using Microsoft.AspNetCore.Authentication.Cookies;
using AgroControlUI.Middleware;
using AgroControlUI.Services.Implementations;
using AgroControlUI.Services.Interfaces;
using Serilog;
using AgroControlUI.Validators.Account;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("Owner", policy =>
        policy.RequireRole("Owner"));

    options.AddPolicy("OwnerOrAccountant", policy =>
    {
        policy.RequireRole("W³aœciciel", "Wspó³w³aœciciel", "Ksiêgowy");
    });
    options.AddPolicy("OwnerOrWorker", policy =>
    {
        policy.RequireRole("W³aœciciel");
        policy.RequireRole("Wspó³w³aœciciel");
        policy.RequireRole("Pracownik");
    });
    options.AddPolicy("OwnerOrCoOwner", policy =>
    {
        policy.RequireRole("W³aœciciel", "Wspó³w³aœciciel");
    });
    options.AddPolicy("Owner", policy =>
    {
        policy.RequireRole("W³aœciciel");
    });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//FluentValidation
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true; 
}).AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterModelDtoValidator>();


builder.Services.AddSingleton<ILoggerService, LoggerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
