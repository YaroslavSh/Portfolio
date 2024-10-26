using Ecoplaza;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

// Использование метода
// 1) Инжектировать CounterManager на страницу где предполагается использование сервиса: @inject Ecoplaza.CounterManager CounterManager
// 2) Разместить на странице строку: @await CounterManager.GetCounterScriptAsync()
// 3) Зарегистрировать сервис в файле Program.cs
// Регистрация CounterManager
// builder.Services.AddSingleton<CounterManager>();

namespace Ecoplaza
{
    public class CounterManager
    {
        private readonly IWebHostEnvironment _env;
        private readonly SubdomainService _subdomainService;

        public CounterManager(IWebHostEnvironment env, SubdomainService subdomainService)
        {
            _env = env;
            _subdomainService = subdomainService;
        }

        // Метод для получения пути к файлу с кодом счетчика
        public string GetCounterFilePath()
        {
            string subdomain = _subdomainService.Subdomain;

            // Определяем путь к файлу на основе поддомена
            string fileName = string.IsNullOrEmpty(subdomain) ? "default.js" : $"{subdomain}.js";
            string filePath = Path.Combine(_env.WebRootPath, "counters", fileName);

            // Возвращаем путь к файлу, если он существует
            // return File.Exists(filePath) ? filePath : null; // Если файла не существует но сайт открыт на поддомене - то счетчика не будет
            return File.Exists(filePath) ? filePath : Path.Combine(_env.WebRootPath, "counters", "default.js"); // Если файла не существует но сайт открыт на поддомене - будет установлен счетчик по умолчанию
        }

        public async Task<HtmlString> GetCounterScriptAsync()
        {
            string filePath = GetCounterFilePath();

            if (filePath != null)
            {
                var content = await File.ReadAllTextAsync(filePath);
                return new HtmlString(content);
            }

            return HtmlString.Empty;
        }

    }
}

