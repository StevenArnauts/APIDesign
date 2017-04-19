using System;
using Microsoft.Owin.Extensions;
using Owin;

namespace Server.Infrastructure.Authentication {

	internal static class BasicAuthenticationExtensions {

		internal static IAppBuilder UseBasicAuthentication(this IAppBuilder app, BasicAuthenticationOptions options) {
			if (app == null) {
				throw new ArgumentNullException(nameof(app));
			}

			app.Use<BasicAuthenticationMiddleware>(options);
			app.UseStageMarker(PipelineStage.Authenticate);
			return app;
		}

	}

}
