using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeatherApp.Services.WeatherService.model
{
	public class CitiesData
	{
		public List<CityData> Cities { get; set; } 
	}

	public class CityData
	{
		public string Country { get; set; }
		public string City { get; set; }	
		public string Id { get; set; }	
	}

	public enum RequestStatus
	{
		Fail = 0,
		Success = 1	
	}

	public class CurrentWeather
	{
		public string Location { get; set; }
		public string Time { get; set; }
		public string Wind { get; set; }
		public string Visibility { get; set; }
		public string Main { get; set; }
		public string Temperature { get; set; }
		public string RelativeHumidity { get; set; }
		public string Pressure { get; set; }
	}
}
