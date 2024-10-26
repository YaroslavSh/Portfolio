using System;
using System.Text.Json;
using Ecoplaza.Data;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

//Пример доступа к компоненту
//@CitiesService.Get("toponyms.nom", "Значение по умолчанию")

namespace Ecoplaza.Data
{
    public static class CitiesService
    {
        private static CityDataService _cityDataService;
        private static SubdomainService _subdomainService;

        public static void Configure(IServiceProvider serviceProvider)
        {
            _cityDataService = serviceProvider.GetRequiredService<CityDataService>();
            _subdomainService = serviceProvider.GetRequiredService<SubdomainService>();
        }

        private static JsonElement? GetSelectedData()
        {
            var subdomain = _subdomainService.Subdomain;

            // Проверяем наличие поддомена перед попыткой получить данные
            if (!string.IsNullOrEmpty(subdomain))
            {
                return _cityDataService.GetCityData(subdomain);
            }

            // Если поддомена нет, возвращаем null, так как данных искать не нужно
            return null;
        }

        public static string Get(string key, string defaultValue = "")
        {
            var selectedData = GetSelectedData();

            if (selectedData == null)
            {
                return defaultValue;
            }

            JsonElement currentData = selectedData.Value;

            if (currentData.ValueKind == JsonValueKind.String)
            {
                return currentData.GetString();
            }

            var keys = key.Split('.');
            foreach (var k in keys)
            {
                if (currentData.ValueKind == JsonValueKind.Object && currentData.TryGetProperty(k, out JsonElement nextElement))
                {
                    currentData = nextElement;
                }
                else
                {
                    return defaultValue;
                }
            }

            return currentData.ValueKind == JsonValueKind.String ? currentData.GetString() : currentData.ToString();
        }


        // Рабочий вариант с повторным кешированием данных по конкретному ключу - нет смысла потому что работа со списком и так молниеносна
        //private static CityDataService _cityDataService;
        //private static SubdomainService _subdomainService;
        //private static JsonElement? _selectedData; // Кеширование данных
        //private static string _currentSubdomain; // Текущий поддомен

        //public static void Configure(IServiceProvider serviceProvider)
        //{
        //    _cityDataService = serviceProvider.GetRequiredService<CityDataService>();
        //    _subdomainService = serviceProvider.GetRequiredService<SubdomainService>();
        //}

        //private static void UpdateSelectedDataIfNeeded()
        //{
        //    var subdomain = _subdomainService.Subdomain;

        //    // Проверка на изменение поддомена
        //    if (_currentSubdomain != subdomain)
        //    {
        //        _currentSubdomain = subdomain;
        //        _selectedData = !string.IsNullOrEmpty(subdomain) ? _cityDataService.GetCityData(subdomain) : null;
        //    }
        //}

        //public static string Get(string key, string defaultValue = "")
        //{
        //    // Обновляем данные только если изменился поддомен
        //    UpdateSelectedDataIfNeeded();

        //    if (_selectedData == null)
        //    {
        //        return defaultValue;
        //    }

        //    JsonElement currentData = _selectedData.Value;

        //    if (currentData.ValueKind == JsonValueKind.String)
        //    {
        //        return currentData.GetString();
        //    }

        //    var keys = key.Split('.');
        //    foreach (var k in keys)
        //    {
        //        if (currentData.ValueKind == JsonValueKind.Object && currentData.TryGetProperty(k, out JsonElement nextElement))
        //        {
        //            currentData = nextElement;
        //        }
        //        else
        //        {
        //            return defaultValue;
        //        }
        //    }

        //    return currentData.ValueKind == JsonValueKind.String ? currentData.GetString() : currentData.ToString();
        //}

    }
}
