using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using OrderEntity = Server.Persistence.Order;

namespace Server.Domain {

	public interface IOrderRepository {

		IEnumerable<Order> All();
		IEnumerable<Order> FindByCustomer(Customer customer);
		Order Get(Guid id);

	}

	public class OrderRepository : DomainRepository<Order, OrderEntity>, IOrderRepository {

		public OrderRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Orders) {}

		public IEnumerable<Order> FindByCustomer(Customer customer) {
			return (this.Query(o => o.CustomerId == customer.Id));
		}

	}

}