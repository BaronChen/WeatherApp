using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using WeatherApp.Services.DataProvider;
using WeatherApp.Services.DataProvider.helper;
using WeatherApp.Services.WeatherService.impl;
using WeatherApp.Services.WeatherService.interfaces;

namespace WeatherApp.Web.App_Start
{
	public static class RegisterDependencies
	{
		public static void ConfigureDependency(HttpConfiguration httpConfiguration)
		{
			var container = new Container();
			container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

			container.RegisterSingleton<CityDataProvider, CityDataProvider>();
			container.RegisterSingleton<IWeatherHttpProxy, WeatherHttpProxy>();
			container.RegisterSingleton<WeatherDataProvider, WeatherDataProvider>();
			container.RegisterSingleton<IWeatherService, WeatherService>();

			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

			container.Verify();


			var dependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
			httpConfiguration.DependencyResolver = dependencyResolver;

			GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;

		}

	}
}