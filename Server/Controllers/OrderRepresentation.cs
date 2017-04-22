using System;
using System.Collections.Generic;
using System.Linq;
using Server.Domain;
using Utilities;

namespace Server.Controllers {

	public class OrderRepresentation : EntityRepresentation {

		public string Description { get; set; }
		public DateTimeOffset Date { get; set; }
		public string State { get; set; }
		public decimal Amount { get; set; }
		public CustomerRepresentation Customer { get; set; }

		public virtual OrderRepresentation SetupHyperMediaFactory(Order order, IRouteLinker linker) {
			this.HyperMedia.Factory = new OrderHyperMediaFactory(order, linker).Setup;
			return (this);
		}

		public class OrderHyperMediaFactory : HyperMediaFactory {

			protected readonly Order _order;

			public OrderHyperMediaFactory(Order order, IRouteLinker linker) : base(linker) {
				this._order = order;
			}

			public void Setup(Representation order) {
				order.Links.AddSafe("self", new Link(this.Link("ORDER_GET", new {
					orderId = this._order.Id
				})));
			}

		}

	}

	public class OrderDetailRepresentation : OrderRepresentation {

		public IEnumerable<OrderLineRepresentation> Lines { get; set; }

		public new OrderDetailRepresentation SetupHyperMediaFactory(Order order, IRouteLinker linker) {
			this.HyperMedia.Factory = new OrderDetailsHyperMediaFactory(order, linker).Setup;
			return (this);
		}

		public class OrderDetailsHyperMediaFactory : OrderHyperMediaFactory {

			public OrderDetailsHyperMediaFactory(Order order, IRouteLinker linker) : base(order, linker) {}

			public new void Setup(Representation order) {
				base.Setup(order);
				order.Links.AddSafe("invoices", new LinkCollection(this._order.Invoices.Select(i => new Link(this.Link("INVOICES_GET", new {
					invoiceId = i.Id
				})))));
			}

		}

	}

}