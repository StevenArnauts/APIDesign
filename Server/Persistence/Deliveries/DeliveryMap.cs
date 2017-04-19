namespace Server.Persistence {

	public class DeliveryMap : EntityMap<Delivery> {

		public DeliveryMap() {
			this.ToTable("Delivery", "dbo");
			this.Property(x => x.Date).HasColumnName("Date").IsRequired();
			this.Property(x => x.BackOrderId).HasColumnName("BackOrderFk").IsRequired();
			this.HasRequired(x => x.BackOrder).WithMany(b => b.Deliveries).HasForeignKey(d => d.BackOrderId);
		}

	}

}