using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Utilities.Entities {

	public interface IDbEntry {

		IDbCollection Collection(string navigationProperty);

	}

	public interface IDbEntry<TEntity> where TEntity : class {

		IDbCollection Collection<TElement>(Expression<Func<TEntity, ICollection<TEntity>>> navigationProperty);

	}

}