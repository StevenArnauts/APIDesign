using System;
using System.Net;
using System.Net.Http;

namespace Utilities.Instrumentation {

	public class DefaultErrorTranslator : BaseTranslator {

		protected override ErrorTranslation InternalTranslate(Exception exception, HttpRequestMessage request) {
			if (exception is OperationNotAllowedException) return (this.CreateTranslation(request, HttpStatusCode.BadRequest, exception));
			if (exception is ObjectNotFoundException) return (this.CreateTranslation(request, HttpStatusCode.NotFound, exception));
			if (exception is ValidationException) {
				ErrorTranslation translation = this.CreateTranslation(request, HttpStatusCode.NotAcceptable, exception);
				((ValidationException) exception).Errors.ForEach(e => translation.Error.Problems.Add(new Error.Problem { Message = e }));
				return (translation);
			}
			return (null);
		}

	}

}
