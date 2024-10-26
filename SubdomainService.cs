using Ecoplaza.Data;

// Алгоритм с кешированием поддомена, обновление кеша обеспечивает SubdomainMiddleware при каждом запросе http
// Вариант с сопостовлением поддомена с данными в JSON что гарантирует работу исключительно с существующими поддоменами
// Доюавлено новое логическое свойство IsSubdomain

namespace Ecoplaza
{
    public class SubdomainService
    {
        private readonly CityDataService _cityDataService;

        // Свойство для хранения и доступа к поддомену
        public string? Subdomain { get; private set; }
        public bool IsSubdomain { get; private set; }

        // Конструктор для внедрения зависимости CityDataService
        public SubdomainService(CityDataService cityDataService)
        {
            _cityDataService = cityDataService;
        }

        // Метод инициализации, вызываемый Middleware
        public void Initialize(HttpContext context)
        {
            var host = context.Request.Host.Host;
            string baseDomain = GetBaseDomain(host);

            // Проверяем, является ли хост поддоменом
            if (!host.Equals(baseDomain, StringComparison.OrdinalIgnoreCase))
            {
                var potentialSubdomain = host.Replace($".{baseDomain}", "");

                // Проверяем, существует ли поддомен в CityDataService
                if (_cityDataService.ContainsKey(potentialSubdomain))
                {
                    Subdomain = potentialSubdomain; // Поддомен существует, сохраняем его
                    IsSubdomain = true;
                }
                else
                {
                    Subdomain = null; // Поддомен не существует, обнуляем его
                    IsSubdomain = false;
                }
            }
            else
            {
                Subdomain = null; // Основной домен, поддомена нет
                IsSubdomain = false;
            }
        }

        // Метод для определения базового домена
        private string GetBaseDomain(string host)
        {
            if (host.Contains("localhost"))
            {
                return "localhost";
            }
            var parts = host.Split('.');
            return parts.Length > 2 ? string.Join(".", parts.Skip(1)) : host;
        }
    }
}



// Вариант без сопостовления поддомена с данными в JSON - рабочий в реальных условиях, по при тестировании возвращает поддомен которого нет в списке
// Алгоритм с кешированием поддомена, обновление кеша обеспечивает SubdomainMiddleware при каждом запросе http
//using System;
//using System.Text.Json;
//using Ecoplaza.Data;
//using Microsoft.AspNetCore.Http;

//namespace Ecoplaza
//{
//    public class SubdomainService
//    {
//        // Свойство для хранения и доступа к поддомену
//        public string Subdomain { get; private set; }

//        // Метод инициализации, вызываемый Middleware
//        public void Initialize(HttpContext context)
//        {
//            var host = context.Request.Host.Host;
//            string baseDomain = GetBaseDomain(host);

//            // Определяем и сохраняем поддомен
//            if (!host.Equals(baseDomain, StringComparison.OrdinalIgnoreCase))
//            {
//                Subdomain = host.Replace($".{baseDomain}", "");
//            }
//            else
//            {
//                Subdomain = null; // Если открыт основной домен
//            }
//        }

//        // Метод для определения базового домена
//        private string GetBaseDomain(string host)
//        {
//            if (host.Contains("localhost"))
//            {
//                return "localhost";
//            }
//            var parts = host.Split('.');
//            return parts.Length > 2 ? string.Join(".", parts.Skip(1)) : host;
//        }
//    }
//}



// Рабочий вариант без кеширования поддомена
//namespace Ecoplaza
//{
//    public class SubdomainService
//    {
//        public string Subdomain { get; private set; }

//        public void Initialize(HttpContext context)
//        {
//            var host = context.Request.Host.Host;
//            string baseDomain = GetBaseDomain(host);

//            if (!host.Equals(baseDomain, StringComparison.OrdinalIgnoreCase))
//            {
//                Subdomain = host.Replace($".{baseDomain}", "");
//            }
//            else
//            {
//                Subdomain = null; // Открыт основной домен
//            }
//        }

//        private string GetBaseDomain(string host)
//        {
//            if (host.Contains("localhost"))
//            {
//                return "localhost";
//            }

//            var parts = host.Split('.');
//            return parts.Length > 2 ? string.Join(".", parts.Skip(1)) : host;
//        }
//    }
//}


// Способ использования сервиса
//@inject SubdomainService SubdomainService

//@if (!string.IsNullOrEmpty(SubdomainService.Subdomain))
//{
//    < p > Сайт открыт на поддомене: @SubdomainService.Subdomain </ p >
//}
//else
//{
//    < p > Сайт открыт на основном домене</ p >
//}