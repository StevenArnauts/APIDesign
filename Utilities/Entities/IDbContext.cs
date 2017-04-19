using System;

namespace Utilities.Entities {

	public interface IDbContext : IDisposable {

		IDbEntry Entry2(object entity);
		IDbEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
		int SaveChanges();

	}

}