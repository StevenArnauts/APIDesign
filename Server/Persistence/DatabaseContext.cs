using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using Utilities.Entities;
using Utilities.Logging;

namespace Server.Persistence {

	public class DatabaseContext : BaseContext, IDatabaseContext {

		public const string SCHEMA_NAME = "dbo";
		public DatabaseContext(string connectionString) : base(connectionString) {}
		public IDbSet<Order> Orders { get; set; }
		public IDbSet<OrderLine> OrderLines { get; set; }
		public IDbSet<Customer> Customers { get; set; }
		public IDbSet<User> Users { get; set; }
		public IDbSet<Claim> Claims { get; set; }
		public IDbSet<BackOrder> BackOrders { get; set; }
		public IDbSet<Delivery> Deliveries { get; set; }
		public IDbSet<Invoice> Invoices { get; set; }
		public IDbSet<Payment> Payments { get; set; }
		public IDbSet<Product> Products { get; set; }
		public IDbSet<Shipment> Shipments { get; set; }

		public override int SaveChanges() {
			try {
				string currentUser = Thread.CurrentPrincipal.Identity.Name;
				if (string.IsNullOrEmpty(currentUser)) {
					currentUser = "Anonymous";
				}
				DateTimeOffset now = DateTimeOffset.Now;
				foreach (DbEntityEntry<Entity> changeEntry in this.ChangeTracker.Entries<Entity>()) {
					if (changeEntry.State == EntityState.Added) {
						changeEntry.Entity.CreatedBy = currentUser;
						changeEntry.Entity.CreatedOn = now;
						changeEntry.Entity.ModifiedBy = currentUser;
						changeEntry.Entity.ModifiedOn = now;
					} else if (changeEntry.State == EntityState.Modified) {
						changeEntry.Entity.ModifiedBy = currentUser;
						changeEntry.Entity.ModifiedOn = now;
					}
				}
				return base.SaveChanges();
			} catch (DbEntityValidationException ex) {
				Logger.Error(this, "Entity validation failed", ex);
				foreach (DbEntityValidationResult e in ex.EntityValidationErrors) {
					Logger.Error(this, "Entity: " + e.Entry.Entity.GetType().FullName);
					foreach (DbValidationError x in e.ValidationErrors) Logger.Error(this, "\tProperty" + x.PropertyName + ": " + x.ErrorMessage);
				}
				throw;
			} catch (Exception ex) {
				Logger.Error(this, "Saving changes failed", ex);
				throw;
			}
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Configurations.Add(new OrderMap());
			modelBuilder.Configurations.Add(new OrderLineMap());
			modelBuilder.Configurations.Add(new CustomerMap());
			modelBuilder.Configurations.Add(new UserMap());
			modelBuilder.Configurations.Add(new ClaimMap());
			modelBuilder.Configurations.Add(new BackOrderMap());
			modelBuilder.Configurations.Add(new DeliveryMap());
			modelBuilder.Configurations.Add(new InvoiceMap());
			modelBuilder.Configurations.Add(new PaymentMap());
			modelBuilder.Configurations.Add(new ProductMap());
			modelBuilder.Configurations.Add(new ShipmentMap());
		}

	}

}
