using System;
using System.Collections.Generic;
using System.Linq;
using Server.Persistence;
using Utilities;
using Utilities.Extensions;
using OrderEntity = Server.Persistence.Order;
using OrderLineEntity = Server.Persistence.OrderLine;

namespace Server.Domain {

	public class Order : DomainObject<OrderEntity> {

		public Order(OrderEntity entity) : base(entity) {}

		public string Description {
			get { return (this.Entity.Description); }
		}

		public DateTimeOffset Date {
			get { return (this.Entity.Date); }
		}

		public OrderState State {
			get { return (this.Entity.State); }
		}

		public decimal Amount {
			get { return (this.Entity.Amount); }
		}

		public Customer Customer {
			get { return (new Customer(this.Entity.Customer)); }
		}

		public IEnumerable<OrderLine> Lines {
			get { return (this.Entity.OrderLines.Select(l => new OrderLine(l))); }
		}

		public IEnumerable<Invoice> Invoices {
			get { return (this.Entity.Invoices.Select(i => new Invoice(i))); }
		}

		public OrderLine AddLine(Product product, int quantity) {
			if (quantity <= 0) throw new OperationNotAllowedException("At least one product must be ordered");
			decimal amount = product.Price*quantity;
			OrderLineEntity entity = this.Entity.OrderLines.FirstOrDefault(l => l.ProductId == product.Id);
			if (entity != null) {
				entity.Quantity += quantity;
				entity.Amount += amount;
			} else {
				entity = new OrderLineEntity {
					Id = Guid.NewGuid(), Amount = amount, Quantity = quantity, Product = product.Entity, Order = this.Entity
				};
				this.Entity.OrderLines.Add(entity);
			}
			this.Entity.Amount += amount;
			return (new OrderLine(entity));
		}

		public void Confirm() {
			if(this.Entity.State != OrderState.Created) throw new OperationNotAllowedException("Order " + this.Entity.Id.Format() + " cannot be confirmed because it is " + this.Entity.State);
			this.Entity.State = OrderState.Confirmed;
		}

	}

}
