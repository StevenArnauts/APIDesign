using System.Net;
using System.Net.Http;

namespace Utilities.Instrumentation {

	public class ErrorTranslation {

		private readonly Error _error;
		private readonly HttpResponseMessage _response;

		public ErrorTranslation(Error error, HttpResponseMessage response) {
			this._response = response;
			this._error = error;
		}

		public HttpResponseMessage Response {
			get { return (this._response); }
		}

		public HttpStatusCode StatusCode {
			get { return (this._response.StatusCode); }
		}

		public Error Error {
			get { return (this._error); }
		}

		public string Id {
			get { return (this._error.Id); }
		}

	}

}