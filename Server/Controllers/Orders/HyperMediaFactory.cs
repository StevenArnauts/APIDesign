using System;
using Utilities;
using Utilities.Extensions;

namespace Server.Controllers {

	public abstract class HyperMediaFactory {

		private readonly IRouteLinker _linker;

		protected HyperMediaFactory(IRouteLinker linker) {
			this._linker = linker;
		}

		protected IRouteLinker Linker {
			get { return (this._linker); }
		}

		protected string Link(string routeName, object routeValues) {
			return (this._linker.Link(routeName, this.PrepareRouteValues(routeValues)));
		}

		private object PrepareRouteValues(object routeValues) {
			return routeValues?.AsMap((o) => { return (o is Guid ? ((Guid)o).Format() : o); });
		}

	}

}