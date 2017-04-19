using System;

namespace Server.Controllers {

	public class PaymentSpecification {

		public DateTimeOffset Date { get; set; }
		public decimal Amount { get; set; }

	}

}