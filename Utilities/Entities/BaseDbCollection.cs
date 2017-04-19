using System.Data.Entity.Infrastructure;

namespace Utilities.Entities {

	public class BaseDbCollection : IDbCollection {

		private readonly DbCollectionEntry _collection;

		public BaseDbCollection(DbCollectionEntry collection) {
			this._collection = collection;
		}

		public void Load() {
			this._collection.Load();
		}

	}

	public class BaseDbCollection<TEntity, TElement> : IDbCollection where TEntity : class {

		private readonly DbCollectionEntry<TEntity, TElement> _collection;

		public BaseDbCollection(DbCollectionEntry<TEntity, TElement> collection) {
			this._collection = collection;
		}

		public void Load() {
			this._collection.Load();
		}

	}

}