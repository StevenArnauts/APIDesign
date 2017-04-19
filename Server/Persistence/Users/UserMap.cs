namespace Server.Persistence {

	public class UserMap : EntityMap<User> {

		public UserMap() {
			this.ToTable("User", "dbo");
			this.Property(x => x.Name).HasColumnName("Name").IsRequired();
			this.Property(x => x.Email).HasColumnName("Email").IsRequired();
			this.Property(x => x.Password).HasColumnName("Password").IsRequired();
			this.HasMany(x => x.Claims);
		}

	}

}