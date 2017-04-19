using System;

namespace Server.Persistence {

	public class Payment : Entity {

		public DateTimeOffset Date { get; set; }
		public decimal Amount { get; set; }
		public Guid InvoiceId { get; set; }

		public virtual Invoice Invoice { get; set; }

	}

}