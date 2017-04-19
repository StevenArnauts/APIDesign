using System;
using Server.Persistence;
using Utilities.Entities;
using BackOrderEntity = Server.Persistence.BackOrder;

namespace Server.Domain {

	public interface IBackOrderFactory : IFactory {

		BackOrder Create(BackOrderState state, Product product, DateTimeOffset date);

	}

	public class BackOrderFactory : BaseFactory<IDatabaseContext>, IBackOrderFactory {

		public BackOrderFactory(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork) {}

		public BackOrder Create(BackOrderState state, Product product, DateTimeOffset date) {
			BackOrderEntity entity = new BackOrderEntity {
				State = state,
				Product = product.Entity,
				Date = date
			};
			this.UnitOfWork.Context.BackOrders.Add(entity);
			return new BackOrder(entity);
		}

	}

}