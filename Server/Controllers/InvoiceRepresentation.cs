using System;
using System.Collections.Generic;

namespace Server.Controllers {

	public class InvoiceRepresentation : EntityRepresentation {

		public DateTimeOffset Date { get; set; }
		public string State { get; set; }
		public Guid OrderId { get; set; }

	}

	public class InvoiceDetailsRepresentation : InvoiceRepresentation {

		public IEnumerable<PaymentRepresentation> Payments { get; set; }

	}

}