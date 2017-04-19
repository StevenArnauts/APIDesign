namespace Utilities.Entities {

	public abstract class BaseRepository<TContext> : IRepository<TContext> where TContext : IDbContext {

		protected BaseRepository(IUnitOfWork<TContext> unitOfWork) {
			this.UnitOfWork = unitOfWork;
		}

		public void Flush() {
			if (this.UnitOfWork != null) {
				this.UnitOfWork.SaveChanges();
			}
		}

		public IUnitOfWork<TContext> UnitOfWork { get; }

	}

}