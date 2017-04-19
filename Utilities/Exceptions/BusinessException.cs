using System;

namespace Utilities {

	public class BusinessException : ApplicationException {

		public BusinessException() {}
		public BusinessException(string message) : base(message) {}

	}

}