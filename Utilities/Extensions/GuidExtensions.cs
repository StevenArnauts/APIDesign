using System;

namespace Utilities.Extensions {

	public static class GuidExtensions {

		public static string Format(this Guid source) {
			return (source.ToString("N").ToUpper());
		}

	}

}
