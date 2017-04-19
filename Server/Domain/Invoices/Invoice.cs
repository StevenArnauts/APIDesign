using System;
using System.Collections.Generic;
using System.Linq;
using Server.Persistence;
using Utilities;
using InvoiceEntity = Server.Persistence.Invoice;
using PaymentEntity = Server.Persistence.Payment;

namespace Server.Domain {

	public class Invoice : DomainObject<InvoiceEntity> {

		public Invoice(InvoiceEntity entity) : base(entity) {}

		public DateTimeOffset Date {
			get { return (this.Entity.Date); }
		}

		public InvoiceState State {
			get { return (this.Entity.State); }
		}

		public Order Order {
			get { return (new Order(this.Entity.Order)); }
		}

		public IEnumerable<Payment> Payments {
			get { return (this.Entity.Payments.Select(p => new Payment(p))); }
		}

		public void Send() {
			this.Entity.State = InvoiceState.Sent;
		}

		public Payment AddPayment(DateTimeOffset date, decimal amount) {
			if (this.Entity.State != InvoiceState.Sent) throw new OperationNotAllowedException("An invoice cannot be paid when it's " + this.Entity.State);
			decimal sum = this.Entity.Payments.Sum(p => p.Amount);
			PaymentEntity entity = new PaymentEntity {
				Amount = amount, Date = date
			};
			sum += entity.Amount;
			entity.Invoice = this.Entity;
			this.Entity.Payments.Add(entity);
			if (sum == this.Entity.Order.Amount) this.Entity.State = InvoiceState.Paid;
			if (sum > this.Entity.Order.Amount) throw new OperationNotAllowedException("The total amount paid " + sum + " cannot exceed the amount due of " + this.Entity.Order.Amount);
			return (new Payment(entity));
		}

	}

}
