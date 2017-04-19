namespace Server.Persistence {

	public class ClaimMap : EntityMap<Claim> {

		public ClaimMap() {
			this.ToTable("Claims", "dbo");
			this.Property(x => x.Name).HasColumnName("Name").IsRequired();
			this.Property(x => x.Value).HasColumnName("Value");
			this.HasRequired(x => x.User).WithMany(u => u.Claims).HasForeignKey(c => c.UserId);
		}

	}

}