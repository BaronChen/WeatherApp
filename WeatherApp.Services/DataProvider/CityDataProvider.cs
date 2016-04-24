using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp.Services.DataProvider
{
	public class CityResource
	{
		[JsonProperty("_id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("coord")]
		public Coordinate Coordinate { get; set; }

	}

	public class Coordinate
	{
		[JsonProperty("lon")]
		public decimal Lon { get; set; }

		[JsonProperty("lat")]
		public decimal Lat { get; set; }
	}

	public class CityDataProvider
	{

		public List<CityResource> Resources { get; set; } 
		public CityDataProvider()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "WeatherApp.Services.DataProvider.city.list.json";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				string result = reader.ReadToEnd();
				Resources = JsonConvert.DeserializeObject<List<CityResource>>(result);
			}
		}


		public virtual CityResource GetCityById(string cityId)
		{
			return Resources.Find(x => x.Id == cityId);
		}


		public virtual List<CityResource> GetCitiesByName(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
				return new List<CityResource>();

			return Resources.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();
		} 
	}
}
