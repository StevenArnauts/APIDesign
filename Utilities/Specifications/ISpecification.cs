using System;
using System.Linq.Expressions;

namespace Utilities.Specifications {

	/// <summary>
	/// Base contract for AbstractSpecification pattern, for more information
	/// about this pattern see http://martinfowler.com/apsupp/spec.pdf
	/// or http://en.wikipedia.org/wiki/Specification_pattern.
	/// Really this is variant implementation for add feature of linq and
	/// lambda expression into this pattern.
	/// </summary>
	/// <typeparam name="TEntity">Type of entity</typeparam>
	public interface ISpecification<TEntity> where TEntity : class {

		/// <summary>
		/// Check if this specification is satisfied by a 
		/// specific expression lambda
		/// </summary>
		/// <returns></returns>
		Expression<Func<TEntity, bool>> IsSatisfiedBy();

	}

}
