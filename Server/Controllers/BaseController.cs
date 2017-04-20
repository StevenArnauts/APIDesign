using System.Net.Http;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Utilities;

namespace Server.Controllers {

	[AutofacControllerConfiguration]
	public class BaseController : ApiController {

	}

	public class RootRepresentation : Representation {

	}

	[RoutePrefix("")]
	public class RootController : BaseController {

		[HttpGet]
		[Route("")]
		public RootRepresentation Root() {
			RootRepresentation rootRepresentation = new RootRepresentation();
			rootRepresentation.HyperMedia.Factory = new RootHyperMediaFactory(this.Request).Setup;
			return rootRepresentation;
		}

		public class RootHyperMediaFactory : HyperMediaFactory {

			public RootHyperMediaFactory(HttpRequestMessage request) : base(request) {}

			public void Setup(Representation representation) {
				representation.Links.AddSafe("customers", new Link {
					Href = this.Link("CUSTOMERS_ALL", new {})
				});
				representation.Links.AddSafe("orders", new Link {
					Href = this.Link("ORDERS_ALL", new { })
				});
			}

		}

	}

}