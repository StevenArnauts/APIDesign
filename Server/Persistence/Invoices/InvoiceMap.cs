namespace Server.Persistence {

	public class InvoiceMap : EntityMap<Invoice> {

		public InvoiceMap() {
			this.ToTable("Invoice", "dbo");
			this.Property(x => x.Date).HasColumnName("Date").IsRequired();
			this.Property(x => x.OrderId).HasColumnName("OrderFk").IsRequired();
			this.Property(x => x.StateString).HasColumnName("State").IsRequired();
			this.HasRequired(x => x.Order).WithMany(p => p.Invoices).HasForeignKey(p => p.OrderId);
			this.HasMany(x => x.Payments).WithRequired(o => o.Invoice).HasForeignKey(ol => ol.InvoiceId);
		}

	}

}