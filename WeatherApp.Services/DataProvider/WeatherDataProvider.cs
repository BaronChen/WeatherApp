using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Services.DataProvider.helper;

namespace WeatherApp.Services.DataProvider
{
	public class WeatherReponse
	{
		[JsonProperty("coord")]
		public Coordinate Coordinate { get; set; }
		[JsonProperty("weather")]
		public List<Weather> Weathers { get; set; }
		[JsonProperty("base")]
		public string Base { get; set; }
		[JsonProperty("main")]
		public MainWeather MainWeather { get; set; }
		[JsonProperty("visibility")]
		public decimal Visibility { get; set; }
		[JsonProperty("wind")]
		public Wind Wind{ get; set; }
		[JsonProperty("sys")]
		public SystemInfo SysInfo { get; set; }
		[JsonProperty("name")]
		public string CityName { get; set; }
		[JsonProperty("id")]
		public string CityId { get; set; }
	}

	public class Weather
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("main")]
		public string Main { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("icon")]
		public string Icon { get; set; }
	}

	public class MainWeather
	{
		[JsonProperty("temp")]
		public decimal Temp { get; set; }
		[JsonProperty("pressure")]
		public decimal Pressure { get; set; }
		[JsonProperty("humidity")]
		public decimal Humidity { get; set; }
		[JsonProperty("temp_min")]
		public decimal TempMin { get; set; }
		[JsonProperty("temp_max")]
		public decimal TempMax { get; set; }

	}

	public class SystemInfo
	{
		[JsonProperty("country")]
		public string Country { get; set; }
	}

	public class Wind
	{
		[JsonProperty("speed")]
		public decimal Speed { get; set; }
		[JsonProperty("deg")]
		public decimal Deg { get; set; }
	}

	public class WeatherDataProvider
	{
		private string ApiKey { get; set; }
		const string CurrentWeatherEndPoint = "http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}&units=metric";

		private IWeatherHttpProxy WeatherHttpProxy;

		public WeatherDataProvider(IWeatherHttpProxy weatherHttpProxy)
		{
			WeatherHttpProxy = weatherHttpProxy;
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "WeatherApp.Services.DataProvider.APIKey.txt";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				ApiKey = reader.ReadToEnd();
			}
		}

		public virtual async Task<WeatherReponse> GetCurrentWeatherByCityId(string cityId)
		{
			var result = await WeatherHttpProxy.GetFromUrl(string.Format(CurrentWeatherEndPoint, cityId, ApiKey));

			try
			{
				var weatherResponse = JsonConvert.DeserializeObject<WeatherReponse>(result);
				if (string.IsNullOrEmpty(weatherResponse.CityId))
					return null;
				return weatherResponse;
			}
			catch (Exception e)
			{
				return null;
			}
		}




	}
}
