using System;
using Utilities;

namespace Server.Controllers {

	public abstract class EntityRepresentation : Representation {

		public Guid Id { get; set; }

	}

}