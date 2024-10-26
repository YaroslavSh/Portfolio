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

// ��������� ������ ��� �������� ������ �� JSON
builder.Services.AddSingleton<CityDataService>();
// ����������� ������� SubdomainService
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<SubdomainService>();

var app = builder.Build();

// ��������� middleware ��� ������������� ���������
app.UseMiddleware<SubdomainMiddleware>();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// ��������� ������ �� JSON ��� ������ ����������
var cityDataService = app.Services.GetRequiredService<CityDataService>();
await cityDataService.LoadCityDataAsync("Data/citydata.json"); // ������� ���� � ������ JSON-�����

// ������������� CitiesService � �������
CitiesService.Configure(app.Services);

// Middleware ��� ��������� �������� � robots.txt � sitemap.xml
app.UseMiddleware<Ecoplaza.Middleware.CustomRobotsAndSitemaps>();

// ��������� �������������� ����� ��� ����������� ������
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot/verification-site-files")),
    RequestPath = "" // ���� ������� ����� ���� � ����� �������� /verification-site-files ��� ����� ����� ��������� ��� �������
});



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

