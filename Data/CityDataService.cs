//namespace Ecoplaza.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecoplaza.Data
{
    public class CityDataService
    {
        private Dictionary<string, dynamic> _cityData;

        public async Task LoadCityDataAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                _cityData = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(json);
            }
            else
            {
                _cityData = new Dictionary<string, dynamic>();
            }
        }

        public dynamic GetCityData(string subdomain)
        {
            if (_cityData.ContainsKey(subdomain))
            {
                return _cityData[subdomain];
            }
            return null;
        }

        // Метод проверки наличия ключа (поддомена) в словаре
        public bool ContainsKey(string subdomain)
        {
            return _cityData.ContainsKey(subdomain);
        }
    }

}