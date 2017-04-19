using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utilities.Instrumentation;

namespace Utilities {

	public static class QueryableExtensions {

		public static T Get<T>(this IQueryable<T> source, Expression<Func<T, bool>> condition) {
			List<T> candidates = source.Where(condition).ToList();
			if (!candidates.Any()) throw new ObjectNotFoundException("No instance of " + typeof(T).Name + " satisfied the criteria");
			if (candidates.Count() > 1) throw new ObjectNotUniqueException("There were " + candidates.Count() + " instances of " + typeof(T).Name + " that satisfied the criteria");
			return (candidates[0]);
		}

	}

}
