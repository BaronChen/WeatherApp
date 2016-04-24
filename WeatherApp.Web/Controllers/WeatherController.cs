using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WeatherApp.Services.WeatherService.interfaces;
using WeatherApp.Services.WeatherService.model;
using WeatherApp.Web.Models;

namespace WeatherApp.Web.Controllers
{

	/* All the exception message will display to the user per requirement
	 */
	[RoutePrefix("api/weather")]
    public class WeatherController : ApiController
	{
		private readonly IWeatherService weatherService;

		public WeatherController(IWeatherService weatherService)
		{
			this.weatherService = weatherService;
		}

		[Route("cities")]
		[HttpGet]
	    public IHttpActionResult GetCities(string query)
		{
			try
			{
				var cities = weatherService.GetCitiesByQuery(query);
				return Ok(cities);
			}
			catch (Exception e)
			{
				return new ErrorResult(e.Message, Request, HttpStatusCode.InternalServerError);
			}
			
		}


		[Route("current")]
		[HttpGet]
		public async Task<IHttpActionResult> GetWeather(string cityId)
		{
			if (string.IsNullOrWhiteSpace(cityId))
			{
				return new ErrorResult("City should not be empty", Request, HttpStatusCode.BadRequest);
			}

			try
			{
				var weather = await weatherService.GetWeatherByCityAsync(cityId);
				return Ok(weather);
			}
			catch (Exception e)
			{
				return new ErrorResult(e.Message, Request, HttpStatusCode.InternalServerError);
			}

		}

	}
}
