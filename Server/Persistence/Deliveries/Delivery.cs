using System;

namespace Server.Persistence {

	public class Delivery : Entity {

		public DateTimeOffset Date { get; set; }
		public Guid BackOrderId { get; set; }
		public virtual BackOrder BackOrder { get; set; }

	}

}