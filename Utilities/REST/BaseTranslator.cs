using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Utilities.WebApi;

namespace Utilities.Instrumentation {

	public abstract class BaseTranslator : IErrorTranslator {

		public IErrorTranslator InnerTranslator { get; set; }

		public ErrorTranslation Translate(Exception exception, HttpRequestMessage request) {
			ErrorTranslation translation = this.InternalTranslate(exception, request);
			if (translation == null) {
				if (this.InnerTranslator != null) { translation = this.InnerTranslator.Translate(exception, request); }
			}
			if (translation == null) {
				translation = this.CreateTranslation(request, HttpStatusCode.InternalServerError, exception);
			}
			return (translation);
		}

		protected abstract ErrorTranslation InternalTranslate(Exception exception, HttpRequestMessage request);

		protected ErrorTranslation CreateTranslation(HttpRequestMessage request, HttpStatusCode statusCode, Exception exception) {
			Error error = new Error();
			Exception rootCause = exception.GetBaseException();
			error.Problems.Add(new Error.Problem { Code = rootCause.GetType().Name, Message = MakeJsonSafe(rootCause.Message) });
			HttpResponseMessage response = new HttpResponseMessage(statusCode) {
				Content = new StringContent(DefaultProtocol.Serializer.Serialize(error), Encoding.UTF8, "application/json")
			};
			ErrorTranslation translation = new ErrorTranslation(error, response);
			return (translation);
		}

		protected static string MakeJsonSafe(string source) {
			if (string.IsNullOrEmpty(source)) { return (source); }
			return (source.Replace('"', '\'').Replace(Environment.NewLine, " "));
		}

	}

}
