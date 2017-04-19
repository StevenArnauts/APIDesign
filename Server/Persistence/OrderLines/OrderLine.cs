using System;

namespace Server.Persistence {

	public class OrderLine : Entity {

		public Guid OrderId { get; set; }
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Amount { get; set; }

		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }

	}

}