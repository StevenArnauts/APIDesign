using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Extensions;

namespace Server.Persistence {

	public class Invoice : Entity {

		public DateTimeOffset Date { get; set; }
		public Guid OrderId { get; set; }
		[NotMapped]
		public InvoiceState State { get; set; }

		public string StateString {
			get { return (this.State.ToString()); }
			set { this.State = value.ToEnum<InvoiceState>(); }
		}

		public virtual Order Order { get; set; }

		public virtual ICollection<Payment> Payments { get; set; }

	}

}