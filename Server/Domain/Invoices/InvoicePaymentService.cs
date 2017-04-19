using System;
using Server.Persistence;

namespace Server.Domain {

	public class InvoicePaymentService : IDomainService {

		private readonly IShipmentFactory _shipmentFactory;

		public InvoicePaymentService(IShipmentFactory shipmentFactory) {
			this._shipmentFactory = shipmentFactory;
		}

		public Payment AddPayment(Invoice invoice, decimal amount, DateTimeOffset date) {
			Payment payment = invoice.AddPayment(date, amount);
			if (invoice.State == InvoiceState.Paid) {
				this._shipmentFactory.Create(invoice.Order, DateTimeOffset.Now);
			}
			return (payment);
		}

	}

}