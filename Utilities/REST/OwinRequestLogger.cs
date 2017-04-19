using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Instrumentation {

	internal class OwinRequestLogger {

		private readonly Func<IDictionary<string, object>, Task> _next;

		public OwinRequestLogger(Func<IDictionary<string, object>, Task> next) {
			this._next = next;
		}

		public async Task Invoke(IDictionary<string, object> environment) {
			string message = environment["owin.RequestMethod"] + " " + environment["owin.RequestPath"];
			await this._next(environment);
			message += " : " + environment["owin.ResponseStatusCode"];
			Console.WriteLine("Owin: " + message);
		}

	}

}