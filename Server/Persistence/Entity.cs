using System;

namespace Server.Persistence {

	public class Entity {

		public Guid Id { get; set; }
		public DateTimeOffset CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public DateTimeOffset ModifiedOn { get; set; }
		public string ModifiedBy { get; set; }

	}

}