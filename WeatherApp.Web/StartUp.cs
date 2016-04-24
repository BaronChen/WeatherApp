using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using WeatherApp.Web.App_Start;

namespace WeatherApp.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			HttpConfiguration httpConfiguration = new HttpConfiguration();
			
			WebApiConfig.Register(httpConfiguration);

			RegisterDependencies.ConfigureDependency(httpConfiguration);
			
			appBuilder.UseWebApi(httpConfiguration);
		}
	}
}