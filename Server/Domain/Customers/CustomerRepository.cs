using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using CustomerEntity = Server.Persistence.Customer;

namespace Server.Domain {

	public interface ICustomerRepository {

		Customer Get(Guid id);
		IEnumerable<Customer> All();

	}

	public class CustomerRepository : DomainRepository<Customer, CustomerEntity>, ICustomerRepository {

		public CustomerRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Customers) {}

	}

}