using System;

namespace Portal {
	
	public class Money {

		public Money() {}

		public Money(decimal amount, string currency) {
			this.Amount = amount;
			this.Currency = currency;
		}

		public string Currency { get; set; }
		public decimal Amount { get; set; }

		public static Money operator +(Money leftOperand, object rightOperand) {
			if(rightOperand is Money) {
				if(string.Equals(leftOperand.Currency, ( rightOperand as Money ).Currency)) {
					return new Money(leftOperand.Amount + ( rightOperand as Money ).Amount, leftOperand.Currency);
				}
			}
			if(rightOperand is decimal) {
				return new Money(leftOperand.Amount + (decimal)rightOperand, leftOperand.Currency);
			}
			throw new InvalidOperationException("+ operator is only supported for 2 instances of Money or Money and Decimal!");
		}

	}

}