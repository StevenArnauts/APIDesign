namespace Server.Persistence {

	public class OrderLineMap : EntityMap<OrderLine> {

		public OrderLineMap() {
			this.ToTable("OrderLine", "dbo");
			this.Property(x => x.OrderId).HasColumnName("OrderFk").IsRequired();
			this.Property(x => x.ProductId).HasColumnName("ProductFk").IsRequired();
			this.Property(x => x.Amount).HasColumnName("Amount").IsRequired();
			this.Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
			this.HasRequired(x => x.Order).WithMany(o => o.OrderLines).HasForeignKey(l => l.OrderId);
			this.HasRequired(x => x.Product).WithMany(o => o.OrderLines).HasForeignKey(l => l.ProductId);
		}

	}

}