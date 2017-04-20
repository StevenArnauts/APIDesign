using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using Utilities.Entities;
using Utilities.Logging;

namespace Server.Persistence {

	/// <summary>
	/// Gives control over SaveChanges() or not when an exception occurs.
	/// </summary>
	internal class UnitOfWorkFilter : IAutofacActionFilter {

		public Task OnActionExecutedAsync(HttpActionExecutedContext ctx, CancellationToken cancellationToken) {
			Logger.Debug(this, "Executed...");
			IDependencyScope requestScope = ctx.Request.GetDependencyScope();
			IUnitOfWork<IDatabaseContext> service = requestScope.GetService(typeof(IUnitOfWork<IDatabaseContext>)) as IUnitOfWork<IDatabaseContext>;
			if (ctx.Exception == null) {
				service?.SaveChanges();
			}
			return (Task.FromResult(0));
		}

		public Task OnActionExecutingAsync(HttpActionContext ctx, CancellationToken cancellationToken) {
			return (Task.FromResult(0));
		}

	}

}