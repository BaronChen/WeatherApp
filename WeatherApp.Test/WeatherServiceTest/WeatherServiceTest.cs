using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using WeatherApp.Services.DataProvider;
using WeatherApp.Services.DataProvider.helper;
using WeatherApp.Services.WeatherService.impl;
using WeatherApp.Services.WeatherService.interfaces;
using WeatherApp.Services.WeatherService.model;

namespace WeatherApp.Test.WeatherServiceTest
{
	[TestFixture]
	public class WeatherServiceTest
	{
		const string TEST_CITY_ID = "2147714";
		const string TEST_CITY_NAME = "Sydney";
		const string TEST_CITY_COUNTRY = "AU";
		const string TEST_CITY_NULL_ID = "56789";

		private IWeatherService WeatherService { get; set; }
		private CityDataProvider CityDataProvider { get; set; }
		private WeatherDataProvider WeatherDataProvider { get; set; }

		[SetUp]
		public void SetUp()
		{
			if (CityDataProvider == null)
			{
				CityDataProvider = Substitute.For<CityDataProvider>();
				CityDataProvider.GetCitiesByName(TEST_CITY_NAME).Returns(new List<CityResource>()
				{
					new CityResource()
					{
						Id = TEST_CITY_ID,
						Name = TEST_CITY_NAME,
						Country = TEST_CITY_COUNTRY
					},
					new CityResource()
					{
						Id = "12345",
						Name = "North Sydney",
						Country = "AU"
					}
				});
			}

			if (WeatherDataProvider == null)
			{
				WeatherDataProvider = Substitute.For<WeatherDataProvider>(new WeatherHttpProxy());
				WeatherDataProvider.GetCurrentWeatherByCityId(TEST_CITY_ID).Returns(new WeatherReponse()
				{
					CityId = TEST_CITY_ID,
					CityName = TEST_CITY_NAME,
					SysInfo = new SystemInfo()
					{
						Country = TEST_CITY_COUNTRY
					},
					Wind = new Wind()
					{
						Speed = 21,
						Deg = 12
					},
					Visibility = 83,
					MainWeather = new MainWeather()
					{
						Temp = 32,
						TempMin = 21,
						TempMax = 36,
						Humidity = 76,
						Pressure = 451

					},
					Weathers = new List<Weather>()
					{
						new Weather()
						{
							Main = "Sunny"
						}
					}
				});

				WeatherDataProvider.GetCurrentWeatherByCityId(TEST_CITY_NULL_ID).Returns<WeatherReponse>(x => null);

			}

			if (WeatherService == null)
			{
				this.WeatherService = new WeatherService(CityDataProvider, WeatherDataProvider);

			}
		}

		[TearDown]
		public void TearDown()
		{
			CityDataProvider.ClearReceivedCalls();
			WeatherDataProvider.ClearReceivedCalls();
		}

		[Test]
		public void GetCitiesByQueryTest_ShouldReturnCitiesData()
		{
			var result = WeatherService.GetCitiesByQuery(TEST_CITY_NAME);

			CityDataProvider.Received(1).GetCitiesByName(TEST_CITY_NAME);

			Assert.AreEqual(result.Cities.Count, 2);
			Assert.AreEqual(result.Cities.Any(x => x.Id == TEST_CITY_ID), true);
		}

		[Test]
		public async Task GetWeatherByCityIdTest_ShouldReturnCurrentWeatherForCity()
		{
			var result = await WeatherService.GetWeatherByCityAsync(TEST_CITY_ID);

			await WeatherDataProvider.Received(1).GetCurrentWeatherByCityId(TEST_CITY_ID);

			Assert.AreEqual(TEST_CITY_NAME + ", " + TEST_CITY_COUNTRY, result.Location);
			Assert.AreEqual(true, result.Temperature.Contains("32"));
			Assert.AreEqual("Sunny", result.Main);
			//need more
		}

		[Test]
		public async Task GetWeatherByCityIdTest_ShouldReturnNull()
		{
			var exception = Assert.Throws<AggregateException>(() => WeatherService.GetWeatherByCityAsync(TEST_CITY_NULL_ID).Wait());

			await WeatherDataProvider.Received(1).GetCurrentWeatherByCityId(TEST_CITY_NULL_ID);

			StringAssert.Contains("Cannot find weather for the given city id.", exception.InnerExceptions[0].Message);
		}

	}
}
