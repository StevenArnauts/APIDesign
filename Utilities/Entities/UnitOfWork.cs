using System;

namespace Utilities.Entities {

	public interface IUnitOfWork<TContext> : IDisposable where TContext : IDbContext {

		TContext Context { get; }
		void SaveChanges();

	}

}