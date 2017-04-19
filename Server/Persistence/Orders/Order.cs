using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Extensions;

namespace Server.Persistence {

	public class Order : Entity {

		public Order() {
			this.Shipments = new List<Shipment>();
			this.OrderLines = new List<OrderLine>();
			this.Invoices = new List<Invoice>();
		}

		public string Description { get; set; }
		public DateTimeOffset Date { get; set; }
		public Guid CustomerId { get; set; }
		public decimal Amount { get; set; }
		[NotMapped]
		public OrderState State { get; set; }
		public string StateString {
			get { return this.State.ToString(); }
			set {this.State = value.ToEnum<OrderState>(); }
		}
		public virtual Customer Customer { get; set; }
		public virtual ICollection<OrderLine> OrderLines { get; set; }
		public virtual ICollection<Shipment> Shipments { get; set; }
		public virtual ICollection<Invoice> Invoices { get; set; }

	}

}