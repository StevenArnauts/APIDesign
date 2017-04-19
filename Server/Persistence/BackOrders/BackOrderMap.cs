namespace Server.Persistence {

	public class BackOrderMap : EntityMap<BackOrder> {

		public BackOrderMap() {
			this.ToTable("BackOrder", "dbo");
			this.Property(x => x.Date).HasColumnName("Date").IsRequired();
			this.Property(x => x.State).HasColumnName("State").IsRequired();
			this.Property(x => x.ProductId).HasColumnName("ProductFk").IsRequired();
			this.HasRequired(x => x.Product).WithMany(p => p.BackOrders).HasForeignKey(p => p.ProductId);
			this.HasMany(x => x.Deliveries);
		}

	}

}