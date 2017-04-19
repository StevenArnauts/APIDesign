using System.Collections.Generic;
using System.Linq;
using CustomerEntity = Server.Persistence.Customer;

namespace Server.Domain {

	public class Customer : DomainObject<CustomerEntity> {

		public Customer(CustomerEntity entity) : base(entity) {}

		public string Name {
			get { return (this.Entity.Name); }
			set { this.Entity.Name = value; }
		}

		public IEnumerable<Order> Orders {
			get { return this.Entity.Orders.Select(e => new Order(e)); }
		}

	}

}