using System;

namespace Server.Controllers {

	public class OrderLineRepresentation : EntityRepresentation {

		public Guid ProductId { get; set; }
		public decimal Amount { get; set; }
		public int Quantity { get; set; }


	}

}
