namespace Utilities.Entities {

	public abstract class BaseFactory<TContext> : IFactory where TContext : IDbContext {

		protected BaseFactory(IUnitOfWork<TContext> unitOfWork) {
			this.UnitOfWork = unitOfWork;
		}

		protected IUnitOfWork<TContext> UnitOfWork { get; }

		public void Flush() {
			this.UnitOfWork.SaveChanges();
		}

	}

}