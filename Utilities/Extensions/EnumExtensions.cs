using System;

namespace Utilities.Extensions {

	public static class EnumExtensions {

		public static T ToEnum<T>(this string s) where T : struct {
			return (T) Enum.Parse(typeof(T), s);
		}

		public static T ToEnumOrDefault<T>(this string s) where T : struct {
			T newValue;
			return Enum.TryParse(s, out newValue) ? newValue : default(T);
		}

	}

}
