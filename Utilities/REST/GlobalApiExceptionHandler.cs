using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Utilities.Logging;

namespace Utilities.REST {

	public class GlobalApiExceptionHandler : ExceptionHandler
	{
		public override void Handle(ExceptionHandlerContext context)
		{
			Logger.Error(this, context.Exception);
			context.Result = new InternalServerErrorResult(context.Request);
		}

		public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken) {
			Logger.Error(this, context.Exception);
			context.Result = new InternalServerErrorResult(context.Request);
			return (Task.FromResult(0));
		}

		public override bool ShouldHandle(ExceptionHandlerContext context) {
			return true;
		}

	}

}