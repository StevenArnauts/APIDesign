using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Utilities;
using Utilities.Instrumentation;
using Utilities.REST;

namespace Server.Configuration {

	public class WebApiConfig {

		public static HttpConfiguration Create() {
			HttpConfiguration configuration = new HttpConfiguration();
			configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
			configuration.Formatters.Remove(configuration.Formatters.JsonFormatter);
			configuration.Formatters.Add(new HalPlusJsonMediaTypeFormatter());
			configuration.Formatters.Add(new JsonMediaTypeFormatter());
			configuration.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter()); // fallback
			configuration.Formatters.OfType<System.Net.Http.Formatting.JsonMediaTypeFormatter>().First().SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
			configuration.Filters.Add(new HandleErrorsAttribute());
			configuration.Services.Replace(typeof(IExceptionHandler), new GlobalApiExceptionHandler());
			DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver();
			constraintResolver.ConstraintMap.Add("id", typeof(IdConstraint));
			configuration.MapHttpAttributeRoutes(constraintResolver);
			configuration.EnableCors();
			return (configuration);
		}

	}

}
