using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Instrumentation;

namespace Utilities {

	public static class EnumerableExtensions {

		/// <summary>
		/// Tries to get a unique item from the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="condition"></param>
		/// <param name="result"></param>
		/// <returns>True if the item was found and was unique, false otherwise</returns>
		public static bool TryGet<T>(this IEnumerable<T> source, Predicate<T> condition, out T result) {
			try {
				result = Get(source, condition);
				return (true);
			} catch (QueryException) {
				result = default(T);
				return (false);
			}
		}

		public static T Get<T>(this IEnumerable<T> source, Predicate<T> condition) {
			List<T> candidates = source.Where(i => condition(i)).ToList();
			if (!candidates.Any()) throw new ObjectNotFoundException("No instance of " + typeof(T).Name + " satisfied the criteria");
			if (candidates.Count > 1) throw new ObjectNotUniqueException("There were " + candidates.Count + " instances of " + typeof(T).Name + " that satisfied the criteria");
			return (candidates[0]);
		}

		public static string Print<TItem>(this IEnumerable<TItem> source, Func<TItem, string> formatter) {
			string result = string.Empty;
			foreach (TItem item in source) {
				result += formatter.Invoke(item);
			}
			return (result);
		}

		public static void ForEach<TItem>(this IEnumerable<TItem> source, Action<TItem> action) {
			foreach (TItem item in source) action(item);
		}

		public static string Print<T>(this IEnumerable<T> source, string separator) {
			StringBuilder bldr = new StringBuilder();
			IList<T> items = new List<T>(source);
			for (int i = 0; i < items.Count; i++) {
				bldr.Append(items[i]);
				if (i < items.Count - 1) bldr.Append(separator);
			}
			return (bldr.ToString());
		}

	}

}
