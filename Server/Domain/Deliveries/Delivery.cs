using System;
using DeliveryEntity = Server.Persistence.Delivery;

namespace Server.Domain {

	public class Delivery : DomainObject<DeliveryEntity> {

		public Delivery(DeliveryEntity entity) : base(entity) {}

		public DateTimeOffset Date {
			get { return (this.Entity.Date); }
		}

		public BackOrder BackOrder {
			get { return (new BackOrder(this.Entity.BackOrder)); }
		}

	}

}
