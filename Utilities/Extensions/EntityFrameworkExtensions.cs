using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Utilities {

	public static class EntityFrameworkExtensions {

		public static IQueryable<T> QueryLocalOrDatabase<T>(this IDbSet<T> context, Expression<Func<T, bool>> expression) where T : class {
			List<T> localResults = context.Local == null ? null : context.Local.Where(expression.Compile()).ToList();
			if (localResults != null && localResults.Any()) return localResults.AsQueryable();
			return(context.Where(expression));
		}

		public static T GetLocalOrDatabase<T>(this IDbSet<T> context, Expression<Func<T, bool>>  expression) where T : class {
			IQueryable<T> candidates = QueryLocalOrDatabase(context, expression);
			if (!candidates.Any()) throw new ObjectNotFoundException();
			if (candidates.Count() > 1) throw new ObjectNotUniqueException();
			return (candidates.First());
		}

		public static IQueryable<T> QueryLocalOrDatabase<T>(this DbContext context, Expression<Func<T, bool>> expression) where T : class {
			List<T> localResults = context.Set<T>().Local == null ? null : context.Set<T>().Local.Where(expression.Compile()).ToList();
			if (localResults != null && localResults.Any()) {
				return (localResults.AsQueryable());
			}
			IQueryable<T> databaseResults = context.Set<T>().Where(expression);
			return (databaseResults);
		}

		public static T GetLocalOrDatabase<T>(this DbContext context, Expression<Func<T, bool>> expression) where T : class {
			List<T> localResults = context.Set<T>().Local.Where(expression.Compile()).ToList();
			if (localResults.Any()) {
				if(localResults.Count() > 1) throw new ObjectNotUniqueException();
				return (localResults.First());
			}
			IQueryable<T> databaseResults = context.Set<T>().Where(expression);
			if(!databaseResults.Any()) throw new ObjectNotFoundException();
			if (databaseResults.Count() > 1) throw new ObjectNotUniqueException();
			return (databaseResults.First());
		}

	}

}