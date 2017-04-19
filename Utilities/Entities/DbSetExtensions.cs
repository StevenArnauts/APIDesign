using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Utilities.Entities {

	public static class DbSetExtensions {

		public static void Remove<TItem>(this IDbSet<TItem> source, Expression<Func<TItem, bool>> condition) where TItem: class {
			List<TItem> items = source.Where(condition).ToList();
			items.ForEach(i => source.Remove(i));
		}

	}

}