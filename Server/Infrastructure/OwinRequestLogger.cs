using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Logging;

namespace Server {

	internal class OwinRequestLogger {

		private readonly Func<IDictionary<string, object>, Task> _next;

		public OwinRequestLogger(Func<IDictionary<string, object>, Task> next) {
			this._next = next;
		}

		public async Task Invoke(IDictionary<string, object> environment) {
			string message = environment["owin.RequestMethod"] + " " + environment["owin.RequestPath"];
			try {
				await this._next(environment);
				message += " : " + environment["owin.ResponseStatusCode"];
				Logger.Info(this, message);
			} catch (Exception ex) {
				Logger.Error(this, message, ex);
			}
			
		}

	}

}