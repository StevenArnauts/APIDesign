using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities {

	public static class DictionaryExtensions {

		public static string Print<TKey, TItem>(this Dictionary<TKey, TItem> source, string valueSeparator = "=", string pairSeparator = ", ") {
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < source.Count; i++) {
				KeyValuePair<TKey, TItem> valuePair = source.ElementAt(i);
				builder.Append(valuePair.Key);
				builder.Append(valueSeparator);
				builder.Append(valuePair.Value);
				if (i < (source.Count - 1)) builder.Append(pairSeparator);
			}
			return (builder.ToString());
		}

		public static string Print<TKey, TItem>(this Dictionary<TKey, TItem> source, Func<TItem, string> printer, string valueSeparator = "=", string pairSeparator = ", ") {
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < source.Count; i++) {
				KeyValuePair<TKey, TItem> valuePair = source.ElementAt(i);
				builder.Append(valuePair.Key);
				builder.Append(valueSeparator);
				builder.Append(printer(valuePair.Value));
				if (i < (source.Count - 1)) builder.Append(pairSeparator);
			}
			return (builder.ToString());
		}

		/// <summary>
		/// Adds the item to the dictionary if the key isn't already present, never overwrites.
		/// </summary>
		/// <returns>True if the key didn't exist and the item was added</returns>
		public static bool AddSafe<TKey, TValue>(this Dictionary<TKey, TValue> target, TKey key, TValue item) {
			if (!target.ContainsKey(key)) {
				target[key] = item;
				return (true);
			}
			return (false);
		}

	}

}
