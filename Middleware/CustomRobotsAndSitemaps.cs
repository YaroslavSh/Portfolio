using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Ecoplaza.Middleware
{
    public class CustomRobotsAndSitemaps
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CustomRobotsAndSitemaps> _logger;
        private readonly SubdomainService _subdomainService;

        public CustomRobotsAndSitemaps(RequestDelegate next, IWebHostEnvironment env, ILogger<CustomRobotsAndSitemaps> logger, SubdomainService subdomainService)
        {
            _next = next;
            _env = env;
            _logger = logger;
            _subdomainService = subdomainService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            // Проверяем запросы к robots.txt и sitemap.xml
            if (path.Equals("/robots.txt") || path.Equals("/sitemap.xml"))
            {
                // Инициализация поддомена через SubdomainService
                _subdomainService.Initialize(context);
                var subdomain = _subdomainService.Subdomain;

                string folder = path.Equals("/robots.txt") ? "robots-txt" : "sitemap-xml";
                string fileToServe;

                if (string.IsNullOrEmpty(subdomain))
                {
                    // Основной домен - используем основной файл robots.txt или sitemap.xml
                    fileToServe = Path.Combine(_env.WebRootPath, folder, path.TrimStart('/'));
                }
                else
                {
                    // Обрабатываем запрос для поддомена
                    string subdomainFilePath = Path.Combine(_env.WebRootPath, folder, path.Equals("/robots.txt") ? $"{subdomain}.txt" : $"{subdomain}.xml");

                    if (path.Equals("/robots.txt") && File.Exists(subdomainFilePath))
                    {
                        // Обрабатываем robots.txt для поддомена: объединяем шаблон и поддоменный файл
                        string templateFilePath = Path.Combine(_env.WebRootPath, folder, "robots-template-sub.txt");
                        string subdomainContent = await File.ReadAllTextAsync(subdomainFilePath);
                        string templateContent = await File.ReadAllTextAsync(templateFilePath);
                        string combinedContent = templateContent + System.Environment.NewLine + subdomainContent;

                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync(combinedContent);
                        return;
                    }
                    else if (path.Equals("/sitemap.xml"))
                    {
                        // Для sitemap.xml используем поддоменный файл напрямую
                        fileToServe = subdomainFilePath;
                    }
                    else
                    {
                        // Если файл для поддомена не найден, передаем управление следующему middleware
                        await _next(context);
                        return;
                    }
                }

                // Отправляем файл, если он существует
                if (File.Exists(fileToServe))
                {
                    context.Response.ContentType = path.Equals("/robots.txt") ? "text/plain" : "application/xml";
                    await context.Response.SendFileAsync(fileToServe);
                }
                else
                {
                    // Если файл не найден, передаем управление следующему middleware, которое вернет 404
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }


    // Альтернативный подход делает тоже самое но без использования SubdomainService, что приводит к повторным вычислениям

    //public class CustomRobotsAndSitemaps
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly IWebHostEnvironment _env;
    //    private readonly ILogger<CustomRobotsAndSitemaps> _logger;

    //    public CustomRobotsAndSitemaps(RequestDelegate next, IWebHostEnvironment env, ILogger<CustomRobotsAndSitemaps> logger)
    //    {
    //        _next = next;
    //        _env = env;
    //        _logger = logger;
    //    }

    //    public async Task InvokeAsync(HttpContext context)
    //    {
    //        var path = context.Request.Path.Value;

    //        // Проверяем запросы к robots.txt и sitemap.xml
    //        if (path.Equals("/robots.txt") || path.Equals("/sitemap.xml"))
    //        {
    //            var host = context.Request.Host.Host;

    //            // Определяем базовый домен
    //            string baseDomain;
    //            if (host.Contains("localhost"))
    //            {
    //                baseDomain = "localhost";
    //            }
    //            else
    //            {
    //                var parts = host.Split('.');
    //                baseDomain = parts.Length > 2
    //                    ? string.Join(".", parts.Skip(1))
    //                    : host;
    //            }

    //            string folder = path.Equals("/robots.txt") ? "robots-txt" : "sitemap-xml";
    //            string fileToServe;

    //            // Проверяем, является ли запрос с основного домена или поддомена
    //            if (host.Equals(baseDomain, StringComparison.OrdinalIgnoreCase))
    //            {
    //                // Основной домен - используем основной файл robots.txt или sitemap.xml
    //                fileToServe = Path.Combine(_env.WebRootPath, folder, path.TrimStart('/'));
    //            }
    //            else
    //            {
    //                // Извлекаем имя поддомена
    //                var subdomain = host.Replace($".{baseDomain}", "");
    //                string subdomainFilePath = Path.Combine(_env.WebRootPath, folder, path.Equals("/robots.txt") ? $"{subdomain}.txt" : $"{subdomain}.xml");

    //                if (path.Equals("/robots.txt") && File.Exists(subdomainFilePath))
    //                {
    //                    // Обрабатываем robots.txt для поддомена: объединяем шаблон и поддоменный файл
    //                    string templateFilePath = Path.Combine(_env.WebRootPath, folder, "robots-template-sub.txt");
    //                    string subdomainContent = await File.ReadAllTextAsync(subdomainFilePath);
    //                    string templateContent = await File.ReadAllTextAsync(templateFilePath);
    //                    string combinedContent = templateContent + System.Environment.NewLine + subdomainContent;

    //                    context.Response.ContentType = "text/plain";
    //                    await context.Response.WriteAsync(combinedContent);
    //                    return;
    //                }
    //                else if (path.Equals("/sitemap.xml"))
    //                {
    //                    // Для sitemap.xml используем поддоменный файл напрямую
    //                    fileToServe = subdomainFilePath;
    //                }
    //                else
    //                {
    //                    // Если файл для поддомена не найден, передаем управление следующему middleware
    //                    await _next(context);
    //                    return;
    //                }
    //            }

    //            // Отправляем файл, если он существует
    //            if (File.Exists(fileToServe))
    //            {
    //                context.Response.ContentType = path.Equals("/robots.txt") ? "text/plain" : "application/xml";
    //                await context.Response.SendFileAsync(fileToServe);
    //            }
    //            else
    //            {
    //                // Если файл не найден, передаем управление следующему middleware, которое вернет 404
    //                await _next(context);
    //            }
    //        }
    //        else
    //        {
    //            await _next(context);
    //        }
    //    }
    //}
}




// Либо этот код поместить непосредственно в Program.cs

//app.Use(async (context, next) =>
//{
//    var path = context.Request.Path.Value;

//    // Проверяем запросы к robots.txt и sitemap.xml
//    if (path.Equals("/robots.txt") || path.Equals("/sitemap.xml"))
//    {
//        var host = context.Request.Host.Host;

//        // Определяем базовый домен
//        string baseDomain;
//        if (host.Contains("localhost"))
//        {
//            baseDomain = "localhost";
//        }
//        else
//        {
//            var parts = host.Split('.');
//            baseDomain = parts.Length > 2
//                ? string.Join(".", parts.Skip(1))
//                : host;
//        }

//        string folder = path.Equals("/robots.txt") ? "robots-txt" : "sitemap-xml";
//        string fileToServe;

//        // Проверяем, является ли запрос с основного домена или поддомена
//        if (host.Equals(baseDomain, StringComparison.OrdinalIgnoreCase))
//        {
//            // Основной домен - используем основной файл robots.txt или sitemap.xml
//            fileToServe = Path.Combine(app.Environment.WebRootPath, folder, path.TrimStart('/'));
//        }
//        else
//        {
//            // Извлекаем имя поддомена
//            var subdomain = host.Replace($".{baseDomain}", "");
//            string subdomainFilePath = Path.Combine(app.Environment.WebRootPath, folder, path.Equals("/robots.txt") ? $"{subdomain}.txt" : $"{subdomain}.xml");

//            if (path.Equals("/robots.txt") && System.IO.File.Exists(subdomainFilePath))
//            {
//                // Обрабатываем robots.txt для поддомена: объединяем шаблон и поддоменный файл
//                string templateFilePath = Path.Combine(app.Environment.WebRootPath, folder, "robots-template-sub.txt");
//                string subdomainContent = await System.IO.File.ReadAllTextAsync(subdomainFilePath);
//                string templateContent = await System.IO.File.ReadAllTextAsync(templateFilePath);
//                string combinedContent = templateContent + Environment.NewLine + subdomainContent;

//                // Отправляем объединенное содержимое без сохранения на диск
//                context.Response.ContentType = "text/plain";
//                await context.Response.WriteAsync(combinedContent);
//                return;
//            }
//            else if (path.Equals("/sitemap.xml"))
//            {
//                // Для sitemap.xml используем поддоменный файл напрямую
//                fileToServe = subdomainFilePath;
//            }
//            else
//            {
//                // Если файл для поддомена не найден, передаем управление следующему middleware
//                await next();
//                return;
//            }
//        }

//        // Отправляем файл, если он существует
//        if (System.IO.File.Exists(fileToServe))
//        {
//            context.Response.ContentType = path.Equals("/robots.txt") ? "text/plain" : "application/xml";
//            await context.Response.SendFileAsync(fileToServe);
//        }
//        else
//        {
//            // Если файл не найден, передаем управление следующему middleware, которое вернет 404
//            await next();
//        }
//    }
//    else
//    {
//        await next();
//    }
//});


// Упрощённая версия без объединения файлов. Помещать в Program.cs

//app.Use(async (context, next) =>
//{
//    var path = context.Request.Path.Value;
//    if (path.Equals("/robots.txt") || path.Equals("/sitemap.xml"))
//    {
//        var host = context.Request.Host.Host;

//        // Определяем базовый домен
//        string baseDomain;
//        if (host.Contains("localhost"))
//        {
//            baseDomain = "localhost";
//        }
//        else
//        {
//            // Извлекаем основной домен из хоста
//            var parts = host.Split('.');
//            baseDomain = parts.Length > 2
//                ? string.Join(".", parts.Skip(1)) // Основной домен, если это поддомен
//                : host; // Сам хост, если это основной домен
//        }

//        string folder = path.Equals("/robots.txt") ? "robots-txt" : "sitemap-xml";
//        string fileToServe;

//        // Проверяем, является ли запрос с основного домена или поддомена
//        if (host.Equals(baseDomain, StringComparison.OrdinalIgnoreCase))
//        {
//            // Используем основной файл
//            fileToServe = Path.Combine(app.Environment.WebRootPath, folder, path.TrimStart('/'));
//        }
//        else
//        {
//            // Извлекаем имя поддомена
//            var subdomain = host.Replace($".{baseDomain}", "");

//            // Формируем имя файла для поддомена
//            var subdomainFileName = path.Equals("/robots.txt") ? $"{subdomain}.txt" : $"{subdomain}.xml";
//            fileToServe = Path.Combine(app.Environment.WebRootPath, folder, subdomainFileName);
//        }

//        // Отправляем файл, если он существует
//        if (System.IO.File.Exists(fileToServe))
//        {
//            context.Response.ContentType = path.Equals("/robots.txt") ? "text/plain" : "application/xml";
//            await context.Response.SendFileAsync(fileToServe);
//        }
//        else
//        {
//            // Если файл не найден, передаем управление следующему middleware, которое вернет 404
//            await next();
//        }
//    }
//    else
//    {
//        await next();
//    }
//});