using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Server.Infrastructure.Authentication {

	public class CheckPermission : AuthorizeAttribute {

		protected override bool IsAuthorized(HttpActionContext actionContext) {
			ClaimsPrincipal principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
			this.LogClaims(principal);
			return base.IsAuthorized(actionContext);
		}

		private void LogClaims(ClaimsPrincipal principal) {
			Console.WriteLine("Principal has claims:");
			if (principal != null) {
				foreach (Claim claim in principal.Claims) {
					Console.WriteLine("\t" + claim.Type + " = " + claim.Value);
				}
			} else {
				Console.WriteLine("No principal");
			}
			Console.WriteLine("");
		}

		protected override void HandleUnauthorizedRequest(HttpActionContext ctx) {
			if (this.IsUserAuthenticated(ctx)) {
				this.Forbidden(ctx);
			} else {
				this.Unauthorized(ctx);
			}
		}

		private bool IsUserAuthenticated(HttpActionContext ctx) {
			IPrincipal principal = ctx.RequestContext.Principal;
			return (principal != null && principal.Identity.IsAuthenticated);
		}

		private void Forbidden(HttpActionContext ctx) {
			ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.Forbidden);
		}

		private void Unauthorized(HttpActionContext ctx) {
			ctx.Response = ctx.Request.CreateResponse(HttpStatusCode.Unauthorized);
		}

	}

}
