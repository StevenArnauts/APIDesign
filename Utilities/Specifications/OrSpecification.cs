using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Utilities.Specifications {

	/// <summary>
	/// A Logic OR AbstractSpecification
	/// </summary>
	/// <typeparam name="T">Type of entity that check this AbstractSpecification</typeparam>
	[ExcludeFromCodeCoverage]
	public class OrSpecification<T> : CompositeSpecification<T> where T : class {

		private readonly ISpecification<T> _rightSideSpecification;
		private readonly ISpecification<T> _leftSideSpecification;

		/// <summary>
		/// Default constructor for AndSpecification
		/// </summary>
		/// <param name="leftSide">Left side AbstractSpecification</param>
		/// <param name="rightSide">Right side AbstractSpecification</param>
		public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide) {
			if(leftSide == null) throw new ArgumentNullException("leftSide");
			if(rightSide == null) throw new ArgumentNullException("rightSide");
			this._leftSideSpecification = leftSide;
			this._rightSideSpecification = rightSide;
		}

		/// <summary>
		/// Left side AbstractSpecification
		/// </summary>
		public override ISpecification<T> LeftSideSpecification {
			get { return this._leftSideSpecification; }
		}

		/// <summary>
		/// Righ side specification
		/// </summary>
		public override ISpecification<T> RightSideSpecification {
			get { return this._rightSideSpecification; }
		}

		public override Expression<Func<T, bool>> IsSatisfiedBy() {
			Expression<Func<T, bool>> left = this._leftSideSpecification.IsSatisfiedBy();
			Expression<Func<T, bool>> right = this._rightSideSpecification.IsSatisfiedBy();
			return ( ExpressionBuilder.Or(left, right) );
		}

	}

}
