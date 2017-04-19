using System.Collections.Generic;

namespace Server.Persistence {

	public class Customer : Entity {

		public Customer()  {
			this.Orders = new List<Order>();
		}

		public string Name { get; set; }
		public virtual ICollection<Order> Orders { get; set; }

	}

}