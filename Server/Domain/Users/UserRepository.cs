using System;
using Server.Persistence;
using Utilities.Entities;
using UserEntity = Server.Persistence.User;

namespace Server.Domain {

	public interface IUserRepository {

		User FindByName(string name);

	}

	public class UserRepository : DomainRepository<User, UserEntity>, IUserRepository {

		public UserRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Users) {}

		public User FindByName(string name) {
			User user = this.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			return (user);
		}

	}

}