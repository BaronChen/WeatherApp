using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WeatherApp.Services.DataProvider;

namespace WeatherApp.Test.WeatherServiceTest
{
	[TestFixture]
	public class CityDataProviderTest
	{
		private CityDataProvider CityDataProvider;

		const string TEST_CITY_ID = "2147714";
		const string TEST_CITY_NAME = "Sydney";
		const string TEST_CITY_COUNTRY = "AU";
		const decimal TEST_CITY_LON = 151.207321m;
		const decimal TEST_CITY_LAT = -33.867851m;

		[SetUp]
		public void Init()
		{
			if (this.CityDataProvider == null)
			{
				this.CityDataProvider = new CityDataProvider();
			}
		}


		[Test]
		public void GetCityByIdTest_ShouldGetACityById()
		{
			var city = CityDataProvider.GetCityById(TEST_CITY_ID);

			Assert.AreEqual(city.Name, TEST_CITY_NAME);
			Assert.AreEqual(city.Id, TEST_CITY_ID);
			Assert.AreEqual(city.Country, TEST_CITY_COUNTRY);
			Assert.NotNull(city.Coordinate);
			Assert.AreEqual(city.Coordinate.Lon, TEST_CITY_LON);
			Assert.AreEqual(city.Coordinate.Lat, TEST_CITY_LAT);
		}


		[Test]
		public void GetCitiesByNameTest_ShouldGetCitiesByName()
		{
			var result = CityDataProvider.GetCitiesByName(TEST_CITY_NAME);

			Assert.AreEqual(result.Count, 10);
		}


		[Test]
		public void GetCitiesByNameTest_ShouldGetNothing()
		{
			var result = CityDataProvider.GetCitiesByName(string.Empty);

			Assert.AreEqual(result.Count, 0);
		}
	}
}
