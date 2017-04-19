using System;

namespace Server.Controllers {

	public class PaymentRepresentation : EntityRepresentation {

		public DateTimeOffset Date { get; set; }
		public decimal Amount { get; set; }

	}

}