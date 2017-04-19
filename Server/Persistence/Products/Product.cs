using System.Collections.Generic;

namespace Server.Persistence {

	public class Product : Entity {

		public Product() {
			this.BackOrders = new List<BackOrder>();
			this.OrderLines = new List<OrderLine>();
		}

		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public virtual ICollection<BackOrder> BackOrders { get; set; }
		public virtual ICollection<OrderLine> OrderLines { get; set; }

	}

}