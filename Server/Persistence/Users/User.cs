using System.Collections.Generic;

namespace Server.Persistence {

	public class User : Entity {

		public User() {
			this.Claims = new List<Claim>();
		}

		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public virtual ICollection<Claim> Claims { get; set; }

	}

}