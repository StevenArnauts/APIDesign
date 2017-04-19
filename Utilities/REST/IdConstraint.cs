using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Utilities {

	public class IdConstraint : IHttpRouteConstraint {

		public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection) {
			object value;
			if (values.TryGetValue(parameterName, out value) && value != null) {
				Guid guid;
				if (value is Guid) {
					guid = (Guid) value;
					return this.IsValid(guid);
				}
				string valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
				if (Guid.TryParse(valueString, out guid)) {
					return guid != Guid.Empty;
				}
			}
			return false;
		}

		private bool IsValid(Guid id) {
			return (id != Guid.Empty);
		}

	}

}