namespace Server.Persistence {

	public class OrderMap : EntityMap<Order> {

		public OrderMap() {
			this.ToTable("Order", "dbo");
			this.Property(x => x.Description).HasColumnName("Description").IsRequired();
			this.Property(x => x.Date).HasColumnName("Date").IsRequired();
			this.Property(x => x.StateString).HasColumnName("State").IsRequired();
			this.Property(x => x.Amount).HasColumnName("Amount").IsRequired();
			this.Property(x => x.CustomerId).HasColumnName("CustomerFK");
			this.HasRequired(x => x.Customer).WithMany(t => t.Orders).HasForeignKey(t => t.CustomerId);
			this.HasMany(x => x.OrderLines).WithRequired(o => o.Order).HasForeignKey(ol => ol.OrderId);
			this.HasMany(x => x.Shipments).WithRequired(o => o.Order).HasForeignKey(ol => ol.OrderId);
			this.HasMany(x => x.Invoices).WithRequired(o => o.Order).HasForeignKey(i => i.OrderId);
		}

	}

}