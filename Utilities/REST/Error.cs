using System;
using System.Collections.Generic;
using Utilities.Extensions;

namespace Utilities.Instrumentation {

	public class Error {

		private readonly List<Problem> _errors = new List<Problem>();

		public Error() {
			this.Id = Guid.NewGuid().Format();
		}

		public string Id { get; set; }

		public ICollection<Problem> Problems {
			get { return (this._errors); }
		}

		public class Problem {

			public string Code { get; set; }
			public string Message { get; set; }

		}

	}

}
