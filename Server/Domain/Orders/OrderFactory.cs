using System;
using Server.Persistence;
using Utilities.Entities;
using OrderEntity = Server.Persistence.Order;

namespace Server.Domain {

	public interface IOrderFactory : IFactory {

		Order Create(Customer customer, string description, DateTimeOffset? date);

	}

	public class OrderFactory : BaseFactory<IDatabaseContext>, IOrderFactory {

		public OrderFactory(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork) {}

		public Order Create(Customer customer, string description, DateTimeOffset? date) {
			DateTimeOffset d = date ?? DateTimeOffset.Now;
			OrderEntity entity = new OrderEntity {
				Description = description,
				Customer = customer.Entity,
				Date = d,
				Amount = 0,
				State = OrderState.Created
			};
			this.UnitOfWork.Context.Orders.Add(entity);
			this.UnitOfWork.Context.SaveChanges();
			return new Order(entity);
		}

	}

}