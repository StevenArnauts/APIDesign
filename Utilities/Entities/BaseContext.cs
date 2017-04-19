using System.Data.Common;
using System.Data.Entity;

namespace Utilities.Entities {

	public abstract class BaseContext : DbContext, IDbContext {

		protected BaseContext() {}
		protected BaseContext(string connectionString) : base(connectionString) {}
		protected BaseContext(DbConnection connection) : base(connection, true) {}

		public IDbEntry Entry2(object entity) {
			return (new BaseDbEntry(base.Entry(entity)));
		}

		public new IDbEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class {
			return (new BaseDbEntry<TEntity>(base.Entry(entity)));
		}

	}

}