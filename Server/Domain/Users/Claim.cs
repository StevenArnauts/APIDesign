using ClaimEntity = Server.Persistence.Claim;

namespace Server.Domain {

	public class Claim : DomainObject<ClaimEntity> {

		public Claim(ClaimEntity entity) : base(entity) {}

		public string Name {
			get { return (this.Entity.Name); }
		}

		public string Value {
			get { return (this.Entity.Value); }
		}

	}

}