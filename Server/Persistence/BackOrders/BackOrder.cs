using System;
using System.Collections.Generic;

namespace Server.Persistence {

	public enum BackOrderState {

		Created,
		Delivered

	}

	public class BackOrder : Entity {

		public BackOrder() {
			this.Deliveries = new List<Delivery>();
		}

		public DateTimeOffset Date { get; set; }
		public BackOrderState State { get; set; }
		public Guid ProductId { get; set; }
		public virtual Product Product { get; set; }
		public virtual ICollection<Delivery> Deliveries { get; set; }

	}

}