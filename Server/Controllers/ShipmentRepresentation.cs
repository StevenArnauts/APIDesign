using System;

namespace Server.Controllers {

	public class ShipmentRepresentation : EntityRepresentation {

		public DateTimeOffset Date { get; set; }
		public Guid OrderId { get; set; }

	}

}