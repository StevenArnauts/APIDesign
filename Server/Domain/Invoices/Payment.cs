using System;
using PaymentEntity = Server.Persistence.Payment;

namespace Server.Domain {

	public class Payment : DomainObject<PaymentEntity> {

		public Payment(PaymentEntity entity) : base(entity) {}

		public DateTimeOffset Date {
			get { return (this.Entity.Date); }
		}

		public decimal Amount {
			get { return (this.Entity.Amount); }
		}

	}

}