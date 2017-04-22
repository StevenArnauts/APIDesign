using Server.Persistence;
using Utilities.Entities;
using CustomerEntity = Server.Persistence.Customer;

namespace Server.Domain {

	public interface ICustomerFactory : IFactory {

		Customer Create(string name);

	}

	public class CustomerFactory : BaseFactory<IDatabaseContext>, ICustomerFactory {

		public CustomerFactory(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork) {}

		public Customer Create(string name) {
			CustomerEntity entity = new CustomerEntity {
				Name = name
			};
			this.UnitOfWork.Context.Customers.Add(entity);
			return new Customer(entity);
		}

	}

}