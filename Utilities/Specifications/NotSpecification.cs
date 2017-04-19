using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Utilities.Specifications {

	/// <summary>
	/// NotEspecification convert a original
	/// AbstractSpecification with NOT logic operator
	/// </summary>
	/// <typeparam name="TEntity">Type of element for this specificaiton</typeparam>
	[ExcludeFromCodeCoverage]
	public class NotSpecification<TEntity> : AbstractSpecification<TEntity> where TEntity : class {

		private readonly Expression<Func<TEntity, bool>> _originalCriteria;

		/// <summary>
		/// Constructor for NotSpecificaiton
		/// </summary>
		/// <param name="originalSpecification">Original AbstractSpecification</param>
		public NotSpecification(ISpecification<TEntity> originalSpecification) {
			if(originalSpecification == null) throw new ArgumentNullException("originalSpecification");
			this._originalCriteria = originalSpecification.IsSatisfiedBy();
		}

		/// <summary>
		/// Constructor for NotSpecification
		/// </summary>
		/// <param name="originalSpecification">Original specificaiton</param>
		public NotSpecification(Expression<Func<TEntity, bool>> originalSpecification) {
			if(originalSpecification == null) throw new ArgumentNullException("originalSpecification");
			this._originalCriteria = originalSpecification;
		}

		public override Expression<Func<TEntity, bool>> IsSatisfiedBy() {
			return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(this._originalCriteria.Body), this._originalCriteria.Parameters.Single());
		}

	}

}
