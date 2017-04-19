using System.Diagnostics.CodeAnalysis;

namespace Utilities.Specifications {

    /// <summary>
    /// Base class for composite specifications
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this AbstractSpecification</typeparam>
	[ExcludeFromCodeCoverage]
	public abstract class CompositeSpecification<TEntity> : AbstractSpecification<TEntity> where TEntity : class {

        /// <summary>
        /// Left side AbstractSpecification for this composite element
        /// </summary>
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Right side AbstractSpecification for this composite element
        /// </summary>
        public abstract ISpecification<TEntity> RightSideSpecification { get; }

    }

}
