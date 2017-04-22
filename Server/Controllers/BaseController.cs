using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Autofac.Integration.WebApi;

namespace Server.Controllers {

	public interface IRouteLinker {

		string Link(string route, object routeValues);

	}

	[AutofacControllerConfiguration]
	public class BaseController : ApiController, IRouteLinker {

		public string Link(string route, object routeValues) {
			return (this.Url.Link(route, routeValues));
		}

	}

}