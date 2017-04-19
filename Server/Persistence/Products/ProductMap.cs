namespace Server.Persistence {

	public class ProductMap : EntityMap<Product> {

		public ProductMap() {
			this.ToTable("Product", "dbo");
			this.Property(x => x.Name).HasColumnName("Name").IsRequired();
			this.Property(x => x.Price).HasColumnName("Price").IsRequired();
			this.Property(x => x.Stock).HasColumnName("Stock").IsRequired();
		}

	}

}