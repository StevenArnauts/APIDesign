namespace Server.Persistence {

	public class CustomerMap : EntityMap<Customer> {

		public CustomerMap() {
			this.ToTable("Customer", "dbo");
			this.Property(x => x.Name).HasColumnName("Name").IsRequired();
			this.HasMany(x => x.Orders);
		}

	}

}