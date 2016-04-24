using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeatherApp.Services.DataProvider;
using WeatherApp.Services.WeatherService.interfaces;
using WeatherApp.Services.WeatherService.model;

namespace WeatherApp.Services.WeatherService.impl
{

	/* Since the web service is not reliable, I will return an empty result other than throw an error whenever an deserialization exception happen.  
	* Therefore, even though the web service fail, we can still walk through the whole application
	*/
	public class WeatherService : IWeatherService
	{

		private readonly CityDataProvider CityDataProvider;

		private readonly WeatherDataProvider WeatherDataProvider;

		public WeatherService(CityDataProvider cityDataProvider, WeatherDataProvider weatherDataProvider)
		{
			CityDataProvider = cityDataProvider;
			WeatherDataProvider = weatherDataProvider;
		}


		public CitiesData GetCitiesByQuery(string query)
		{
			var cityResources = CityDataProvider.GetCitiesByName(query);

			var citiesData = new CitiesData();

			citiesData.Cities =
				cityResources.Select(x => new CityData() {City = x.Name, Country = x.Country, Id = x.Id}).ToList();

			return citiesData;

		}


		public async Task<CurrentWeather> GetWeatherByCityAsync(string cityId)
		{
			var weatherRsource = await WeatherDataProvider.GetCurrentWeatherByCityId(cityId);

			if (weatherRsource == null)
			{
				throw new Exception("Cannot find weather for the given city id.");
			}

			var currrentWeather= new CurrentWeather()
			{
				Location = weatherRsource.CityName + ", " + weatherRsource.SysInfo.Country,
				Time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
				Wind = "Speed: " + weatherRsource.Wind.Speed +" m/s, " + "Deg: " + weatherRsource.Wind.Deg,
				Visibility = weatherRsource.Visibility.ToString(CultureInfo.InvariantCulture),
				Main = weatherRsource.Weathers.First().Main,
				Temperature = weatherRsource.MainWeather.Temp + " Celsius, Range:" + weatherRsource.MainWeather.TempMin + "~" +weatherRsource.MainWeather.TempMax + "Celsius",
				RelativeHumidity = weatherRsource.MainWeather.Humidity + "%",
				Pressure = weatherRsource.MainWeather.Pressure + " hPa"
			};

			return currrentWeather;
		}


	}
}
