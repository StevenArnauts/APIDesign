using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Utilities {

	public static class StringHelper {

		public static string ToCamelCase(this string source) {
			return ( source[0].ToString(CultureInfo.InvariantCulture).ToLower() + source.Substring(1) );
		}

		public static string ToPascalCase(this string source) {
			return ( source[0].ToString(CultureInfo.InvariantCulture).ToUpper() + source.Substring(1) );
		}

		public static string ToTitleCase(this string source) {
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source.ToLowerInvariant());
		}

		public static string Reverse(this string source)
		{
			char[] charArray = source.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}

		public static string DecimalFormatter(decimal value)
		{
			return value.ToString("N", new CultureInfo("nl-BE"));
		}

		public static string AmountFormatter(decimal value)
		{
			return string.Format("{0} €", DecimalFormatter(value));
		}

		public static string ToAlphaNumericOnly(string value) {
			if (String.IsNullOrEmpty(value)) return "";
			return Regex.Replace(value, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
		}

		public static string ToNumericOnly(string value) {
			if (String.IsNullOrEmpty(value)) return "";
			return Regex.Replace(value, "[^0-9]+", "", RegexOptions.Compiled);
		}

	}

}
