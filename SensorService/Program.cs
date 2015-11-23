using System;
using System.Net.Http.Headers;
using Microsoft.Owin.Hosting;
using Owin;
using System.Web.Http;

namespace SensorService
{
    public class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine("Hello Kitty!");

            using (WebApp.Start<Startup>("http://192.168.1.144:8080")) 
			{
			    Console.ReadKey();
			}
		}
	}

	public class Startup
	{
		public Startup()
		{
			
		}

		public void Configuration(IAppBuilder appBuilder)
		{
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });

            appBuilder.UseWebApi(config);
		}
	}
}
