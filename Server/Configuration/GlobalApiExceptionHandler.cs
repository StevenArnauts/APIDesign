using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Utilities.Instrumentation;
using Utilities.Logging;
using Utilities.WebApi;

namespace Server.Configuration {

	public class ErrorResult : IHttpActionResult {

		private readonly ErrorTranslation _translation;

		public ErrorResult(ErrorTranslation translation) {
			this._translation = translation;
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
			return (Task.FromResult(this._translation.Response));
		}

	}

	public class GlobalApiExceptionHandler : ExceptionHandler {

		public override void Handle(ExceptionHandlerContext context) {
			context.Result = this.InternalHandle(context);
		}

		public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken) {
			context.Result = this.InternalHandle(context);
			return (Task.FromResult(0));
		}

		private IHttpActionResult InternalHandle(ExceptionHandlerContext context) {
			ErrorTranslation translation = ErrorTranslatorFactory.Translator.Translate(context.Exception, context.Request);
			Logger.Error(this, context.Request.RequestUri.AbsoluteUri + " failed, error id = " + translation.Id, context.Exception);
			IHttpActionResult result = new ErrorResult(translation);
			return result;
		}

		public override bool ShouldHandle(ExceptionHandlerContext context) {
			return true;
		}

	}

}
