using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Server.Infrastructure;
using Server.Persistence;
using Utilities;

namespace Server.Domain {

	public interface IDomainRepository {

	}

	public abstract class DomainRepository<TDomainType, TEntityType> : IDomainRepository where TEntityType : Entity where TDomainType : DomainObject<TEntityType> {

		protected DomainRepository(IDbSet<TEntityType> entities) {
			this.Entities = entities;
		}

		public IDbSet<TEntityType> Entities { get; }

		public IEnumerable<TDomainType> All() {
			return this.Entities.ToList().Select(this.CreateInstance);
		}

		public IEnumerable<TDomainType> Query(Expression<Func<TEntityType, bool>> condition) {
			IEnumerable<TEntityType> queryable = this.Entities.Where(condition);
			return queryable.Select(this.CreateInstance);
		}

		public TDomainType Get(Guid id) {
			TEntityType entity = this.Entities.Get(p => p.Id == id);
			return (this.CreateInstance(entity));
		}

		public TDomainType FirstOrDefault(Expression<Func<TEntityType, bool>> condition) {
			TEntityType entity = this.Entities.FirstOrDefault(condition);
			return (this.CreateInstance(entity));
		}

		public void Delete(Guid id) {
			TEntityType entity = this.Entities.Get(p => p.Id == id);
			this.Entities.Remove(entity);
		}

		protected virtual TDomainType CreateInstance(TEntityType entity) {
			var instance = Activator.CreateInstance(typeof (TDomainType), entity) as TDomainType;
			return (instance);
		}

	}

}