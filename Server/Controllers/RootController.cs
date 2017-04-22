using System.Web.Http;
using Utilities;

namespace Server.Controllers {

	[RoutePrefix("")]
	public class RootController : BaseController {

		[HttpGet]
		[Route("")]
		public RootRepresentation Root() {
			RootRepresentation rootRepresentation = new RootRepresentation {
				HyperMedia = {
					Factory = new RootHyperMediaFactory(this).Setup
				}
			};
			return rootRepresentation;
		}

	}

	public class RootHyperMediaFactory : HyperMediaFactory {

		public RootHyperMediaFactory(IRouteLinker linker) : base(linker) {}

		public void Setup(Representation representation) {
			representation.Links.AddSafe("customers", new Link(this.Link("CUSTOMERS_ALL", new {})));
			representation.Links.AddSafe("orders", new Link(this.Link("ORDERS_ALL", new {})));
		}

	}

}