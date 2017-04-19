using AutoMapper;
using Server.Controllers;
using Server.Domain;

namespace Server.Configuration {

	internal class MapperConfig {

		internal static MapperConfiguration Configure() {
			MapperConfiguration config = new MapperConfiguration(cfg => {
				cfg.CreateMap<Product, ProductRepresentation>();
				cfg.CreateMap<Customer, CustomerRepresentation>();
				cfg.CreateMap<Order, OrderRepresentation>();
				cfg.CreateMap<Order, OrderDetailRepresentation>();
				cfg.CreateMap<OrderLine, OrderLineRepresentation>();
				cfg.CreateMap<Invoice, InvoiceRepresentation>();
				cfg.CreateMap<Invoice, InvoiceDetailsRepresentation>();
				cfg.CreateMap<Payment, PaymentRepresentation>();
				cfg.CreateMap<Shipment, ShipmentRepresentation>();
			});
			return (config);
		}

	}

}