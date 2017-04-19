using System;
using System.Collections.Generic;

namespace Server.Controllers {

	public class OrderRepresentation : EntityRepresentation {

		public string Description { get; set; }
		public DateTimeOffset Date { get; set; }
		public string State { get; set; }
		public decimal Amount { get; set; }
		public CustomerRepresentation Customer { get; set; }

	}

	public class OrderDetailRepresentation : OrderRepresentation {

		public IEnumerable<OrderLineRepresentation> Lines { get; set; }

	}

}