using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Services.DataProvider.helper
{
	public interface IWeatherHttpProxy
	{
		Task<string> GetFromUrl(string url);
	}
}
