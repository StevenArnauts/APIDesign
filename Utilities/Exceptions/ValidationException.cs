using System.Collections.Generic;

namespace Utilities {

	public class ValidationException : BusinessException {

		private readonly List<string> _errors = new List<string>();

		public ValidationException(string message, params string[] errors) : base(message) {
			foreach (string error in errors) this._errors.Add(error);
		}

		public ValidationException(string message, IEnumerable<string> errors) : base(message) {
			foreach (string error in errors) this._errors.Add(error);
		}

		public IEnumerable<string> Errors {
			get { return (this._errors.AsReadOnly()); }
		}

	}

}
