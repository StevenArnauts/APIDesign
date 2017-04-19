using System;
using ShipmentEntity = Server.Persistence.Shipment;

namespace Server.Domain {

	public class Shipment : DomainObject<ShipmentEntity> {

		public Shipment(ShipmentEntity entity) : base(entity) {}

		public DateTimeOffset Date {
			get { return (this.Entity.Date); }
			set { this.Entity.Date = value; }
		}

		public Order Order {
			get { return (new Order(this.Entity.Order)); }
		}

	}

}