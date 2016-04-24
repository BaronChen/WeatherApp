using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using WeatherApp.Services.DataProvider;
using WeatherApp.Services.DataProvider.helper;

namespace WeatherApp.Test.WeatherServiceTest
{
	[TestFixture]
	public class WeatherDataProviderTest
	{
		const string TEST_CITY_ID = "2147714";
		const string TEST_CITY_ID_FAIL = "123232";
		const string TEST_CITY_NAME = "Sydney";
		const string TEST_CITY_COUNTRY = "AU";

		private WeatherDataProvider WeatherDataProvider;
		private IWeatherHttpProxy WeatherHttpProxy;

		private string TEST_URL = "http://api.openweathermap.org/data/2.5/weather?id=" + TEST_CITY_ID + "&APPID=b64cc985535e7cb6a791f93f5713d3dc&units=metric";
		private string TEST_URL_FAIL = "http://api.openweathermap.org/data/2.5/weather?id=" + TEST_CITY_ID_FAIL + "&APPID=b64cc985535e7cb6a791f93f5713d3dc&units=metric";
		[SetUp]
		public void SetUp()
		{
			if (WeatherHttpProxy == null)
			{
				WeatherHttpProxy = Substitute.For<IWeatherHttpProxy>();

				WeatherHttpProxy.GetFromUrl(TEST_URL).Returns(@"{""coord"" : {
																				""lon"" : 151.21,
																				""lat"" : -33.87
																			},
																			""weather"" : [{
																					""id"" : 521,
																					""main"" : ""Rain"",
																					""description"" : ""shower rain"",
																					""icon"" : ""09n""
																				}
																			],
																			""base"" : ""stations"",
																			""main"" : {
																				""temp"" : 18,
																				""pressure"" : 1020,
																				""humidity"" : 93,
																				""temp_min"" : 18,
																				""temp_max"" : 18
																			},
																			""visibility"" : 10000,
																			""wind"" : {
																				""speed"" : 3.1,
																				""deg"" : 220
																			},
																			""clouds"" : {
																				""all"" : 75
																			},
																			""dt"" : 1460977200,
																			""sys"" : {
																				""type"" : 1,
																				""id"" : 8233,
																				""message"" : 0.0137,
																				""country"" : ""AU"",
																				""sunrise"" : 1460924421,
																				""sunset"" : 1460964476
																			},
																			""id"" : 2147714,
																			""name"" : ""Sydney"",
																			""cod"" : 200
																		}");

				WeatherHttpProxy.GetFromUrl(TEST_URL_FAIL).Returns(@"{""message"":""something wrong!""}");

			}

			if (WeatherDataProvider == null)
			{
				WeatherDataProvider = new WeatherDataProvider(WeatherHttpProxy);
			}	
		}

		[TearDown]
		public void TearDown()
		{
			WeatherHttpProxy.ClearReceivedCalls();
		}

		[Test]
		public async Task GetWeatherByCityIdTest_ShouldReturnWeatherResponse()
		{
			var result = await WeatherDataProvider.GetCurrentWeatherByCityId(TEST_CITY_ID);

			await WeatherHttpProxy.Received(1).GetFromUrl(TEST_URL);

			Assert.NotNull(result);
			Assert.NotNull(result.MainWeather);
			Assert.NotNull(result.Weathers);
			Assert.NotNull(result.Coordinate);
			Assert.NotNull(result.Wind);
			Assert.AreEqual(1, result.Weathers.Count);
			Assert.AreEqual(TEST_CITY_ID, result.CityId);
			Assert.AreEqual(10000, result.Visibility);
			Assert.AreEqual(18, result.MainWeather.Temp);
			Assert.AreEqual(1020, result.MainWeather.Pressure);
			Assert.AreEqual(@"shower rain", result.Weathers[0].Description);
			Assert.AreEqual(@"Rain", result.Weathers[0].Main);

		}

		[Test]
		public async Task GetWeatherByCityIdTest_ShouldReturnNull()
		{
			var result = await WeatherDataProvider.GetCurrentWeatherByCityId(TEST_CITY_ID_FAIL);

			await WeatherHttpProxy.Received(1).GetFromUrl(TEST_URL_FAIL);

			
			Assert.Null(result);

		}
	}
}
