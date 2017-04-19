namespace Server.Persistence {

	public class PaymentMap : EntityMap<Payment> {

		public PaymentMap() {
			this.ToTable("Payment", "dbo");
			this.Property(x => x.Date).HasColumnName("Date").IsRequired();
			this.Property(x => x.Amount).HasColumnName("Amount").IsRequired();
			this.Property(x => x.InvoiceId).HasColumnName("InvoiceFk").IsRequired();
			this.HasRequired(x => x.Invoice).WithMany(o => o.Payments).HasForeignKey(p => p.InvoiceId);
		}

	}

}