using System;
using Server.Persistence;

namespace Server.Domain {

	public abstract class DomainObject<TEntityType> where TEntityType : Entity {

		protected DomainObject(TEntityType entity) {
			this.Entity = entity;
		}

		public Guid Id {
			get { return (this.Entity.Id); }
		}

		public TEntityType Entity { get; }

	}

}
