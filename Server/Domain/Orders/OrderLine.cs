using OrderLineEntity = Server.Persistence.OrderLine;

namespace Server.Domain {

	public class OrderLine : DomainObject<OrderLineEntity> {

		public OrderLine(OrderLineEntity entity) : base(entity) {}

		public decimal Amount {
			get { return (this.Entity.Amount); }
		}

		public int Quantity{
			get { return (this.Entity.Quantity); }
		}

		public Product Product {
			get { return (new Product(this.Entity.Product)); }
		}

	}

}