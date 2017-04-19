namespace Utilities.Entities {

	public interface IRepository<TContext> where TContext : IDbContext {

		IUnitOfWork<TContext> UnitOfWork { get; }

		/// <summary>
		/// Persists all changes to the underlying datastore
		/// </summary>
		void Flush();

	}

}