using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Server.Infrastructure.Authentication {

	internal static class ClaimsExtensions {

		/// <summary>
		/// Returns the value of the first claim in the collection with the specified type
		/// </summary>
		public static string GetValue(this IEnumerable<Claim> source, string type) {
			Claim result = source.FirstOrDefault(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
			return result?.Value;
		}

	}

}
