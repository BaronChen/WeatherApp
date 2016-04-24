using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using NSubstitute;
using NUnit.Framework;
using WeatherApp.Services.WeatherService.interfaces;
using WeatherApp.Services.WeatherService.model;
using WeatherApp.Web.Controllers;
using WeatherApp.Web.Models;

namespace WeatherApp.Test.WeatherControllerTest
{
	[TestFixture]
	public class WeatherControllerTest
	{

		const string CITY_TEST_INPUT = "Sydney";
		const string CITY_TEST_EMPTY_INPUT = "";

		const string CITY_TEST_EXCEPTION_INPUT = "CityException";

		const string TEXT_EXCEPTION_MSG = "Test Exception";
		const string COUNTRY_TEST_CODE = "AU";

		const string CITY_ID_TEST_INPUT = "654321";
		const string CITY_ID_EXCEPTION_INPUT = "9876";

		private WeatherController WeatherController;
		private IWeatherService WeatherService;


		[SetUp]
		public void Init()
		{
			if (WeatherService == null)
			{
				WeatherService = Substitute.For<IWeatherService>();

				WeatherService.GetCitiesByQuery(CITY_TEST_INPUT).Returns<CitiesData>(new CitiesData()
				{
					Cities = new List<CityData>()
				{
					new CityData()
					{
						City = CITY_TEST_INPUT,
						Country = COUNTRY_TEST_CODE
					}
				}
				});

				WeatherService.GetCitiesByQuery(CITY_TEST_EXCEPTION_INPUT).Returns<CitiesData>(x =>
				{
					throw new Exception(TEXT_EXCEPTION_MSG);
				});

				WeatherService.GetWeatherByCityAsync(CITY_ID_TEST_INPUT).Returns<CurrentWeather>(new CurrentWeather()
				{
					Location = CITY_TEST_INPUT +", " + COUNTRY_TEST_CODE,
					Status = RequestStatus.Success
				});

				WeatherService.GetWeatherByCityAsync(CITY_ID_EXCEPTION_INPUT).Returns<CurrentWeather>(x =>
				{
					throw new Exception(TEXT_EXCEPTION_MSG);
				});

			}

			if (WeatherController == null)
			{
				WeatherController = new WeatherController(WeatherService);
			}
		}

		[TearDown]
		public void TearDowan()
		{
			WeatherService.ClearReceivedCalls();
		}

		[Test]
		public void TestGetCitiesByQuery_ShouldReturnOkResult()
		{
			var response = WeatherController.GetCities(CITY_TEST_INPUT) as OkNegotiatedContentResult<CitiesData>;

			WeatherService.Received(1).GetCitiesByQuery(CITY_TEST_INPUT);

			Assert.NotNull(response);
			Assert.AreEqual(1, response.Content.Cities.Count);
			Assert.AreEqual(CITY_TEST_INPUT, response.Content.Cities[0].City);

		}

		[Test]
		public void TestGetCities_ShouldReturnInternalErrorResult()
		{
			var response = WeatherController.GetCities(CITY_TEST_EXCEPTION_INPUT) as ErrorResult;

			WeatherService.Received(1).GetCitiesByQuery(CITY_TEST_EXCEPTION_INPUT);

			Assert.NotNull(response);
			Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
			Assert.AreEqual(TEXT_EXCEPTION_MSG, response.Message);

		}

		[Test]
		public async Task TestGetWeather_ShouldReturnOkResult()
		{
			var response = await WeatherController.GetWeather(CITY_ID_TEST_INPUT) as OkNegotiatedContentResult<CurrentWeather>;

			await WeatherService.Received(1).GetWeatherByCityAsync(CITY_ID_TEST_INPUT);

			Assert.NotNull(response);
			Assert.AreEqual(CITY_TEST_INPUT + ", " + COUNTRY_TEST_CODE, response.Content.Location);
			Assert.AreEqual(RequestStatus.Success, response.Content.Status);
		}

		[Test]
		public async Task TestGetWeather_ShouldReturnBadResult()
		{
			var response = await WeatherController.GetWeather(CITY_TEST_EMPTY_INPUT) as ErrorResult;

			Assert.NotNull(response);
			Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
			Assert.AreEqual("City should not be empty", response.Message);
		}

		[Test]
		public async Task TestGetWeather_ShouldReturnInternalErrorResult()
		{
			var response = await WeatherController.GetWeather(CITY_ID_EXCEPTION_INPUT) as ErrorResult;

			await WeatherService.Received(1).GetWeatherByCityAsync(CITY_ID_EXCEPTION_INPUT);

			Assert.NotNull(response);
			Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
			Assert.AreEqual(TEXT_EXCEPTION_MSG, response.Message);

		}

	}
}
