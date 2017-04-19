using System;

namespace Server.Persistence {

	public class Claim : Entity {

		public string Name { get; set; }
		public string Value { get; set; }
		public Guid UserId { get; set; }
		public virtual User User { get; set; }

	}

}