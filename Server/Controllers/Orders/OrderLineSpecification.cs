using System;

namespace Server.Controllers {

	public class OrderLineSpecification {

		public Guid ProductId { get; set; }
		public int Quantity { get; set; }

	}

}
