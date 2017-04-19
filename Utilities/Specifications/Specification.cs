using System;
using System.Linq.Expressions;

namespace Utilities.Specifications {

	/// <summary>
	/// A Direct AbstractSpecification is a simple implementation
	/// of AbstractSpecification that acquire this from a lambda expression
	/// in  constructor
	/// </summary>
	/// <typeparam name="TEntity">Type of entity that check this AbstractSpecification</typeparam>
	public class Specification<TEntity> : AbstractSpecification<TEntity> where TEntity : class {

		readonly Expression<Func<TEntity, bool>> _matchingCriteria;

		/// <summary>
		/// Default constructor for Direct AbstractSpecification
		/// </summary>
		/// <param name="matchingCriteria">A Matching Criteria</param>
		public Specification(Expression<Func<TEntity, bool>> matchingCriteria) {
			if (matchingCriteria == null) throw new ArgumentNullException("matchingCriteria");
			this._matchingCriteria = matchingCriteria;
		}

		public override Expression<Func<TEntity, bool>> IsSatisfiedBy() {
			return this._matchingCriteria;
		}

	}

}
