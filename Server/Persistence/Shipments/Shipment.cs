using System;

namespace Server.Persistence {

	public class Shipment : Entity {

		public DateTimeOffset Date { get; set; }
		public Guid OrderId { get; set; }
		public virtual Order Order { get; set; }

	}

}