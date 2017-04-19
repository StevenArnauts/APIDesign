using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Utilities.WebApi;

namespace Utilities.Instrumentation {

	public class TraceRequestAttribute : ActionFilterAttribute {

		public override void OnActionExecuting(HttpActionContext actionContext) {
			Console.WriteLine(actionContext.ActionDescriptor.ControllerDescriptor.ControllerType + " " + actionContext.Request.RequestUri + " => " + actionContext.ActionDescriptor.ActionName + "(" + actionContext.ActionArguments.Print(DefaultProtocol.Serializer.Serialize) + ")");
		}

		public override void OnActionExecuted(HttpActionExecutedContext context) {
			if (context.Exception != null) {
				Console.WriteLine(context.ActionContext.ControllerContext.Request.RequestUri + " => " + context.Exception.GetBaseException().Message);
			}
		}

	}

}