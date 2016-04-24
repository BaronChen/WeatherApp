using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Services.WeatherService.model;

namespace WeatherApp.Services.WeatherService.interfaces
{
    public interface IWeatherService
    {
		CitiesData GetCitiesByQuery(string query);
		Task<CurrentWeather> GetWeatherByCityAsync(string cityId);
    }
}
