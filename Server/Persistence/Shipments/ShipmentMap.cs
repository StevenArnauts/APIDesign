namespace Server.Persistence {

	public class ShipmentMap : EntityMap<Shipment> {

		public ShipmentMap() {
			this.ToTable("Shipment", "dbo");
			this.Property(x => x.Date).HasColumnName("Date").IsRequired();
			this.Property(x => x.OrderId).HasColumnName("OrderFk").IsRequired();
			this.HasRequired(x => x.Order).WithMany(p => p.Shipments).HasForeignKey(p => p.OrderId);
		}

	}

}