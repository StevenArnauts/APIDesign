using System;

namespace Server.Domain {

	public class OrderConfirmationService : IDomainService {

		private readonly IInvoiceFactory _invoiceFactory;

		public OrderConfirmationService(IInvoiceFactory invoiceFactory) {
			this._invoiceFactory = invoiceFactory;
		}

		public void ConfirmOrder(Order order) {
			this._invoiceFactory.Create(order, DateTimeOffset.Now);
			order.Confirm();
		}

	}

}