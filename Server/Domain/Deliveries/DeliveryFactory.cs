using System;
using Server.Persistence;
using Utilities.Entities;
using DeliveryEntity = Server.Persistence.Delivery;

namespace Server.Domain {

	public interface IDeliveryFactory : IFactory {

		Delivery Create(BackOrder backOrder, DateTimeOffset date);

	}

	public class DeliveryFactory : BaseFactory<IDatabaseContext>, IDeliveryFactory {

		public DeliveryFactory(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork) {}

		public Delivery Create(BackOrder backOrder, DateTimeOffset date) {
			DeliveryEntity entity = new DeliveryEntity {
				BackOrder = backOrder.Entity,
				Date = date
			};
			this.UnitOfWork.Context.Deliveries.Add(entity);
			return new Delivery(entity);
		}

	}

}