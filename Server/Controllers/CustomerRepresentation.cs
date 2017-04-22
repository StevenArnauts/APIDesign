using System.Linq;
using Server.Domain;
using Utilities;

namespace Server.Controllers {

	public class CustomerRepresentation : EntityRepresentation {

		public string Name { get; set; }

		public CustomerRepresentation SetupHyperMediaFactory(Customer customer, IRouteLinker linker) {
			this.HyperMedia.Factory = new CustomerHyperMediaFactory(customer, linker).Setup;
			return (this);
		}

		public class CustomerHyperMediaFactory : HyperMediaFactory {

			private readonly Customer _customer;

			public CustomerHyperMediaFactory(Customer customer, IRouteLinker linker) : base(linker) {
				this._customer = customer;
			}

			public void Setup(Representation representation) {
				representation.AddLink("self", this.Link("CUSTOMER_GET", new {
					customerId = this._customer.Id
				}));
				representation.AddLink("orders", new LinkCollection(this._customer.Orders.Select(o => new Link(this.Link("ORDER_GET", new {
					orderId = o.Id
				})))));
			}

		}

	}

}