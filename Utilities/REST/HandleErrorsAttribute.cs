using System.Web.Http.Filters;

namespace Utilities.Instrumentation {

	public class HandleErrorsAttribute : ActionFilterAttribute {

		public override void OnActionExecuted(HttpActionExecutedContext context) {
			if (context.Exception != null) {
				ErrorTranslation translation = ErrorTranslatorFactory.Translator.Translate(context.Exception, context.Request);
				context.Response = translation.Response;
			}
		}

	}

}
