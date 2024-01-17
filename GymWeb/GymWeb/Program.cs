using GymWeb.Config;
using GymWeb.Models;
using Stripe;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IUsuarioModel, UsuarioModel>();
builder.Services.AddSingleton<ISubscripcionModel, SubscripcionModel>();
builder.Services.AddSingleton<IPagosModel, PagosModel>();
builder.Services.AddSingleton<IEntrenadorModel, EntrenadorModel>();
builder.Services.AddSingleton<IEjercicioModel, EjercicioModel>();
builder.Services.AddSingleton<IRutinasModel, RutinasModel>();
builder.Services.AddSingleton<IRutinasDetalleModel, RutinasDetalleModel>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("GYMPRO_Client").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

StripeConfiguration.ApiKey = "sk_test_51O6kaBERe0Ut4IOaXPcHe5yh9GNG6uebaOJEYLTQbpJaoDyU1bm4bPTgYZS6Gc1nCzJgGVZdrg69r7Uz7QBN2zbW00kjj0oPhz";

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var supportedCultures = new[]
{
    new CultureInfo("es-CR")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es-CR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
