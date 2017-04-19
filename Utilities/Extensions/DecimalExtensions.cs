using System;

namespace Utilities {

	public static class DecimalExtensions {

		public static decimal Normalize(this Decimal value) {
			return value/1.000000000000000000000000000000000m;
		}

		public static string ToStringWithTrailingZeros(this Decimal value, int decimals = 2) {
			decimals = Math.Max(0, decimals);
			string format = "0.";
			for (int i = 0; i < decimals; i++) { format += "0"; }
			for (int i = 0; i < (6 - decimals); i++) { format += "#"; }

			return value.Normalize().ToString(format); // default: "0.00####"
		}

	}

}
