using System;
using System.Web.Http;
using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Server.Configuration;
using Utilities.Logging;

namespace Server {

	public class Startup {

		public void Configuration(IAppBuilder app) {
			try {
				Logger.Appenders.Add(new ConsoleAppender());
				Logger.Level = Level.Debug;
				Logger.Info(this, "Configuring...");
				app.UseCors(CorsOptions.AllowAll);
				app.Use<OwinRequestLogger>();
				//ConfigureSignalR(app);
				ConfigureWebApi(app);
				Logger.Info(this, "Configured");
			} catch (Exception ex) {
				Console.WriteLine("Startup failed : " + ex.Message);
			}
		}

		private static void ConfigureWebApi(IAppBuilder app) {
			HttpConfiguration configuration = WebApiConfig.Create();
			IContainer container = ContainerConfig.Configure(configuration);
			app.Map("/api", map => {
				app.UseAutofacMiddleware(container);
				map.UseAutofacWebApi(configuration);
				map.UseWebApi(configuration);
			});
		}

		private static void ConfigureSignalR(IAppBuilder app) {
			// string connectionString = "Endpoint=sb://sar-test-signalr.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tPoJkP2xnaEki+qkj3J9nw6fsyoXfSYoeiUu54NqAWY=";
			// GlobalHost.DependencyResolver.UseServiceBus(connectionString, "SARSignalRTest");
			app.Map("/signalr", map => {
				map.UseCors(CorsOptions.AllowAll);
				HubConfiguration hubConfiguration = new HubConfiguration {
					EnableJSONP = true // JSONP requests are insecure but some older browsers (and some versions of IE) require JSONP to work cross domain
				};
				map.RunSignalR(hubConfiguration);
			});
		}

	}

}