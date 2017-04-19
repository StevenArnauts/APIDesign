using System.Net;

namespace Utilities.WebApi {

	public class HttpResult<TResponse> {

		public HttpResult(TResponse response, HttpStatusCode httpStatusCode) {
			this.Response = response;
			this.HttpStatusCode = httpStatusCode;
		}

		public TResponse Response { get; }

		public HttpStatusCode HttpStatusCode { get; }

	}

}
