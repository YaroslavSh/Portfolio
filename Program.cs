using Ecoplaza;
using Ecoplaza.Data;
using Ecoplaza.Middleware;
using Ecoplaza.Shared.ContactForms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Добавляем сервис для загрузки данных из JSON
builder.Services.AddSingleton<CityDataService>();
// Регистрация сервиса SubdomainService
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<SubdomainService>();

var app = builder.Build();

// Добавляем middleware для инициализации поддомена
app.UseMiddleware<SubdomainMiddleware>();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Загружаем данные из JSON при старте приложения
var cityDataService = app.Services.GetRequiredService<CityDataService>();
await cityDataService.LoadCityDataAsync("Data/citydata.json"); // Укажите путь к вашему JSON-файлу

// Конфигурируем CitiesService с данными
CitiesService.Configure(app.Services);

// Middleware для обработки запросов к robots.txt и sitemap.xml
app.UseMiddleware<Ecoplaza.Middleware.CustomRobotsAndSitemaps>();

// Настройка дополнительной папки для статических файлов
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot/verification-site-files")),
    RequestPath = "" // если указать любой путь к папке например /verification-site-files его нужно будет учитывать при запросе
});



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

