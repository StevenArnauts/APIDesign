using System;
using System.Collections.Generic;
using System.Linq;
using UserEntity = Server.Persistence.User;

namespace Server.Domain {

	public class User : DomainObject<UserEntity> {

		public User(UserEntity entity) : base(entity) {}

		public string Name {
			get { return (this.Entity.Name); }
		}

		public string Email {
			get { return (this.Entity.Email); }
		}

		public IEnumerable<Claim> Assignments {
			get { return this.Entity.Claims.Select(e => new Claim(e)); }
		}

		public bool PasswordMatches(string password) {
			return (this.Entity.Password.Equals(password, StringComparison.InvariantCulture));
		}

	}

}