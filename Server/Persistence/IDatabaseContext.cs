using System.Data.Entity;
using Utilities.Entities;

namespace Server.Persistence {

	public interface IDatabaseContext : IDbContext {

		IDbSet<Order> Orders { get; set; }
		IDbSet<OrderLine> OrderLines { get; set; }
		IDbSet<Customer> Customers { get; set; }
		IDbSet<User> Users { get; set; }
		IDbSet<Claim> Claims { get; set; }
		IDbSet<BackOrder> BackOrders { get; set; }
		IDbSet<Delivery> Deliveries { get; set; }
		IDbSet<Invoice> Invoices { get; set; }
		IDbSet<Payment> Payments { get; set; }
		IDbSet<Product> Products { get; set; }
		IDbSet<Shipment> Shipments { get; set; }

	}

}