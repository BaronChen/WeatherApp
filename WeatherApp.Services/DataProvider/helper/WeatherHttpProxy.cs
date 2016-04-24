using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp.Services.DataProvider.helper
{
	public class WeatherHttpProxy : IWeatherHttpProxy
	{

		public WeatherHttpProxy()
		{

		}

		public async Task<string> GetFromUrl(string url)
		{
			using (WebClient client = new WebClient())
			{
				client.Headers["Accept"] = "application/json";

				var result = await client.DownloadStringTaskAsync(new Uri(url));

				return result;
			}
		}

	}
}
	;