using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Utilities.Entities {

	public class BaseDbEntry<TEntity> : IDbEntry<TEntity> where TEntity : class {

		private readonly DbEntityEntry<TEntity> _entry;

		public BaseDbEntry(DbEntityEntry<TEntity> entry) {
			this._entry = entry;
		}

		public IDbCollection Collection<TElement>(Expression<Func<TEntity, ICollection<TEntity>>> navigationProperty) {
			//var collection = this._entry.Collection<TElement>(navigationProperty);
			//return (new BaseDbCollection<TEntity, TElement>(collection));
			throw new NotImplementedException();
		}

	}

	public class BaseDbEntry : IDbEntry {

		private readonly DbEntityEntry _entry;

		public BaseDbEntry(DbEntityEntry entry) {
			this._entry = entry;
		}

		public IDbCollection Collection(string navigationProperty) {
			var collection = this._entry.Collection(navigationProperty);
			return (new BaseDbCollection(collection));
		}

	}

}