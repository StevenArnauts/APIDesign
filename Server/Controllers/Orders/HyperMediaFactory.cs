using System;
using System.Net.Http;
using Utilities;
using Utilities.Extensions;

namespace Server.Controllers {

	public abstract class HyperMediaFactory {

		private readonly UrlHelper _urlHelper;

		protected HyperMediaFactory(HttpRequestMessage request) {
			this._urlHelper = new UrlHelper(request);
		}

		protected string Link(string routeName, object routeValues) {
			return (this._urlHelper.Link(routeName, this.PrepareRouteValues(routeValues)));
		}

		private object PrepareRouteValues(object routeValues) {
			return routeValues?.AsMap((o) => { return (o is Guid ? ((Guid)o).Format() : o); });
		}

	}

}