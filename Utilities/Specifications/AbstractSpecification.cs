using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Utilities.Specifications {

	/// <summary>
	/// Represent a Expression AbstractSpecification
	/// <remarks>
	/// AbstractSpecification overload operators for create AND,OR or NOT specifications.
	/// Additionally overload AND and OR operators with the same sense of ( binary And and binary Or ).
	/// C# couldn’t overload the AND and OR operators directly since the framework doesn’t allow such craziness. But
	/// with overloading false and true operators this is posible. For explain this behavior please read
	/// http://msdn.microsoft.com/en-us/library/aa691312(VS.71).aspx
	/// </remarks>
	/// </summary>
	/// <typeparam name="TEntity">Type of item in the criteria</typeparam>
	[ExcludeFromCodeCoverage]
	public abstract class AbstractSpecification<TEntity> : ISpecification<TEntity> where TEntity : class {

		/// <summary>
		/// IsSatisFied AbstractSpecification pattern method,
		/// </summary>
		/// <returns>Expression that satisfy this AbstractSpecification</returns>
		public abstract Expression<Func<TEntity, bool>> IsSatisfiedBy();

		/// <summary>
		///  And operator
		/// </summary>
		/// <param name="leftSideSpecification">left operand in this AND operation</param>
		/// <param name="rightSideSpecification">right operand in this AND operation</param>
		/// <returns>New AbstractSpecification</returns>
		public static AbstractSpecification<TEntity> operator &(AbstractSpecification<TEntity> leftSideSpecification, AbstractSpecification<TEntity> rightSideSpecification) {
			return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
		}
		/// <summary>
		/// Or operator
		/// </summary>
		/// <param name="leftSideSpecification">left operand in this OR operation</param>
		/// <param name="rightSideSpecification">left operand in this OR operation</param>
		/// <returns>New </returns>
		public static AbstractSpecification<TEntity> operator |(AbstractSpecification<TEntity> leftSideSpecification, AbstractSpecification<TEntity> rightSideSpecification) {
			return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
		}
		/// <summary>
		/// Not AbstractSpecification
		/// </summary>
		/// <param name="specification">AbstractSpecification to negate</param>
		/// <returns>New AbstractSpecification</returns>
		public static AbstractSpecification<TEntity> operator !(AbstractSpecification<TEntity> specification) {
			return new NotSpecification<TEntity>(specification);
		}

		/// <summary>
		/// Override operator false, only for support AND OR operators
		/// </summary>
		/// <param name="specification">AbstractSpecification instance</param>
		/// <returns>See False operator in C#</returns>
		public static bool operator false(AbstractSpecification<TEntity> specification) {
			return false;
		}
		/// <summary>
		/// Override operator True, only for support AND OR operators
		/// </summary>
		/// <param name="specification">AbstractSpecification instance</param>
		/// <returns>See True operator in C#</returns>
		public static bool operator true(AbstractSpecification<TEntity> specification) {
			return true;
		}

	}

}