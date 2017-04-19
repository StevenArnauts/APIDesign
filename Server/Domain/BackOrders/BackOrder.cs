using System;
using Server.Persistence;
using BackOrderEntity = Server.Persistence.BackOrder;

namespace Server.Domain {

	public class BackOrder : DomainObject<BackOrderEntity> {

		public BackOrder(BackOrderEntity entity) : base(entity) {}

		public BackOrderState State {
			get { return (this.Entity.State); }
		}

		public DateTimeOffset Date {
			get { return (this.Entity.Date); }
		}

		public Product Product {
			get { return (new Product(this.Entity.Product)); }
		}

	}

}
